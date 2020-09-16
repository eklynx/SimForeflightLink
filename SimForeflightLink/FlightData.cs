using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SimForeflightLink.FlightData.FlightDataUpdatedEventArgs;

namespace SimForeflightLink
{
    public class FlightData
    {

        private static readonly double FEET_PER_METER = 3.2808399f;
        private static readonly double KNOTS_PER_METERS_PER_SEC = 1.94384449f;

        public event EventHandler<FlightDataUpdatedEventArgs> OnFlightDataUpdate;

        public class FlightDataUpdatedEventArgs : EventArgs
        {
            public enum FlightDataField
            {
                Latitude = 0x1,
                Longitude = 0x10,
                Altitude = 0x100,
                GroundTrack = 0x1000,
                GroundSpeed = 0x10000,
                TrueHeading = 0x100000,
                Pitch = 0x1000000,
                Roll = 0x10000000,
                All = 0x11111111
            }

            public FlightDataField Field { get; }
            internal FlightDataUpdatedEventArgs(FlightDataField field) { Field = field; }
        }


        private double? latitude;
        private double? longitude;
        private double? altitudeFt;
        private double? groundTrackDeg;
        private double? groundSpeedKt;
        private double? trueHeading;
        private double? pitchDeg;
        private double? rollDeg;

        public double? Latitude 
        { 
            get { return latitude; }
            set
            {
                if (latitude != value )
                {
                    if (value < -180.0 || value > 180.0)
                        throw new ArgumentOutOfRangeException("Latitude must be between -180 and 180 degrees");
                    latitude = value;
                    OnFlightDataUpdate?.Invoke(this, new FlightDataUpdatedEventArgs(FlightDataField.Latitude));
                }
            }
        }
        public double? Longitude
        {
            get { return longitude; }
            set
            {
                if (longitude != value)
                {
                    if (value < -180.0 || value > 180.0)
                        throw new ArgumentOutOfRangeException("Longitude must be between -180 and 180 degrees");
                    longitude = value;
                    OnFlightDataUpdate?.Invoke(this, new FlightDataUpdatedEventArgs(FlightDataField.Longitude));
                }
            }
        }
        public double? AltitudeFt
        {
            get { return altitudeFt; }
            set
            {
                if (altitudeFt != value)
                {
                    altitudeFt = value;
                    OnFlightDataUpdate?.Invoke(this, new FlightDataUpdatedEventArgs(FlightDataField.Altitude));
                }
            }
        }
        public double? AltitudeMeters {
            get { return AltitudeFt / FEET_PER_METER; }
            set { AltitudeFt = value * FEET_PER_METER; }
        }
        public double? GroundTrackDegrees
        {
            get { return groundTrackDeg; }
            set
            {
                if (groundTrackDeg != value)
                {
                    if (value < 0 || value > 360.0)
                        throw new ArgumentOutOfRangeException("Ground Track must be between 0 and 360 degrees");
                    groundTrackDeg = value;
                    OnFlightDataUpdate?.Invoke(this, new FlightDataUpdatedEventArgs(FlightDataField.GroundTrack));
                }
            }
        }
        public double? GroundTrackRadians
        {
            get { return DegreesToRadians(GroundTrackDegrees); }
            set {
                if (value < 0 || value > (2.0 * Math.PI))
                    throw new ArgumentOutOfRangeException("Ground Track must be between 0 and 2pi radians");
                GroundTrackDegrees = RadiansToDegrees(SanitizeValue(value)); 
            }
        }
        public double? GroundSpeedKt
        {
            get { return groundSpeedKt; }
            set
            {
                if (groundSpeedKt != value)
                {
                    groundSpeedKt = value;
                    if (null != OnFlightDataUpdate)
                        OnFlightDataUpdate?.Invoke(this, new FlightDataUpdatedEventArgs(FlightDataField.GroundSpeed));
                }
            }
        }
        public double? GroundSpeedMPS
        {
            get { return GroundSpeedKt / KNOTS_PER_METERS_PER_SEC; }
            set { GroundSpeedKt = SanitizeValue(value * KNOTS_PER_METERS_PER_SEC); }
        }
        public double? TrueHeadingDegrees
        {
            get { return trueHeading; }
            set
            {
                if (trueHeading != value)
                {
                    if (value < 0 || value > 360.0)
                        throw new ArgumentOutOfRangeException("True Heading must be between 0 and 360 degrees");
                    trueHeading = value;
                    if (null != OnFlightDataUpdate)
                        OnFlightDataUpdate?.Invoke(this, new FlightDataUpdatedEventArgs(FlightDataField.TrueHeading));
                }
            }
        }
        public double? TrueHeadingRadians
        {
            get { return DegreesToRadians(TrueHeadingDegrees); }
            set
            {
                if (value < 0 || value > (2.0 * Math.PI))
                    throw new ArgumentOutOfRangeException("True Heading must be between 0 and 2pi radians");
                TrueHeadingDegrees = RadiansToDegrees(SanitizeValue(value)); 
            }
        }
        
