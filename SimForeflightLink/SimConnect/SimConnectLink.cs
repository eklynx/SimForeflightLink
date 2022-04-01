using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SimForeflightLink.SimConnectLink.SimConnectEventArgs;
using Timer = System.Windows.Forms.Timer;

namespace SimForeflightLink
{
    public class SimConnectLink
    {
        // Windows Message Simconnect user
        public static readonly uint WM_USER_SIMCONNECT = 0x5432;

        private static readonly string APP_NAME = "SimconnectForeflightLink";
        private const uint SIMCONNECT_OPEN_CONFIGINDEX_LOCAL = uint.MaxValue;

        private SimConnect simConnect;
        private readonly FlightData flightData;
        private readonly Dictionary<uint, TrafficData> trafficData = new Dictionary<uint, TrafficData>();

        private Timer simConnectPoller;

        private IntPtr handlePtr;
        private bool isActive;
        private DateTime waitUntil = DateTime.Now;

        public event EventHandler<SimConnectEventArgs> OnConnectionStatusChange;
        public class SimConnectEventArgs : EventArgs
        {
            public enum ConnectionEventType
            {
                Neutral,
                Connecting,
                Connected,
                Abnormal_Disconnect
            }
            public string Message { get; }
            public ConnectionEventType EventType { get; }
            internal SimConnectEventArgs(ConnectionEventType connectionEventType, string message)
            {
                Message = message;
                EventType = connectionEventType;
            }
        }


        private enum CustomStructs
        {
            SimConnectData,
            TrafficData
        }

        private enum Requests
        {
            MainRequest,
            TrafficRequest
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct SimConnectData
        {
            public double latitude;
            public double longitude;
            public double altitude;
            public double pitch;
            public double roll;
            public double groundTrack;
            public double trueHeading;
            public double groundSpeed;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct SimConnectTrafficData
        {
            public double latitude;
            public double longitude;
            public double altitude;
            public double verticalSpeed;
            public Int32 isAirborne;
            public double groundTrack;
            public double velocity;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst =128)]
            public string callsign;
        };

        public SimConnectLink(ref FlightData flightData)
        {
            this.flightData = flightData;

            simConnectPoller = new Timer()
            {
                Interval = 50,
                Enabled = false
            };
            simConnectPoller.Tick += SimConnectPoller_Elapsed;

            isActive = false;
        }

        private void SimConnectPoller_Elapsed(object sender, EventArgs e)
        {
            if (!isActive)
            {
                simConnectPoller.Stop();
                handlePtr = IntPtr.Zero;
                simConnect = null;
            }

            if (DateTime.Now < waitUntil)
                return;

            if (null == simConnect)
                InitConnection();

            if (null == simConnect)
                return;

            try
            {

                //-- Keeping this here for when they fix RequstDataOnSimObject() for testing
                //simConnect.RequestDataOnSimObject(
                //    Requests.MainRequest,
                //    CustomStructs.SimConnectData,
                //    SIMCONNECT_SIMOBJECT_TYPE.USER,
                //    SIMCONNECT_PERIOD.ONCE,
                //    0,
                //    0,
                //    0, 
                //    0
                //);

                simConnect.RequestDataOnSimObjectType(
                    Requests.MainRequest,
                    CustomStructs.SimConnectData,
                    0,
                    SIMCONNECT_SIMOBJECT_TYPE.USER
                );

                simConnect.RequestDataOnSimObjectType(
                    Requests.TrafficRequest,
                    CustomStructs.TrafficData,
                    10000, // 10km
                    SIMCONNECT_SIMOBJECT_TYPE.AIRCRAFT
                );
            }

            catch (COMException)
            {
                Disconnect("Lost Connection to SimConnect", true);
            }

        }

        private void RecieveSimConnectMessage(object sender, EventArgs e)
        {
            try
            {
                    simConnect?.ReceiveMessage();
            }
            catch (COMException)
            {
                Disconnect("Lost Connection to SimConnect", true);
            }
        }

