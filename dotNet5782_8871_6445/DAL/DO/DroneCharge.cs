using System;

namespace IDAL
{
    namespace DO
    {
        public struct DroneCharge
        {
            public int DroneID { get; set; }
            public int StationID { get; set; }
            public DateTime? Start { get; set; }     /// Yair recommended we'll add the time the drone started charging.

            public DroneCharge(int droneID, int stationID, DateTime? start)
            {
                DroneID = droneID;
                StationID = stationID;
                Start = start;
            }

            public override string ToString()
            {
                return ($"Drone #{DroneID} is being charge at {StationID}\n");
            }
        }
    }
}