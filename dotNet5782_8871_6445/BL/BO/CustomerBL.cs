using System;
using IBL.BO;
using System.Collections.Generic;

namespace IBL
{
    namespace BO
    {
        class CustomerBL
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public Location Location { get; set; }
            public IEnumerable<ParcelBL> ParcelesSentByCustomer { get; set; }
            public IEnumerable<ParcelBL> ParcelesSentToCustomer { get; set; }
            public CustomerBL(int id, string name, string phone,
                            double longitude, double latitude)
            {
                ID = id;
                Name = name;
                Phone = phone;
                Location.Longitude = longitude;
                Location.Latitude = latitude;
            }
            public override string ToString()
            {
                string sLong = ConvertCoordinates(Location.Longitude), sLatit = ConvertCoordinates(Location.Latitude);    /// Converts the coordinates to be in base 60 (bonus).
                return ($"{Name} #{ID}:\n" +
                    $"Phone number: {Phone}\n" +
                    $"Location: (" + sLong + " E, " + sLatit + " N)\n");
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
