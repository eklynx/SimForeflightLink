using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimForeflightLink;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SimForeflightLink.FlightData;

namespace SimForeflightLink.Tests
{
    [TestClass()]
    public class FlightDataTests
    {
        public readonly static FlightData DEFAULT_DATA = new FlightData
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

        [TestMethod()]
        public void ClearDataTest()
        {
            FlightData fd = DEFAULT_DATA;
            fd.ClearData();
            Assert.IsNull(fd.AltitudeFt);
            Assert.IsNull(fd.AltitudeMeters);
            Assert.IsNull(fd.GroundSpeedKt);
            Assert.IsNull(fd.GroundSpeedMPS);
            Assert.IsNull(fd.GroundTrackDegress);
            Assert.IsNull(fd.GroundTrackRadians);
            Assert.IsNull(fd.Latitude);
            Assert.IsNull(fd.LatitudeRadians);
            Assert.IsNull(fd.Longitude);
            Assert.IsNull(fd.LongitudeRadians);
            Assert.IsNull(fd.PitchDegrees);
            Assert.IsNull(fd.PitchRadians);
            Assert.IsNull(fd.RollDegrees);
            Assert.IsNull(fd.RollRadians);
        }

        [TestMethod()]
        public void ClearDataEventTest()
        {
            FlightData data = new FlightData()
            {
                AltitudeFt = 1
            };
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            
            data.ClearData();

            Assert.IsNotNull(argsToVerify);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.All, argsToVerify.Field);
            Assert.IsNull(data.AltitudeFt);
        }

        [TestMethod()]
        public void TestSetAltitudeFt()
        {
            FlightData data = new FlightData();
            data.AltitudeFt = 1;
            Assert.AreEqual(1, data.AltitudeFt);
            Assert.AreEqual("0.3048", String.Format("{0:F4}", data.AltitudeMeters));
        }

        [TestMethod()]
        public void TestSetAltitudeFtWithEvent()
        {
            FlightData data = new FlightData();
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.AltitudeFt = 1;
            Assert.AreEqual(1, data.AltitudeFt);
            Assert.AreEqual("0.3048", String.Format("{0:F4}", data.AltitudeMeters));
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.Altitude, argsToVerify.Field);
        }
        
        [TestMethod()]
        public void TestSetAltitudeFtNullWithEvent()
        {
            FlightData data = new FlightData()
            {
                AltitudeFt = 1
            };
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.AltitudeFt = null;
            Assert.IsNull(data.AltitudeFt);
            Assert.IsNull(data.AltitudeMeters);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.Altitude, argsToVerify.Field);
        }

        [TestMethod()]
        public void TestSetAltitudeMeters()
        {
            FlightData data = new FlightData();
            data.AltitudeMeters = 1;
            Assert.AreEqual(1, data.AltitudeMeters);
            Assert.AreEqual("3.2808", String.Format("{0:F4}", data.AltitudeFt));
        }

        [TestMethod()]
        public void TestSetAltitudeMetersWithEvent()
        {
            FlightData data = new FlightData();
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.AltitudeMeters = 1;
            Assert.AreEqual(1, data.AltitudeMeters);
            Assert.AreEqual("3.2808", String.Format("{0:F4}", data.AltitudeFt));
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.Altitude, argsToVerify.Field);
        }

        [TestMethod()]
        public void TestSetAltitudeMetersNullWithEvent()
        {
            FlightData data = new FlightData()
            {
                AltitudeMeters = 1
            };
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.AltitudeMeters = null;
            Assert.IsNull(data.AltitudeFt);
            Assert.IsNull(data.AltitudeMeters);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.Altitude, argsToVerify.Field);
        }

    }
}