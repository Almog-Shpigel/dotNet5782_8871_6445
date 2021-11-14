using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    class Request
    {
        public static int StationID()
        {
            Console.Write("Please enter station ID: ");    ///Reciving ID
            bool input = int.TryParse(Console.ReadLine(), out int StationId);
            return StationId;
        }
        public static string StationName()
        {
            Console.Write("Please enter station name: ");                 ///Reciving name
            return Console.ReadLine();
        }
        public static double Longitude()
        {
            Console.Write("Enter longitude: ");
            bool input = double.TryParse(Console.ReadLine(), out double longitude);    ///Reciving location
            return longitude;
        }
        public static double Latitude()
        {
            Console.Write("Enter latitude: ");
            bool input = double.TryParse(Console.ReadLine(), out double latitude);
            return latitude;
        }
        public static int ChargeSlots()
        {
            Console.Write("Enter number of charge slots:");
            bool input = int.TryParse(Console.ReadLine(), out int ChargeSlots);
            return ChargeSlots;
        }

        public static int ChargeTime()
        {
            Console.WriteLine("Enter amount of time to be recharged: ");
            bool input = int.TryParse(Console.ReadLine(), out int TimeToCharge);
            return TimeToCharge;
        }

        public static int DroneID()
        {
            Console.Write("Please enter drone ID: ");    ///Reciving ID
            bool input = int.TryParse(Console.ReadLine(), out int DroneId);
            return DroneId;
        }
        public static string ModelName()
        {
            Console.WriteLine("Enter model name: ");
            return Console.ReadLine();
        }
        public static WeightCategories WeightCategorie()
        {
            Console.WriteLine("Enter weight category:\n" +
                "0- Light \n1- Medium \n2- Heavy");
            bool input = int.TryParse(Console.ReadLine(), out int weight);       ///Reciving weight category
            return (WeightCategories)weight;
        }
        public static Priorities Prioritie()
        {
            Console.WriteLine("Enter priority category:\n" +
               "0- Regular\n 1- Express\n 2- Urgent");
            bool input = int.TryParse(Console.ReadLine(), out int priority);
            return (Priorities)priority;      ///Choosing a priority category for the parcel
        }
        public static int CustomerID(string name)
        {
            Console.Write($"Please enter {0} ID: ", name);    ///Reciving ID
            bool input = int.TryParse(Console.ReadLine(), out int CustomerId);
            return CustomerId;
        }
        public static string CustomerName()
        {
            Console.Write("Please enter your full name: ");                 ///Reciving name
            return Console.ReadLine();
        }
        public static string PhoneNumber()
        {
            Console.Write("Please enter your phone number: ");  ///Reciving phone number
            return Console.ReadLine();
        }
        public static int ParcelID()
        {
            Console.Write("Please enter parcel ID: ");    ///Reciving ID
            bool input = int.TryParse(Console.ReadLine(), out int ParcelId);
            return ParcelId;
        }
    }
}
