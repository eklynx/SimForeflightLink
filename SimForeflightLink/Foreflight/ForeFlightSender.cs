using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace SimForeflightLink.Foreflight
{
    public class ForeFlightSender
    {
        public static readonly string DEFAULT_DEVICE_NAME = "MSFS 2020";
        private static readonly string GPS_MESSAGE_FORMAT = "XGPS{0},{1:F4},{2:F4},{3:F1},{4:F2},{5:F1}";
        private static readonly string ATTITUDE_MESSAGE_FORMAT = "XATT{0},{1:F1},{2:F1},{3:F1}";
        private static readonly string TRAFFIC_MESSAGE_FORMAT = "XTRAFFIC{0},{1},{2:F3},{3:F3},{4:F1},{5:F1},{6},{7:F1},{8:F1},{9}";

        public static readonly int DEFAULT_PORT = 49002;
        public static readonly int GPS_RATE_MS = 1000;
        public static readonly int ATTITUDE_RATE_MS = 150;
        public static readonly int TRAFFIC_SEND_RATE_MS = 100;

        private UdpClient udpClient;
        private readonly Timer gpsTimer;
        private readonly Timer attitudeTimer;
        private readonly Timer trafficTimer;

        public event EventHandler<ForeFlightErrorEventArgs> OnForeFlightSenderError;
        public class ForeFlightErrorEventArgs : EventArgs
        {
            public ForeFlightErrorEventArgs(string message)
            {
                Message = message;
            }
            public string Message { get; }
        }

        public string DeviceName { get; set; } = DEFAULT_DEVICE_NAME;
        public IPEndPoint EndPoint { get; set; }
        virtual protected FlightData FlightData { get; } // virtual for moq testing
        virtual protected ConcurrentDictionary<uint, TrafficData> TrafficDataMap { get; } // virtual for moq testing

        DateTime lastTrafficSend = DateTime.Now;

        public ForeFlightSender(ref FlightData flightData, ref ConcurrentDictionary<uint, TrafficData> trafficDataMap, UdpClient udpClient)
        {
            this.FlightData = flightData;
            this.udpClient = udpClient;
            this.TrafficDataMap = trafficDataMap;

            gpsTimer = new Timer(GPS_RATE_MS)
            {
                AutoReset = true
            };
            gpsTimer.Elapsed += (sender, args) => SendGps();

            attitudeTimer = new Timer(ATTITUDE_RATE_MS)
            {
                AutoReset = true
            };
            attitudeTimer.Elapsed += (sender, args) => SendAttiude();

            trafficTimer = new Timer(TRAFFIC_SEND_RATE_MS)
            {
                AutoReset = true
            };
            trafficTimer.Elapsed += (sender, args) => SendTraffic();
        }

        public void Start()
        {
            if (null == EndPoint)
            {
                throw new InvalidOperationException("The EndPoint has not been set");
            }
            gpsTimer.Start();
            attitudeTimer.Start();
            trafficTimer.Start();
        }

        public void Stop()
        {
            gpsTimer.Stop();
            attitudeTimer.Stop();
            trafficTimer.Stop();
        }

        protected void SendGps()
        {
            if (VerifyCompleteFlightData(FlightData) && null != EndPoint)
            {
                string gpsString = string.Format(
                    GPS_MESSAGE_FORMAT,
                    DeviceName,
                    FlightData.Longitude,
                    FlightData.Latitude,
                    FlightData.AltitudeMeters,
                    FlightData.GroundTrackDegrees,
                    FlightData.GroundSpeedMPS
                    );
                Send(gpsString);
            }
        }

        protected void SendAttiude()
        {
            if (VerifyCompleteFlightData(FlightData) && null != EndPoint)
            {
                string attitudeString = string.Format(
                    ATTITUDE_MESSAGE_FORMAT,
                    DeviceName,
                    FlightData.TrueHeadingDegrees,
                    FlightData.PitchDegrees,
                    FlightData.RollDegrees
                    );
                Send(attitudeString);
            }
        }

        Random rand = new Random();
        protected void SendTraffic()
        {
            if (TrafficDataMap.Count > 0 && null != EndPoint)
            {
                List<uint> keys = new List<uint>(TrafficDataMap.Count); 
                foreach (uint key in TrafficDataMap.Keys)
                {
                    keys.Add(key);
                }
                int debugLogMaxLines = 3;
                uint deleted = 0;
                uint sent = 0;
                uint ignored = 0;
                foreach (uint id in keys)
                {
                    TrafficData data = TrafficDataMap[id];
                    if (lastTrafficSend < data.LastSeen.AddSeconds(1) && VerifyCompleteTrafficData(data))
                    {
                        string callsign = data.Callsign.Length == 0 ? "Private" : data.Callsign.Replace(",", "_");
                        string trafficString = string.Format(
                            TRAFFIC_MESSAGE_FORMAT,
                            DeviceName,
                            data.ICAOAddress.Value,
                            data.Latitude.Value,
                            data.Longitude.Value,
                            data.Altitude.Value,
                            data.VerticalSpeed.Value,
                            data.IsAirborne.Value ? 1 : 0,
                            data.GroundTrackDegrees.Value,
                            data.Velocity.Value,
                            callsign
                            );
                        //if (rand.Next(3) == 1 && debugLogMaxLines-- > 0)
                        //    Debug.WriteLine(trafficString);
                        Send(trafficString);
                        ++sent;
                    } else
                    {
                        ignored++;
                    }
                    
                    if (lastTrafficSend > data.LastSeen.AddSeconds(3))
                    {
                        TrafficData rem;
                        TrafficDataMap.TryRemove(id, out rem);
                        ++deleted;
                    }
                }
                lastTrafficSend = DateTime.Now;
                if (deleted > 0)
                    Debug.WriteLine("Sent {0} Traffic lines, deleted {1}, ignored {2}", sent , deleted, ignored - deleted);
            }
        }

        virtual protected void Send(string message)
        {
            lock (this)
            {
                byte[] msg = Encoding.ASCII.GetBytes(message);
                try
                {
                    udpClient.Send(msg, msg.Length, EndPoint);
                }
                catch (SocketException)
                {
                    OnForeFlightSenderError?.Invoke(this, new ForeFlightErrorEventArgs(String.Format("Error sending to IP address {0}", EndPoint.Address)));
                }
            }
        }

        public static bool VerifyCompleteFlightData(FlightData flightData)
        {
            if (null == flightData.AltitudeFt)
                return false;
            if (null == flightData.GroundSpeedKt)
                return false;
            if (null == flightData.GroundTrackDegrees)
                return false;
            if (null == flightData.Latitude)
                return false;
            if (null == flightData.Longitude)
                return false;
            if (null == flightData.PitchDegrees)
                return false;
            if (null == flightData.RollDegrees)
                return false;
            if (null == flightData.TrueHeadingDegrees)
                return false;
            return true;
        }

        public static bool VerifyCompleteTrafficData(TrafficData trafficData)
        {
            if (null == trafficData.ICAOAddress)
                return false;
            if (null == trafficData.Latitude)
                return false;
            if (null == trafficData.Longitude)
                return false;
            if (null == trafficData.Altitude)
                return false;
            if (null == trafficData.VerticalSpeed)
                return false;
            if (null == trafficData.IsAirborne)
                return false;
            if (null == trafficData.GroundTrackDegrees)
                return false;
            if (null == trafficData.Velocity)
                return false;
            if (null == trafficData.Callsign)
                return false;
            return true;
        }
    }
}
