using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    class Display
    {
        public static void Station(BL.BL IBL)
        {
            Console.WriteLine(IBL.DisplayStation(Request.StationID()));
        }

        public static void Drone(BL.BL IBL)
        {
            Console.WriteLine(IBL.DisplayDrone(Request.DroneID()));
        }

        public static void Customer(BL.BL IBL)
        {
            Console.WriteLine(IBL.DisplayCustomer(Request.CustomerID("customer")));
        }

        public static void Parcel(BL.BL IBL)
        {
            Console.WriteLine(IBL.DisplayParcel(Request.ParcelID()));
        }

        public static void DistanceFromStation(BL.BL IBL)
        {
            Console.WriteLine(IBL.DisplayDistanceFromStation(Request.Longitude(), Request.Latitude(), Request.StationID()));
        }

        public static void DistanceFromCustomer(BL.BL IBL)
        {
            Console.WriteLine(IBL.DisplayDistanceFromCustomer(Request.Longitude(),Request.Latitude(), Request.DroneID()));
        }
    }
}
