namespace ConsoleUI.IDAL.DO
{
    internal struct Customer
    {
        public int ID;
        public string Name;
        public string Phone;
        public double Longitude;
        public double Latitude;

        public Customer(int iD, string name, string phone,
                        double longitude,double latitude)
        {
            ID = iD;
            Name = name;
            Phone = phone;
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}