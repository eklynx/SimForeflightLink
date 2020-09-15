using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using SimForeflightLink;
using System.Net.Sockets;
using Moq;
using Moq.Protected;
using System.Collections.Generic;

namespace SimForeflightLinkTests
{
    [TestClass]
    public class ForeFlightSendingTests
    {
        public static FlightData DEFUALT_DATA = new FlightData
        {
            AltitudeMeters = 1.1,
            GroundSpeedMPS = 2.2,
            GroundTrackDegress = 3.3,
            Latitude = 4.4,
            Longitude = 5.5,
            PitchDegrees = 6.6,
            RollDegrees = 7.7,
            TrueHeadingDegrees = 8.8
        };


        [TestMethod]
        public void TestForeflightSending()
        {
            FlightData data = DEFUALT_DATA;
            string GPS_STRING = "XGPSMSFS 2020,5.5000,4.4000,1.1,3.30,2.2";
            string ATTITUDE_STRING = "XATTMSFS 2020,8.8,6.6,7.7";

            var mock = new Mock<ForeFlightSender>(MockBehavior.Default, data, new UdpClient())
            {
                CallBase = true
            };

            ForeFlightSender sender = mock.Object;

            mock.Protected().SetupGet<FlightData>("FlightData").Returns(data);
            mock.Protected().Setup("Send", ItExpr.IsAny<string>())
                .Callback((string msg) => { });

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), ForeFlightSender.DEFAULT_PORT);
            sender.EndPoint = endpoint;

            sender.Start();
            System.Threading.Thread.Sleep(5100);
            sender.Stop();
            System.Threading.Thread.Sleep(1100);

            mock.Protected().Verify("Send", Times.Exactly(5), ItExpr.Is<string>(s => GPS_STRING.Equals(s)));
            // giving some leway on Attitude sending just in case of lag.
            mock.Protected().Verify("Send", Times.AtLeast(31), ItExpr.Is<string>(s => ATTITUDE_STRING.Equals(s)));
            mock.Protected().Verify("Send", Times.AtMost(32), ItExpr.Is<string>(s => ATTITUDE_STRING.Equals(s)));

        }


        public static IEnumerable<object[]> GetNullEntries()
        {
            yield return new object[] { new FlightData() };
            yield return new object[] { new FlightData()
            {
                AltitudeMeters = null,
                GroundSpeedMPS = 2.2,
                GroundTrackDegress = 3.3,
                Latitude = 4.4,
                Longitude = 5.5,
                PitchDegrees = 6.6,
                RollDegrees = 7.7,
                TrueHeadingDegrees = 8.8
            } };

            yield return new object[] { new FlightData()
            {
                AltitudeMeters = 1.1,
                GroundSpeedMPS = null,
                GroundTrackDegress = 3.3,
                Latitude = 4.4,
                Longitude = 5.5,
                PitchDegrees = 6.6,
                RollDegrees = 7.7,
                TrueHeadingDegrees = 8.8
            } };

            yield return new object[] { new FlightData()
            {
                AltitudeMeters = 1.1,
                GroundSpeedMPS = 2.2,
                GroundTrackDegress = null,
                Latitude = 4.4,
                Longitude = 5.5,
                PitchDegrees = 6.6,
                RollDegrees = 7.7,
                TrueHeadingDegrees = 8.8
            } };

            yield return new object[] { new FlightData()
            {
                AltitudeMeters = 1.1,
                GroundSpeedMPS = 2.2,
                GroundTrackDegress = 3.3,
                Latitude = null,
                Longitude = 5.5,
                PitchDegrees = 6.6,
                RollDegrees = 7.7,
                TrueHeadingDegrees = 8.8
            } };

            yield return new object[] { new FlightData()
            {
                AltitudeMeters = 1.1,
                GroundSpeedMPS = 2.2,
                GroundTrackDegress = 3.3,
                Latitude = 4.4,
                Longitude = null,
                PitchDegrees = 6.6,
                RollDegrees = 7.7,
                TrueHeadingDegrees = 8.8
            } };

            yield return new object[] { new FlightData()
            {
                AltitudeMeters = 1.1,
                GroundSpeedMPS = 2.2,
                GroundTrackDegress = 3.3,
                Latitude = 4.4,
                Longitude = 5.5,
                PitchDegrees = null,
                RollDegrees = 7.7,
                TrueHeadingDegrees = 8.8
            } };

            yield return new object[] { new FlightData()
            {
                AltitudeMeters = 1.1,
                GroundSpeedMPS = 2.2,
                GroundTrackDegress = 3.3,
                Latitude = 4.4,
                Longitude = 5.5,
                PitchDegrees = 6.6,
                RollDegrees = null,
                TrueHeadingDegrees = 8.8
            } };

            yield return new object[] { new FlightData()
            {
                AltitudeMeters = 1.1,
                GroundSpeedMPS = 2.2,
                GroundTrackDegress = 3.3,
                Latitude = 4.4,
                Longitude = 5.5,
                PitchDegrees = 6.6,
                RollDegrees = 7.7,
                TrueHeadingDegrees = null
            } };

        }

        [DataTestMethod]
        [DynamicData(nameof(GetNullEntries), DynamicDataSourceType.Method)]
        public void TestForeflightSendingSkipInvalid(FlightData data)
        {
            string GPS_STRING = "XGPSMSFS 2020,5.5000,4.4000,1.1,3.30,2.2";
            string ATTITUDE_STRING = "XATTMSFS 2020,8.8,6.6,7.7";

            var mock = new Mock<ForeFlightSender>(MockBehavior.Default, data, new UdpClient())
            {
                CallBase = true
            };

            ForeFlightSender sender = mock.Object;

            mock.Protected().SetupGet<FlightData>("FlightData").Returns(data);
            mock.Protected().Setup("Send", ItExpr.IsAny<string>())
                .Callback((string msg) => { });

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), ForeFlightSender.DEFAULT_PORT);
            sender.EndPoint = endpoint;

            sender.Start();
            System.Threading.Thread.Sleep(1100);
            sender.Stop();

            mock.Protected().Verify("Send", Times.Never(), ItExpr.Is<string>(s => GPS_STRING.Equals(s)));
            mock.Protected().Verify("Send", Times.Never(), ItExpr.Is<string>(s => ATTITUDE_STRING.Equals(s)));

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestForeflightNoEndpoint()
        {
            FlightData fd = null;
            ForeFlightSender sender = new ForeFlightSender(ref fd, null);
            sender.Start();
        }
        
    }
}
