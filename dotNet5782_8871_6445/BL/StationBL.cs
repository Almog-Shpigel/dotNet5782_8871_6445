﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class StationBL
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int ChargeSlots { get; set; }
            public Location Location { get; set; }
            public List<DroneCharge> ChargingDrones;

        public StationBL(int id, string name, int slots,
                           double longitude, double latitude)
            {
                ID = id;
                Name = name;
                ChargeSlots = slots;
                Location = new (longitude, latitude);
                ChargingDrones = new();
            }
            public override string ToString()
            {
                string sLong = ConvertCoordinates(Location.Longitude), sLatit = ConvertCoordinates(Location.Latitude);    /// Converts the coordinates to be in base 60 (bonus).
                return ($"{Name} #{ID}:\n" +
                    $"Charge slots available: {ChargeSlots}\n" +
                    $"Location: (" + sLong + "E, " + sLatit + "N)\n");
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
