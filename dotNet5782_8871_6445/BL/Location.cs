using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Location
        {
            public double Latitude;
            public double Longitude;
            public Location (double latitude, double longitude) { Latitude = latitude; Longitude = longitude; }
        }
    }
}