        /// <summary>
        /// Pitch Degrees (+ = nose up, - = nose down)
        /// </summary>
        public double? PitchDegrees
        {
            get { return pitchDeg; }
            set
            {
                if (pitchDeg != value)
                {
                    if (value < -90.0 || value > 90.0)
                        throw new ArgumentOutOfRangeException("Pitch must be between -90 and 90 degrees");
                    pitchDeg = value;
                    OnFlightDataUpdate?.Invoke(this, new FlightDataUpdatedEventArgs(FlightDataField.Pitch));
                }
            }
        }
        public double? PitchRadians
        {
            get { return DegreesToRadians(PitchDegrees); }
            set
            {
                if (value < (-2.0 * Math.PI) || value > (2.0 * Math.PI))
                    throw new ArgumentOutOfRangeException("True Heading must be between -2pi and 2pi radians");
                PitchDegrees = RadiansToDegrees(SanitizeValue(value)); }
        }
        
        /// <summary>
        /// Roll Degrees (+ = right roll, - = left roll)
        /// </summary>
        public double? RollDegrees
        {
            get { return rollDeg; }
            set
            {
                if (rollDeg != value)
                {
                    if (value < -180.0 || value > 180.0)
                        throw new ArgumentOutOfRangeException("Roll must be between -180 and 180 degrees");
                    rollDeg = value;
                    OnFlightDataUpdate?.Invoke(this, new FlightDataUpdatedEventArgs(FlightDataField.Roll));
                }
            }
        }
        public double? RollRadians
        {
            get { return DegreesToRadians(RollDegrees); }
            set
            {
                if (value < (-2.0 * Math.PI) || value > (2.0 * Math.PI))
                    throw new ArgumentOutOfRangeException("True Heading must be between -2pi and 2pi radians");
                RollDegrees = RadiansToDegrees(SanitizeValue(value)); 
            }
        }

        public void ClearData()
        {
            Latitude = Longitude = GroundTrackDegrees = GroundSpeedKt = TrueHeadingDegrees = PitchDegrees = RollDegrees = AltitudeFt = null;
            OnFlightDataUpdate?.Invoke(this, new FlightDataUpdatedEventArgs(FlightDataField.All));
        }


        private static double? DegreesToRadians(double? degrees)
        {
            if (!degrees.HasValue)
                return null;
            return (double)(degrees * (Math.PI / 180.0));
        }

        private static double? RadiansToDegrees(double? radians)
        {
            if (!radians.HasValue)
                return null;
            return (double)(radians * (180.0 / Math.PI));
        }

        private double? SanitizeValue(double? value)
        {
            if (!value.HasValue || double.IsNaN(value.Value) || double.IsInfinity(value.Value))
                return null;
            return value;
        }
    }

}
