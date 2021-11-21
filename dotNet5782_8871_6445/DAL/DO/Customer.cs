namespace IDAL
{
    namespace DO
    {
        public struct Customer
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            

            public Customer(int id, string name, string phone,double latitude, double longitude)
            {
                ID = id;
                Name = name;
                Phone = phone;
                Longitude = longitude;
                Latitude = latitude;
            }
            public override string ToString()
            {
                string sLong = ConvertCoordinates(Longitude), sLatit = ConvertCoordinates(Latitude);    /// Converts the coordinates to be in base 60 (bonus).
                return ($"{Name} #{ID}:\n" +
                    $"Phone number: {Phone}\n" +
                    $"Location: (" + sLong + " E, " + sLatit + " N)\n");
            }

            public string ConvertCoordinates(double number)
            {
                int result, remainder;
                string coordinates;
                result = (int)number;
                coordinates = result + "" + (char)176 + " ";                            /// coordinates holds now the degrees.
                result = (int)((number - result) * 10000);
                result = result * 60;
                remainder = result % 10000;
                result /= 10000;
                coordinates += result + "\' ";                          /// coordinates holds now the minutes.
                result = remainder * 60;
                remainder = result % 10000;
                result /= 10000;
                coordinates += result + "." + remainder + "\" ";      /// coordinates holds now the seconds.
                return coordinates;
            }
        }
    }
}