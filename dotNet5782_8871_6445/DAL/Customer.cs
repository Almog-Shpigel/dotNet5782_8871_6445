namespace IDAL
{
    namespace DO
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
            public void print()
            {
                string sLong = convertLongLatit(Longitude), sLatit = convertLongLatit(Latitude);
                System.Console.WriteLine($"{Name} #{ID}:\n" +
                    $"Phone number: {Phone}\n" +
                    $"Location: "+ sLong+", "+ sLatit + "\n");
            }
            public string convertLongLatit(double number)
            {
                int result, temp;
                string sLongLatit;
                result = (int)number;
                sLongLatit = result + "d ";
                result = (int)((number - result) * 10000);
                result = result * 60;
                temp = result % 10000;
                result /= 10000;
                sLongLatit += result + "\' ";
                result = temp * 60;
                temp = result % 10000;
                result /= 10000;
                sLongLatit += result + "." + temp + "\'\' ";
                return sLongLatit;
            }
        }
    }
}