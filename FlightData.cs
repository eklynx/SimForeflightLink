using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimForeflightLink
{
    public class FlightData
    {
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public float? Altitude { get; set; }
        public float? GroundTrack { get; set; }
        public float? GroundSpeed { get; set; }
        public float? TrueHeading { get; set; }
        public float? Pitch { get; set; }
        public float? Roll { get; set; }

        public void ClearData()
        {
            Latitude = Longitude = Altitude = GroundTrack = GroundSpeed = TrueHeading = Pitch = Roll = null;
        }
    }
}
