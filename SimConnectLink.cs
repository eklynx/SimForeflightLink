using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Timers;

namespace SimForeflightLink
{
    public class SimConnectLink
    {
        private static readonly string APP_NAME = "SimconnectForeflightLink";
        private const uint SIMCONNECT_OPEN_CONFIGINDEX_LOCAL = uint.MaxValue;
        private const uint SIMCONNECT_OBJECT_ID_USER = 0;

        private SimConnect simConnect;
        private readonly FlightData flightData;
        private Timer timer;

        private enum CustomStructs
        {
            SimConnectData
        }

        private enum Requests
        {
            MainRequest
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct SimConnectData
        {
            public float latitude;
            public float longitude;
            public double altitude;
            //public double pitch;
            //public double roll;
            //public double groundTrack;
            //public double trueHeading;
            //public double groundSpeed;
        };

        public SimConnectLink(ref FlightData flightData)
        {
            this.flightData = flightData;
            timer = new System.Timers.Timer(50)
            {
                AutoReset = true,
                Enabled = false
            };
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (null != simConnect)
                simConnect.ReceiveMessage();
        }

        public void ReadMessages()
        {
            if (null != simConnect)
                simConnect.ReceiveMessage();
        }

        public void Connect(IntPtr handle)
        {
            simConnect = new SimConnect(
                APP_NAME,
                handle,
                0,
                null,
                SIMCONNECT_OPEN_CONFIGINDEX_LOCAL
            );

            simConnect.OnRecvOpen += SimConnect_OnRecvOpen;
            simConnect.OnRecvQuit += SimConnect_OnRecvQuit;
            simConnect.OnRecvException += SimConnect_OnRecvException;
            simConnect.OnRecvSimobjectData += SimConnect_OnRecvSimobjectData;

            simConnect.RegisterDataDefineStruct<SimConnectData>(CustomStructs.SimConnectData);
            simConnect.AddToDataDefinition((Enum)CustomStructs.SimConnectData, "Plane Latitude", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
            simConnect.AddToDataDefinition((Enum)CustomStructs.SimConnectData, "Plane Longitude", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
            simConnect.AddToDataDefinition((Enum)CustomStructs.SimConnectData, "Plane Altitude", "Feet", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
            //simConnect.AddToDataDefinition((Enum)CustomStructs.ForeflightData, "Plane Pitch Degrees", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
            //simConnect.AddToDataDefinition((Enum)CustomStructs.ForeflightData, "Plane Bank Degrees", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
            //simConnect.AddToDataDefinition((Enum)CustomStructs.ForeflightData, "GPS Ground True Track", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
            //simConnect.AddToDataDefinition((Enum)CustomStructs.ForeflightData, "Plane Heading Degrees True", "Degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
            //simConnect.AddToDataDefinition((Enum)CustomStructs.ForeflightData, "Ground Velocity", "Knots", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);

            timer.Start();

            simConnect.RequestDataOnSimObject(
                Requests.MainRequest,
                CustomStructs.SimConnectData,
                SIMCONNECT_OBJECT_ID_USER,
                SIMCONNECT_PERIOD.SIM_FRAME,
                SIMCONNECT_DATA_REQUEST_FLAG.TAGGED,
                0,
                5, // every 5 frames 
                0
            );

            Console.Out.WriteLine("Connection initiated to FSX");
            Console.Out.Flush();
        }

        private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            Console.Out.WriteLine("Quit");
            Console.Out.Flush();
        }

        private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            Console.Out.WriteLine("Open: " + data.dwApplicationVersionMajor + "." + data.dwApplicationVersionMinor);
            Console.Out.Flush();
        }

        private void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            Console.Error.WriteLine("Exception: " + data.dwException);
            Console.Error.Flush();
        }

        private void SimConnect_OnRecvSimobjectData(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA data)
        {

            if (data.dwRequestID == (uint)(Requests.MainRequest))
            {
                SimConnectData typedData = (SimConnectData)data.dwData[0];

                if (!double.IsNaN(typedData.latitude))
                    flightData.LatitudeRadians = typedData.latitude;
                if (!double.IsNaN(typedData.longitude))
                    flightData.LongitudeRadians = typedData.longitude;
                if (!double.IsNaN(typedData.altitude))
                    flightData.AltitudeFt = typedData.altitude;
                //if (!double.IsNaN(typedData.groundSpeed))
                //    flightData.GroundSpeedKt = typedData.groundSpeed;
                //if (!double.IsNaN(typedData.groundTrack))
                //    flightData.GroundTrackRadians = typedData.groundTrack;
                //if (!double.IsNaN(typedData.pitch))
                //    flightData.PitchRadians = typedData.pitch;
                //if (!double.IsNaN(typedData.roll))
                //    flightData.RollRadians = typedData.roll;
                //if (!double.IsNaN(typedData.trueHeading))
                //    flightData.TrueHeadingRadians = typedData.trueHeading;
                Console.Out.WriteLine("Data: " + flightData);
            }
            Console.Out.Flush();
        }

    }
}
