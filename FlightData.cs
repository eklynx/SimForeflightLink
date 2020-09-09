using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimForeflightLink
{
    public class FlightData
    {

        private static readonly double FEET_PER_METER = 3.2808399f;
        private static readonly double KNOTS_PER_METERS_PER_SEC = 1.94384449f;

        public double? Latitude { get; set; }
        public double? LatitudeRadians
        {
            get { return Latitude.HasValue ? DegreesToRadians(Latitude.Value) : (double?)null; }
            set { Latitude = value.HasValue ? RadiansToDegrees(value.Value) : (double?)null; }
        }
        public double? Longitude { get; set; }
        public double? LongitudeRadians
        {
            get { return Longitude.HasValue ? DegreesToRadians(Longitude.Value) : (double?)null; }
            set { Longitude = value.HasValue ? RadiansToDegrees(value.Value) : (double?)null; }
        }
        public double? AltitudeFt { get; set; }
        public double? AltitudeMeters {
            get { return AltitudeFt / FEET_PER_METER; }
            set { AltitudeFt = value * FEET_PER_METER; }
        }
        public double? GroundTrackDegress { get; set; }
        public double? GroundTrackRadians
        {
            get { return GroundTrackDegress.HasValue ? DegreesToRadians(GroundTrackDegress.Value) : null as double?; }
            set { GroundTrackDegress = value.HasValue ? (double?)RadiansToDegrees(value.Value) : null; }
        }
        public double? GroundSpeedKt { get; set; }
        public double? GroundSpeedMPS
        {
            get { return GroundSpeedKt / KNOTS_PER_METERS_PER_SEC; }
            set { GroundSpeedKt = value * KNOTS_PER_METERS_PER_SEC; }
        }
        public double? TrueHeadingDegrees { get; set; }
        public double? TrueHeadingRadians
        {
            get { return TrueHeadingDegrees.HasValue ? DegreesToRadians(TrueHeadingDegrees.Value) : null as double?; }
            set { TrueHeadingDegrees = value.HasValue ? (double?)RadiansToDegrees(value.Value) : null; }
        }
        public double? PitchDegrees { get; set; }
        public double? PitchRadians
        {
            get { return PitchDegrees.HasValue ? DegreesToRadians(PitchDegrees.Value) : null as double?; }
            set { PitchDegrees = value.HasValue ? (double?)RadiansToDegrees(value.Value) : null; }
        }
        public double? RollDegrees { get; set; }
        public double? RollRadians
        {
            get { return RollDegrees.HasValue ? DegreesToRadians(RollDegrees.Value) : null as double?; }
            set { RollDegrees = value.HasValue ? (double?)RadiansToDegrees(value.Value) : null; }
        }
        public void ClearData()
        {
            Latitude = Longitude = GroundTrackDegress = GroundSpeedKt = TrueHeadingDegrees = PitchDegrees = RollDegrees = AltitudeFt = null;
        }


        private static double DegreesToRadians(double degrees)
        {
            return (double)(degrees * (Math.PI / 180.0));
        }

        private static double RadiansToDegrees(double radians)
        {
            return (double)(radians * (180.0 / Math.PI));
        }

    }

}
