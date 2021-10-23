using DalObject;
using static DalObject.DataSource;

namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int ChargeSlots { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }

            public Station(int id, string name, int chargeSlots,
                           double longitude, double latitude)
            {
                
                ID = id;
                Name = name;
                ChargeSlots = chargeSlots;
                Longitude = longitude;
                Latitude = latitude;
            }
            public void print()
            {
                string sLong = convertLongLatit(Longitude), sLatit = convertLongLatit(Latitude);
                //newLatt = (int)Latitude;
                System.Console.WriteLine($"{Name} #{ID}:\n" +
                    $"Charge slots available: {ChargeSlots}\n" +
                    $"Location: (" + sLong + ", " + sLatit + ")\n");
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