namespace ConsoleUI.IDAL.DO
{
    internal struct DroneCharge
    {
        public int DroneID;
        public int StationID;

        public DroneCharge(int droneID, int stationID)
        {
            DroneID = droneID;
            StationID = stationID;
        }
    }
}