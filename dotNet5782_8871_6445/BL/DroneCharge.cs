using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class DroneCharge
        {
            public int DroneID { get; set; }
            public int StationID { get; set; }

            public DroneCharge(int droneID, int stationID)
            {
                DroneID = droneID;
                StationID = stationID;
            }

            public override string ToString()
            {
                return ($"Drone #{DroneID} is being charge at {StationID}\n");
            }
        }
    }
}
