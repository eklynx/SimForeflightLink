﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace SimForeflightLink
{
    public class ForeFlightSender
    {
        public static readonly string DEFAULT_DEVICE_NAME = "MSFS 2020";
        private static readonly string GPS_MESSAGE_FORMAT = "XGPS{0},{1:F2},{2:F2},{3:F1},{4:F2},{5:F1}";
        private static readonly string ATTITUDE_MESSAGE_FORMAT = "XATT{0},{1:F1},{2:F1},{3:F1}";
        public static readonly int DEFAULT_PORT = 49002;
        public static readonly int GPS_RATE_MS = 1000;
        public static readonly int ATTITUDE_RATE_MS = 150;

        private UdpClient udpClient;
        private readonly Timer gpsTimer;
        private readonly Timer attitudeTimer;
        private readonly FlightData flightData;


        public string DeviceName { get; set; } = DEFAULT_DEVICE_NAME;
        public IPEndPoint EndPoint { get; set; }


        public ForeFlightSender(ref FlightData flightData, UdpClient udpClient)
        {
            this.flightData = flightData;
            this.udpClient = udpClient;

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
        }

        public void Start()
        {
            if (null == EndPoint)
            {
                throw new ArgumentNullException("The EndPoint has not been set");
            }
            gpsTimer.Start();
            attitudeTimer.Start();
        }

        public void Stop()
        {
            gpsTimer.Stop();
            attitudeTimer.Stop();
        }

        private void SendGps()
        {
            if (VerifyFlightData(flightData) && null != EndPoint)
            {
                string gpsString = string.Format(
                    GPS_MESSAGE_FORMAT,
                    DeviceName,
                    flightData.Longitude,
                    flightData.Latitude,
                    flightData.Altitude,
                    flightData.GroundTrack,
                    flightData.GroundSpeed
                    );
                Send(gpsString);
            }
        }

        private void SendAttiude()
        {
            if (VerifyFlightData(flightData) && null != EndPoint)
            {
                string attitudeString = string.Format(
                    ATTITUDE_MESSAGE_FORMAT,
                    DeviceName,
                    flightData.TrueHeading,
                    flightData.Pitch,
                    flightData.Roll
                    );
                Send(attitudeString);
            }
        }

        private void Send(string message)
        {
            lock (this)
            {
                Console.Out.WriteLine("Sending message ${message}");
                byte[] msg = Encoding.ASCII.GetBytes(message);
                udpClient.Send(msg, msg.Length, EndPoint);
            }
        }

        private static bool VerifyFlightData(FlightData flightData)
        {
            if (null == flightData.Altitude)
                return false;
            if (null == flightData.GroundSpeed)
                return false;
            if (null == flightData.GroundTrack)
                return false;
            if (null == flightData.Latitude)
                return false;
            if (null == flightData.Longitude)
                return false;
            if (null == flightData.Pitch)
                return false;
            if (null == flightData.Roll)
                return false;
            if (null == flightData.TrueHeading)
                return false;
            return true;
        }
    }
}