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
            GroundTrackDegrees = 3.3,
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
            Assert.IsNull(fd.GroundTrackDegrees);
            Assert.IsNull(fd.GroundTrackRadians);
            Assert.IsNull(fd.Latitude);
            Assert.IsNull(fd.Longitude);
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

        #region Altitude

        [TestMethod()]
        public void TestSetAltitudeFt()
        {
            FlightData data = new FlightData();
            data.AltitudeFt = 1;
            Assert.AreEqual(1, data.AltitudeFt);
            Assert.AreEqual(0.3048, Math.Round( data.AltitudeMeters.Value, 4));
        }

        [TestMethod()]
        public void TestSetAltitudeFtWithEvent()
        {
            FlightData data = new FlightData();
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.AltitudeFt = 1;
            Assert.AreEqual(1, data.AltitudeFt);
            Assert.AreEqual(0.3048, Math.Round(data.AltitudeMeters.Value, 4));
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
            Assert.AreEqual(3.2808, Math.Round(data.AltitudeFt.Value, 4));
        }

        [TestMethod()]
        public void TestSetAltitudeMetersWithEvent()
        {
            FlightData data = new FlightData();
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.AltitudeMeters = 1;
            Assert.AreEqual(1, data.AltitudeMeters);
            Assert.AreEqual(3.2808, Math.Round(data.AltitudeFt.Value, 4));
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

        #endregion Altitude

        #region Latitdue/Longitude

        [TestMethod()] // TODO: Data source to test limit values
        public void TestSetLatitude()
        {
            FlightData data = new FlightData();
            data.Latitude = 45;
            Assert.AreEqual(45, data.Latitude);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetLatitudeTooHigh()
        {
            FlightData data = new FlightData();
            data.Latitude = 180.01;
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetLatitudeTooLow()
        {
            FlightData data = new FlightData();
            data.Latitude = -180.01;
        }

        [TestMethod()]
        public void TestSetLatitudeWithEvent()
        {
            FlightData data = new FlightData();
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.Latitude = 1;
            Assert.AreEqual(1, data.Latitude);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.Latitude, argsToVerify.Field);
        }

        [TestMethod()]
        public void TestSetLatitudeNullWithEvent()
        {
            FlightData data = new FlightData()
            {
                Latitude = 1
            };
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.Latitude = null;
            Assert.IsNull(data.Latitude);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.Latitude, argsToVerify.Field);
        }

        [TestMethod()] // TODO: Data source to test limit values
        public void TestSetLongitude()
        {
            FlightData data = new FlightData();
            data.Longitude = 45;
            Assert.AreEqual(45, data.Longitude);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetLongitudeTooHigh()
        {
            FlightData data = new FlightData();
            data.Longitude = 180.01;
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetLongitudeTooLow()
        {
            FlightData data = new FlightData();
            data.Longitude = -180.01;
        }

        [TestMethod()]
        public void TestSetLongitudeWithEvent()
        {
            FlightData data = new FlightData();
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.Longitude = 1;
            Assert.AreEqual(1, data.Longitude);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.Longitude, argsToVerify.Field);
        }

        [TestMethod()]
        public void TestSetLongitudeNullWithEvent()
        {
            FlightData data = new FlightData()
            {
                Longitude = 1
            };
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.Longitude = null;
            Assert.IsNull(data.Longitude);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.Longitude, argsToVerify.Field);
        }

        #endregion Latitdue/Longitude

        #region Ground Track

        [TestMethod()] // TODO: Data source to test limit values
        public void TestSetGroundTrackDegrees()
        {
            FlightData data = new FlightData();
            data.GroundTrackDegrees = 45;
            Assert.AreEqual(45, data.GroundTrackDegrees);
            Assert.AreEqual(Math.Round(45.0 * Math.PI / 180.0, 4), Math.Round(data.GroundTrackRadians.Value, 4));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetGroundTrackDegreesTooHigh()
        {
            FlightData data = new FlightData();
            data.GroundTrackDegrees = 360.01;
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetGroundTrackDegreesTooLow()
        {
            FlightData data = new FlightData();
            data.GroundTrackDegrees = -0.01;
        }

        [TestMethod()]
        public void TestSetGroundTrackDegreesEvent()
        {
            FlightData data = new FlightData();
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.GroundTrackDegrees = Math.PI / 2.0;
            Assert.AreEqual(Math.PI / 2.0, data.GroundTrackDegrees);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.GroundTrack, argsToVerify.Field);
        }

        [TestMethod()]
        public void TestSetGroundTrackDegreesNullWithEvent()
        {
            FlightData data = new FlightData()
            {
                GroundTrackDegrees = 1
            };
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.GroundTrackDegrees = null;
            Assert.IsNull(data.GroundTrackDegrees);
            Assert.IsNull(data.GroundTrackRadians);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.GroundTrack, argsToVerify.Field);
        }

        [TestMethod()] // TODO: Data source to test limit values
        public void TestSetGroundTrackRadians()
        {
            FlightData data = new FlightData();
            data.GroundTrackRadians = Math.PI / 2.0;
            Assert.AreEqual(Math.Round(Math.PI / 2, 4), Math.Round(data.GroundTrackRadians.Value, 4));
            Assert.AreEqual(Math.Round(90.0, 4), Math.Round(data.GroundTrackDegrees.Value, 4));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetGroundTrackRadiansTooHigh()
        {
            FlightData data = new FlightData();
            data.GroundTrackRadians = 0.01f + 2* Math.PI;
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetGroundTrackRadiansTooLow()
        {
            FlightData data = new FlightData();
            data.GroundTrackRadians = -0.01;
        }

        [TestMethod()]
        public void TestSetGroundTrackRadiansEvent()
        {
            FlightData data = new FlightData();
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.GroundTrackRadians = Math.PI / 2.0;
            Assert.AreEqual(Math.Round(Math.PI / 2, 4), Math.Round(data.GroundTrackRadians.Value, 4));
            Assert.AreEqual(Math.Round(90.0, 4), Math.Round(data.GroundTrackDegrees.Value, 4));
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.GroundTrack, argsToVerify.Field);
        }

        [TestMethod()]
        public void TestSetGroundTrackRadiansNullWithEvent()
        {
            FlightData data = new FlightData()
            {
                GroundTrackRadians = 1
            };
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.GroundTrackRadians = null;
            Assert.IsNull(data.GroundTrackRadians);
            Assert.IsNull(data.GroundTrackDegrees);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.GroundTrack, argsToVerify.Field);
        }

        #endregion Ground Track

        #region True Heading

        [TestMethod()] // TODO: Data source to test limit values
        public void TestSetTrueHeadingDegrees()
        {
            FlightData data = new FlightData();
            data.TrueHeadingDegrees = 45;
            Assert.AreEqual(45, data.TrueHeadingDegrees);
            Assert.AreEqual(Math.Round(45.0 * Math.PI / 180.0, 4), Math.Round(data.TrueHeadingRadians.Value, 4));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetTrueHeadingDegreesTooHigh()
        {
            FlightData data = new FlightData();
            data.TrueHeadingDegrees = 360.01;
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetTrueHeadingDegreesTooLow()
        {
            FlightData data = new FlightData();
            data.TrueHeadingDegrees = -0.01;
        }

        [TestMethod()]
        public void TestSetTrueHeadingDegreesEvent()
        {
            FlightData data = new FlightData();
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.TrueHeadingDegrees = Math.PI / 2.0;
            Assert.AreEqual(Math.PI / 2.0, data.TrueHeadingDegrees);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.TrueHeading, argsToVerify.Field);
        }

        [TestMethod()]
        public void TestSetTrueHeadingDegreesNullWithEvent()
        {
            FlightData data = new FlightData()
            {
                TrueHeadingDegrees = 1
            };
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.TrueHeadingDegrees = null;
            Assert.IsNull(data.TrueHeadingDegrees);
            Assert.IsNull(data.TrueHeadingRadians);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.TrueHeading, argsToVerify.Field);
        }

        [TestMethod()] // TODO: Data source to test limit values
        public void TestSetTrueHeadingRadians()
        {
            FlightData data = new FlightData();
            data.TrueHeadingRadians = Math.PI / 2.0;
            Assert.AreEqual(Math.Round(Math.PI / 2, 4), Math.Round(data.TrueHeadingRadians.Value, 4));
            Assert.AreEqual(Math.Round(90.0, 4), Math.Round(data.TrueHeadingDegrees.Value, 4));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetTrueHeadingRadiansTooHigh()
        {
            FlightData data = new FlightData();
            data.TrueHeadingRadians = 0.01f + 2 * Math.PI;
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetTrueHeadingRadiansTooLow()
        {
            FlightData data = new FlightData();
            data.TrueHeadingRadians = -0.01;
        }

        [TestMethod()]
        public void TestSetTrueHeadingRadiansEvent()
        {
            FlightData data = new FlightData();
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.TrueHeadingRadians = Math.PI / 2.0;
            Assert.AreEqual(Math.Round(Math.PI / 2, 4), Math.Round(data.TrueHeadingRadians.Value, 4));
            Assert.AreEqual(Math.Round(90.0, 4), Math.Round(data.TrueHeadingDegrees.Value, 4));
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.TrueHeading, argsToVerify.Field);
        }

        [TestMethod()]
        public void TestSetTrueHeadingRadiansNullWithEvent()
        {
            FlightData data = new FlightData()
            {
                TrueHeadingRadians = 1
            };
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.TrueHeadingRadians = null;
            Assert.IsNull(data.TrueHeadingRadians);
            Assert.IsNull(data.TrueHeadingDegrees);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.TrueHeading, argsToVerify.Field);
        }

        #endregion True Heading

        #region Pitch

        [TestMethod()] // TODO: Data source to test limit values
        public void TestSetPitchDegrees()
        {
            FlightData data = new FlightData();
            data.PitchDegrees = 45;
            Assert.AreEqual(45, data.PitchDegrees);
            Assert.AreEqual(Math.Round(45.0 * Math.PI / 180.0, 4), Math.Round(data.PitchRadians.Value, 4));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetPitchDegreesTooHigh()
        {
            FlightData data = new FlightData();
            data.PitchDegrees = 360.01;
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetPitchDegreesTooLow()
        {
            FlightData data = new FlightData();
            data.PitchDegrees = -90.01;
        }

        [TestMethod()]
        public void TestSetPitchDegreesEvent()
        {
            FlightData data = new FlightData();
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.PitchDegrees = Math.PI / 2.0;
            Assert.AreEqual(Math.PI / 2.0, data.PitchDegrees);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.Pitch, argsToVerify.Field);
        }

        [TestMethod()]
        public void TestSetPitchDegreesNullWithEvent()
        {
            FlightData data = new FlightData()
            {
                PitchDegrees = 1
            };
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.PitchDegrees = null;
            Assert.IsNull(data.PitchDegrees);
            Assert.IsNull(data.PitchRadians);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.Pitch, argsToVerify.Field);
        }

        [TestMethod()] // TODO: Data source to test limit values
        public void TestSetPitchRadians()
        {
            FlightData data = new FlightData();
            data.PitchRadians = Math.PI / 2.0;
            Assert.AreEqual(Math.Round(Math.PI / 2, 4), Math.Round(data.PitchRadians.Value, 4));
            Assert.AreEqual(Math.Round(90.0, 4), Math.Round(data.PitchDegrees.Value, 4));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetPitchRadiansTooHigh()
        {
            FlightData data = new FlightData();
            data.PitchRadians = 0.01f + 2 * Math.PI;
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetPitchRadiansTooLow()
        {
            FlightData data = new FlightData();
            data.PitchRadians = -(0.01f + 2 * Math.PI);
        }

        [TestMethod()]
        public void TestSetPitchRadiansEvent()
        {
            FlightData data = new FlightData();
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.PitchRadians = Math.PI / 2.0;
            Assert.AreEqual(Math.Round(Math.PI / 2, 4), Math.Round(data.PitchRadians.Value, 4));
            Assert.AreEqual(Math.Round(90.0, 4), Math.Round(data.PitchDegrees.Value, 4));
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.Pitch, argsToVerify.Field);
        }

        [TestMethod()]
        public void TestSetPitchRadiansNullWithEvent()
        {
            FlightData data = new FlightData()
            {
                PitchRadians = 1
            };
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.PitchRadians = null;
            Assert.IsNull(data.PitchRadians);
            Assert.IsNull(data.PitchDegrees);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.Pitch, argsToVerify.Field);
        }

        #endregion Pitch

        #region Roll

        [TestMethod()] // TODO: Data source to test limit values
        public void TestSetRollDegrees()
        {
            FlightData data = new FlightData();
            data.RollDegrees = 45;
            Assert.AreEqual(45, data.RollDegrees);
            Assert.AreEqual(Math.Round(45.0 * Math.PI / 180.0, 4), Math.Round(data.RollRadians.Value, 4));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetRollDegreesTooHigh()
        {
            FlightData data = new FlightData();
            data.RollDegrees = 180.01;
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetRollDegreesTooLow()
        {
            FlightData data = new FlightData();
            data.RollDegrees = -180.01;
        }

        [TestMethod()]
        public void TestSetRollDegreesEvent()
        {
            FlightData data = new FlightData();
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.RollDegrees = Math.PI / 2.0;
            Assert.AreEqual(Math.PI / 2.0, data.RollDegrees);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.Roll, argsToVerify.Field);
        }

        [TestMethod()]
        public void TestSetRollDegreesNullWithEvent()
        {
            FlightData data = new FlightData()
            {
                RollDegrees = 1
            };
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.RollDegrees = null;
            Assert.IsNull(data.RollDegrees);
            Assert.IsNull(data.RollRadians);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.Roll, argsToVerify.Field);
        }

        [TestMethod()] // TODO: Data source to test limit values
        public void TestSetRollRadians()
        {
            FlightData data = new FlightData();
            data.RollRadians = Math.PI / 2.0;
            Assert.AreEqual(Math.Round(Math.PI / 2, 4), Math.Round(data.RollRadians.Value, 4));
            Assert.AreEqual(Math.Round(90.0, 4), Math.Round(data.RollDegrees.Value, 4));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetRollRadiansTooHigh()
        {
            FlightData data = new FlightData();
            data.RollRadians = 0.01f + Math.PI;
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSetRollRadiansTooLow()
        {
            FlightData data = new FlightData();
            data.RollRadians = -(0.01f + Math.PI);
        }

        [TestMethod()]
        public void TestSetRollRadiansEvent()
        {
            FlightData data = new FlightData();
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.RollRadians = Math.PI / 2.0;
            Assert.AreEqual(Math.Round(Math.PI / 2, 4), Math.Round(data.RollRadians.Value, 4));
            Assert.AreEqual(Math.Round(90.0, 4), Math.Round(data.RollDegrees.Value, 4));
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.Roll, argsToVerify.Field);
        }

        [TestMethod()]
        public void TestSetRollRadiansNullWithEvent()
        {
            FlightData data = new FlightData()
            {
                RollRadians = 1
            };
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.RollRadians = null;
            Assert.IsNull(data.RollRadians);
            Assert.IsNull(data.RollDegrees);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.Roll, argsToVerify.Field);
        }

        #endregion Roll

        #region Ground Speed

        public void TestSetGroundSpeedFt()
        {
            FlightData data = new FlightData();
            data.GroundSpeedKt = 1;
            Assert.AreEqual(1, data.GroundSpeedKt);
            Assert.AreEqual(0.5144, Math.Round(data.GroundSpeedMPS.Value, 4));
        }

        [TestMethod()]
        public void TestSetGroundSpeedFtWithEvent()
        {
            FlightData data = new FlightData();
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.GroundSpeedKt = 1;
            Assert.AreEqual(1, data.GroundSpeedKt);
            Assert.AreEqual(0.5144, Math.Round(data.GroundSpeedMPS.Value, 4));
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.GroundSpeed, argsToVerify.Field);
        }

        [TestMethod()]
        public void TestSetGroundSpeedFtNullWithEvent()
        {
            FlightData data = new FlightData()
            {
                GroundSpeedKt = 1
            };
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.GroundSpeedKt = null;
            Assert.IsNull(data.GroundSpeedKt);
            Assert.IsNull(data.GroundSpeedMPS);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.GroundSpeed, argsToVerify.Field);
        }

        [TestMethod()]
        public void TestSetGroundSpeedMetersPerSec()
        {
            FlightData data = new FlightData();
            data.GroundSpeedMPS = 1;
            Assert.AreEqual(1, data.GroundSpeedMPS);
            Assert.AreEqual(1.9438, Math.Round(data.GroundSpeedKt.Value, 4));
        }

        [TestMethod()]
        public void TestSetGroundSpeedMetersWithEvent()
        {
            FlightData data = new FlightData();
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.GroundSpeedMPS = 1;
            Assert.AreEqual(1, data.GroundSpeedMPS);
            Assert.AreEqual(1.9438, Math.Round(data.GroundSpeedKt.Value, 4));
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.GroundSpeed, argsToVerify.Field);
        }

        [TestMethod()]
        public void TestSetGroundSpeedMetersNullWithEvent()
        {
            FlightData data = new FlightData()
            {
                GroundSpeedMPS = 1
            };
            FlightDataUpdatedEventArgs argsToVerify = null;
            data.OnFlightDataUpdate += (object sender, FlightDataUpdatedEventArgs args) => { argsToVerify = args; };
            data.GroundSpeedMPS = null;
            Assert.IsNull(data.GroundSpeedMPS);
            Assert.IsNull(data.GroundSpeedKt);
            Assert.AreEqual(FlightDataUpdatedEventArgs.FlightDataField.GroundSpeed, argsToVerify.Field);
        }

        #endregion Ground Speed

    }
}