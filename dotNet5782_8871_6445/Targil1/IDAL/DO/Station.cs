namespace ConsoleUI.IDAL.DO
{
    internal struct Station
    {
        public int ID;
        public int Name;
        public int ChargeSlots;
        public double Longitude;
        public double Latitude;

        public Station(int id, int name, int chargeSlots,
                       double longitude, double latitude)
        {
            ID = id;
            Name = name;
            ChargeSlots = chargeSlots;
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}