        public void ReadMessages()
        {
            if (null != simConnect)
                simConnect.ReceiveMessage();
        }

        public void Connect(IntPtr handle)
        {
            handlePtr = handle;
            simConnectPoller.Start();
            isActive = true;

            //-- TODO: Once they fix RequstDataOnSimObject(), replace the simConnectPoller for the auto-sending every 5 frames.

        }

        public void InitConnection()
        {
            try
            {
                OnConnectionStatusChange(this, new SimConnectEventArgs(ConnectionEventType.Connecting, "Connecting to SimConnect"));

                simConnect = new SimConnect(
                    APP_NAME,
                    handlePtr,
                    WM_USER_SIMCONNECT,
                    null,
                    SIMCONNECT_OPEN_CONFIGINDEX_LOCAL
                );
                

                simConnect.OnRecvOpen += SimConnect_OnRecvOpen;
                simConnect.OnRecvQuit += SimConnect_OnRecvQuit;
                simConnect.OnRecvException += SimConnect_OnRecvException;
                simConnect.OnRecvSimobjectData += SimConnect_OnRecvSimobjectData;
                simConnect.OnRecvSimobjectDataBytype += SimConnect_OnRecvSimobjectData;

                simConnect.AddToDataDefinition(CustomStructs.SimConnectData, "Plane Latitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.00f, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(CustomStructs.SimConnectData, "Plane Longitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.00f, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(CustomStructs.SimConnectData, "Plane Altitude", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(CustomStructs.SimConnectData, "Plane Pitch Degrees", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(CustomStructs.SimConnectData, "Plane Bank Degrees", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(CustomStructs.SimConnectData, "GPS Ground True Track", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(CustomStructs.SimConnectData, "Plane Heading Degrees True", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(CustomStructs.SimConnectData, "Ground Velocity", "Knots", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                
                simConnect.RegisterDataDefineStruct<SimConnectData>(CustomStructs.SimConnectData);

                simConnect.AddToDataDefinition(CustomStructs.TrafficData, "Plane Latitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.00f, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(CustomStructs.TrafficData, "Plane Longitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0.00f, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(CustomStructs.TrafficData, "Plane Altitude", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(CustomStructs.TrafficData, "VERTICAL SPEED", "ft/min", SIMCONNECT_DATATYPE.FLOAT64, 0.0f, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(CustomStructs.TrafficData, "GEAR IS ON GROUND:1", "", SIMCONNECT_DATATYPE.INT32, 0.0f, SimConnect.SIMCONNECT_UNUSED); // maybe use PLANE IN PARKING STATE ?
                simConnect.AddToDataDefinition(CustomStructs.TrafficData, "GPS Ground True Track", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(CustomStructs.TrafficData, "Ground Velocity", "Knots", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simConnect.AddToDataDefinition(CustomStructs.TrafficData, "ATC ID", "", SIMCONNECT_DATATYPE.STRING128, 0.0f, SimConnect.SIMCONNECT_UNUSED);

                simConnect.RegisterDataDefineStruct<SimConnectTrafficData>(CustomStructs.TrafficData);



                Debug.WriteLine("Connection initiated to FSX");
                Debug.Flush();
            }
            catch (COMException)
            {
                Disconnect("Could not connect to SimConnect", true);
            }
        }

        public void Disconnect(String message = "Disconnected from SimConnect", bool isUnexpected = false)
        {
            simConnect?.Dispose();
            simConnect = null;
            flightData.ClearData();
            trafficData.Clear();
            OnConnectionStatusChange(this, new SimConnectEventArgs(isUnexpected ? ConnectionEventType.Abnormal_Disconnect : ConnectionEventType.Neutral, message));
            
            if (isUnexpected)
            {
                waitUntil = DateTime.Now.AddMilliseconds(500);
                Connect(handlePtr);
            } else
            {
                simConnectPoller.Stop();
                handlePtr = IntPtr.Zero;
            }
        }



        private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            Debug.WriteLine("Quit");
            Debug.Flush();
        }

        private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            string message = "Flight sim connection Open. App Version" + data.dwApplicationVersionMajor + "." + data.dwApplicationVersionMinor;
            Debug.WriteLine(message);
            Debug.Flush();
            OnConnectionStatusChange(this, new SimConnectEventArgs(ConnectionEventType.Connected, message));
        }

        private void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            Debug.WriteLine("Exception: " + data.dwException);
            Debug.Flush();
        }

        private void SimConnect_OnRecvSimobjectData(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data)
        {

            if (data.dwRequestID == (uint)(Requests.MainRequest))
            {
                SimConnectData typedData = (SimConnectData)data.dwData[0];

                if (!double.IsNaN(typedData.latitude))
                    flightData.Latitude = typedData.latitude;
                if (!double.IsNaN(typedData.longitude))
                    flightData.Longitude = typedData.longitude;
                if (!double.IsNaN(typedData.altitude))
                    flightData.AltitudeFt = typedData.altitude;
                if (!double.IsNaN(typedData.groundSpeed))
                    flightData.GroundSpeedKt = typedData.groundSpeed;
                if (!double.IsNaN(typedData.groundTrack))
                    flightData.GroundTrackDegrees = typedData.groundTrack;
                if (!double.IsNaN(typedData.pitch))
                    flightData.PitchDegrees = -typedData.pitch;
                if (!double.IsNaN(typedData.roll))
                    flightData.RollDegrees = -typedData.roll;
                if (!double.IsNaN(typedData.trueHeading))
                    flightData.TrueHeadingDegrees = typedData.trueHeading;


                Debug.WriteLine("SimConnect - Alt:{0:F2}, Lat:{1:F4}, Lon:{2:F4}, GS:{3:F1}, GT:{4:F1}, Pitch:{5:F2}, Roll:{6:F2}, TH:{7:F1}",
                    typedData.altitude, typedData.latitude, typedData.longitude, typedData.groundSpeed,
                    typedData.groundTrack, typedData.pitch, typedData.roll, typedData.trueHeading);
                //Debug.WriteLine("Flight Data - Alt:{0:F2}, Lat:{1:F4}, Lon:{2:F4}, GS:{3:F1}, GT:{4:F1}, Pitch:{5:F2}, Roll:{6:F2}, TH:{7:F1}",
                //    flightData.AltitudeFt, flightData.Latitude, flightData.Longitude, flightData.GroundSpeedKt,
                //    flightData.GroundTrackDegrees, flightData.PitchDegrees, flightData.RollDegrees, flightData.TrueHeadingDegrees);
            }
            else if (data.dwRequestID == (uint)(Requests.TrafficRequest) && data.dwObjectID != 1) // objectId 1 is me.
            {
              
                SimConnectTrafficData typedData = (SimConnectTrafficData)data.dwData[0];
                TrafficData td = new TrafficData();
                td.ICAOAddress = data.dwObjectID;
                if (!double.IsNaN(typedData.latitude))
                    td.Latitude = typedData.latitude;
                if (!double.IsNaN(typedData.longitude))
                    td.Longitude = typedData.longitude;
                if (!double.IsNaN(typedData.altitude))
                    td.Altitude = typedData.altitude;
                if (!double.IsNaN(typedData.verticalSpeed))
                    td.VerticalSpeed = typedData.verticalSpeed;
                if (typedData.isAirborne == 0) td.IsAirborne = false;
                else if (typedData.isAirborne == 1) td.IsAirborne = true;
                if (!double.IsNaN(typedData.groundTrack))
                    td.GroundTrackDegrees = typedData.groundTrack;
                if (!double.IsNaN(typedData.velocity))
                    td.Velocity = typedData.velocity;
                td.Callsign = typedData.callsign;
                td.LastSeen = DateTime.Now;

                //Debug.WriteLine("Traffic: ID:{0}, Callsign:{1}", td.ICAOAddress, td.Callsign);

                this.trafficData[td.ICAOAddress.Value] = td;
            }

            Debug.Flush();
        }

    }
}
