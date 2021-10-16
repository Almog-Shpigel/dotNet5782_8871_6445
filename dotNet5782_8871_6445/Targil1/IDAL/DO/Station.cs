namespace ConsoleUI.IDAL.DO
{
    public struct Station
    {
        public int ID { get; set; }
        public int Name { get; set; }
        public int ChargeSlots { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

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