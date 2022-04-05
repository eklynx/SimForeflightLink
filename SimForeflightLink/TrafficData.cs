using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimForeflightLink
{
    /// <summary>
    /// Format for Traffic:
    ///   Note: use 2-3 decimals of precision only
    ///   boolean gets sent as 1/0
    /// </summary>
    public class TrafficData
    {
        public DateTime LastSeen { get; set; }

        /// <summary>
        /// Unique ID for the plane
        /// </summary>
        public uint? ICAOAddress { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        /// <summary>
        /// Geometric altitude (feet)
        /// </summary>
        public double? Altitude { get; set; }

        /// <summary>
        /// Vertical Speed (feet)
        /// </summary>
        public double? VerticalSpeed { get; set; }

        /// <summary>
        /// Is this traffic airborne? sends as 1/0
        /// </summary>
        public bool? IsAirborne { get; set; }
        
        /// <summary>
        /// Ground track course, degrees based on true north
        /// </summary>
        public double? GroundTrackDegrees { get; set; }

        /// <summary>
        /// Velocity (knots)
        /// </summary>
        public double? Velocity { get; set; }

        public string Callsign { get; set; }

//Track - float, positive, degrees true
//Velocity knots - float
//Callsign - string


    }
}
