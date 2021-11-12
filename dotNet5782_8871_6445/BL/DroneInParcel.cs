using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    class DroneInParcel
    {
        public int ID { get; set; }
        public double BatteryStatus { get; set; }
        public Location Location { get; set; }
        public DroneInParcel(int id, double battery, Location location)
        {
            ID = id;
            BatteryStatus = battery;
            Location = location;
        }
    }
}
