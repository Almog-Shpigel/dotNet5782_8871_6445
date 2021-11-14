namespace IDAL
{
    namespace DO
    {
        public struct DroneCharge
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