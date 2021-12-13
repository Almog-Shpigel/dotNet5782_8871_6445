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
                string sLong = ConvertCoordinates(Latitude), sLatit = ConvertCoordinates(Longitude);    /// Converts the coordinates to be in base 60 (bonus).
                return $"(" + sLatit + "N, " + sLong + "E)";
            }
            public string ConvertCoordinates(double number)
            {
                //string coordinates;
                int sec = (int)Math.Round(number * 3600);
                int deg = sec / 3600;
                sec = Math.Abs(sec % 3600);
                int min = sec / 60;
                sec %= 60;
                //sec = (int)(sec * 100);
                //sec /= 100;
                //min = (int)(min * 100);
                //min /= 100;
                return deg + "" + (char)176 + " " + min + " " + sec;
                //int result, remainder;
                //string coordinates;
                //result = (int)number;
                //coordinates = result + "" + (char)176 + " ";                            /// coordinates holds now the degrees.
                //result = (int)((number - result) * 10000);
                //result *= 60;
                //remainder = result % 10000;
                //result /= 10000;
                //coordinates += result + "\' ";                          /// coordinates holds now the minutes.
                //result = remainder * 60;
                //remainder = result % 10000;
                //remainder /= 100;
                //result /= 10000;
                //coordinates += result + "." + remainder + "\" ";      /// coordinates holds now the seconds.
                //return coordinates;
            }
        }
    }

