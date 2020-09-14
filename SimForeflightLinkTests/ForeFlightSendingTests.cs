using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using SimForeflightLink;
using System.Net.Sockets;

namespace SimForeflightLinkTests
{
    [TestClass]
    public class ForeFlightSendingTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            FlightData data = new FlightData
            {
                AltitudeFt = 5,
                GroundSpeedKt = 1,
                GroundTrackDegress = 90,
                Latitude = 1,
                Longitude = 1,
                PitchDegrees = 2.5f,
                RollDegrees = 15,
                TrueHeadingDegrees = 270
            };

            ForeFlightSender sender = new ForeFlightSender(ref data, new UdpClient()) ;

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("192.168.1.255"), ForeFlightSender.DEFAULT_PORT);
            sender.EndPoint = endpoint;

            sender.Start();

            System.Threading.Thread.Sleep(20000);

            sender.Stop();
        }
    }
}
