﻿using Microsoft.FlightSimulator.SimConnect;
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
        private static readonly string APP_NAME = "SimconnectForeflightLink";
        private const uint SIMCONNECT_OPEN_CONFIGINDEX_LOCAL = uint.MaxValue;

        private SimConnect simConnect;
        private readonly FlightData flightData;
        private Timer receiveMessagePoller;
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
            SimConnectData
        }

        private enum Requests
        {
            MainRequest
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

        public SimConnectLink(ref FlightData flightData)
        {
            this.flightData = flightData;
            receiveMessagePoller = new Timer()
            {
                Interval = 10,
                Enabled = false
            };
            receiveMessagePoller.Tick += RecieveSimConnectMessage;

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
                receiveMessagePoller.Stop();
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
                if (null != simConnect)
                    simConnect.ReceiveMessage();
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
            receiveMessagePoller.Start();
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
                    0,
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
            receiveMessagePoller.Stop();
            simConnect?.Dispose();
            simConnect = null;
            flightData.ClearData();
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
                        flightData.GroundTrackDegress = typedData.groundTrack;
                    if (!double.IsNaN(typedData.pitch))
                        flightData.PitchDegrees = -typedData.pitch;
                    if (!double.IsNaN(typedData.roll))
                        flightData.RollDegrees = typedData.roll;
                    if (!double.IsNaN(typedData.trueHeading))
                        flightData.TrueHeadingDegrees = typedData.trueHeading;


                Debug.WriteLine("SimConnect - Alt:{0:F2}, Lat:{1:F4}, Lon:{2:F4}, GS:{3:F1}, GT:{4:F1}, Pitch:{5:F2}, Roll:{6:F2}, TH:{7:F1}",
                    typedData.altitude, typedData.latitude, typedData.longitude, typedData.groundSpeed,
                    typedData.groundTrack, typedData.pitch, typedData.roll, typedData.trueHeading);
                Debug.WriteLine("Flight Data - Alt:{0:F2}, Lat:{1:F4}, Lon:{2:F4}, GS:{3:F1}, GT:{4:F1}, Pitch:{5:F2}, Roll:{6:F2}, TH:{7:F1}",
                    flightData.AltitudeFt, flightData.Latitude, flightData.Longitude, flightData.GroundSpeedKt,
                    flightData.GroundTrackDegress, flightData.PitchDegrees, flightData.RollDegrees, flightData.TrueHeadingDegrees);
            }
            Debug.Flush();
        }

    }
}
