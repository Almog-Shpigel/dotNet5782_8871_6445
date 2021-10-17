namespace ConsoleUI.IDAL.DO
{
    public struct Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public Customer(int id, string name, string phone,
                        double longitude, double latitude)
        {
            ID = id;
            Name = name;
            Phone = phone;
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}