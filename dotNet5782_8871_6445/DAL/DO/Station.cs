using DalObject;
using static DalObject.DataSource;


namespace DO
{
    public struct Station
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ChargeSlots { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }  

        public Station(int id = 0, string name = "", int slots = 0, double latitude = 0, double longitude = 0)
        {
            ID = id;
            Name = name;
            ChargeSlots = slots;
            Longitude = longitude;
            Latitude = latitude;
        }

        public override string ToString()
        {
            string sLong = ConvertCoordinates(Longitude), sLatit = ConvertCoordinates(Latitude);    /// Converts the coordinates to be in base 60 (bonus).
            return ($"{Name} #{ID}:\n" +
                $"Charge slots available: {ChargeSlots}\n" +
                $"Location: (" + sLatit + "N, " + sLong + "E)");
        }

        public string ConvertCoordinates(double number)
        {
            int result, remainder;
            string coordinates;
            result = (int)number;
            coordinates = result + "" + (char)176 + " ";                            /// coordinates holds now the degrees.
            result = (int)((number - result) * 10000);
            result *= 60;
            remainder = result % 10000;
            result /= 10000;
            coordinates += result + "\' ";                          /// coordinates holds now the minutes.
            result = remainder * 60;
            remainder = result % 10000;
            remainder /= 100;
            result /= 10000;
            coordinates += result + "." + remainder + "\" ";      /// coordinates holds now the seconds.
            return coordinates;
        }
    }
}
