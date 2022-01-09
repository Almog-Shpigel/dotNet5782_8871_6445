using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Location
    {
        public double Latitude;
        public double Longitude;
        public Location(double latitude = 0, double longitude = 0) { Latitude = latitude; Longitude = longitude; }
        public override string ToString()
        {
            string sLatit = ConvertCoordinates(Latitude), sLong = ConvertCoordinates(Longitude);    /// Converts the coordinates to be in base 60 (bonus).
            return sLatit + "N, " + sLong + "E";
        }
        public string ConvertCoordinates(double number)
        {
            //string coordinates;
            int sec = (int)Math.Round(number * 3600);
            int deg = sec / 3600;
            sec = Math.Abs(sec % 3600);
            int min = sec / 60;
            sec %= 60;
            return deg + "" + (char)176 + " " + min + " " + sec;
        }
    }
}

