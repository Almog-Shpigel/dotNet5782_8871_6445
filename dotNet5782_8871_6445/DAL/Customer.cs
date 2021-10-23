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
                string sLong = ConvertCoordinates(Longitude), sLatit = ConvertCoordinates(Latitude);    // Converts the coordinates to be in base 60 (bonus).
                System.Console.WriteLine($"{Name} #{ID}:\n" +
                    $"Phone number: {Phone}\n" +
                    $"Location: "+ sLong+", "+ sLatit + "\n");
            }
            public string ConvertCoordinates(double number)
            {
                int result, remainder;
                string coordinates;
                result = (int)number;
                coordinates = result + "d ";                            // coordinates holds now the degrees.
                result = (int)((number - result) * 10000);
                result = result * 60;
                remainder = result % 10000;
                result /= 10000;
                coordinates += result + "\' ";                          // coordinates holds now the minutes.
                result = remainder * 60;
                remainder = result % 10000;
                result /= 10000;
                coordinates += result + "." + remainder + "\'\' ";      // coordinates holds now the seconds.
                return coordinates;
            }
        }
    }
}