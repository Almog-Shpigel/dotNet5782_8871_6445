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
            public Location(double latitude = 0, double longitude = 0) { Latitude = latitude; Longitude = longitude; }
            public override string ToString()
            {
                string sLong = ConvertCoordinates(Longitude), sLatit = ConvertCoordinates(Latitude);    /// Converts the coordinates to be in base 60 (bonus).
                return $"(" + sLatit + " E, " + sLong + " N)";
            }
            public string ConvertCoordinates(double number)
            {
                int result, remainder;
                string coordinates;
                result = (int)number;
                coordinates = result + "" + (char)176 + " ";                            /// coordinates holds now the degrees.
                result = (int)((number - result) * 10000);
                result = result * 60;
                remainder = result % 10000;
                result /= 10000;
                coordinates += result + "\' ";                          /// coordinates holds now the minutes.
                result = remainder * 60;
                remainder = result % 10000;
                result /= 10000;
                coordinates += result + "." + remainder + "\" ";      /// coordinates holds now the seconds.
                return coordinates;
            }
        }
    }
}
