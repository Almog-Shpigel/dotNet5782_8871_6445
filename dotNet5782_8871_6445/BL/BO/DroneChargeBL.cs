using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace BO
    {
        public class DroneChargeBL
        {
            public int DroneID { get; set; }
            public double BatteryStatus { get; set; }

            public DroneChargeBL(int droneID, double battery)
            {
                DroneID = droneID;
                BatteryStatus = battery;
            }

            public override string ToString()
            {
                return ($"Drone #{DroneID}, battery status: {BatteryStatus}%\n");
            }
        }
    }

