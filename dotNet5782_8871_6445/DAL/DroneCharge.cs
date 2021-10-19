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
        }
    }
}