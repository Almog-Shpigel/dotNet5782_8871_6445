using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{

    class Menus
    {
        public static int Main(BL.BL IBL)                /// Main program menu.
        {
            Console.WriteLine("Please choose one of the following options: \n" +
                                "1- Add \n2- Update \n3- Display \n4- Display all \n0- Exit");
            bool input = int.TryParse(Console.ReadLine(), out int choice);

            CHOICE option = (CHOICE)choice;
            switch (option)
            {
                case CHOICE.ADD: Menus.ADD(IBL); break;       /// Receives an add choice from the user.
                case CHOICE.UPDATE: Menus.UPDATE(IBL); break;
                case CHOICE.DISPLAY: break;
                case CHOICE.DATA_PRINT: break;
                case CHOICE.EXIT: break;                    /// Exit from menu by choosing 0.
            }
            return choice;
        }
        public static void ADD(BL.BL IBL)       /// Add menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                "1- Add new station. \n2- Add new drone. \n3- Add new customer. \n4- Add new parcel.");
            bool input = ADD_CHOICE.TryParse(Console.ReadLine(), out ADD_CHOICE AddOption);
            switch (AddOption)
            {
                case ADD_CHOICE.ADD_STATION: Add.NewStation(IBL); break;
                case ADD_CHOICE.ADD_DRONE: Add.NewDrone(IBL); break;
                case ADD_CHOICE.ADD_CUSTOMER: Add.NewCustomer(IBL); break;
                case ADD_CHOICE.ADD_PARCEL: Add.NewParcel(IBL); break;
            }
        }

        public static void UPDATE(BL.BL IBL)   /// Update menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                "1- Update Drone's name. \n" +
                                "2- Update Station. \n" +
                                "3- Update Customer. \n" +
                                "4- Assign parcel to a drone. \n" +
                                "5- Parcel collected by a drone. \n" +
                                "6- Parcel deleivered to customer. \n" +
                                "7- Send drone to be charged. \n" +
                                "8- Realse drone from charging.");
            bool input = UPDATE_CHOICE.TryParse(Console.ReadLine(), out UPDATE_CHOICE UpdateOption);
            switch (UpdateOption)
            {
                case UPDATE_CHOICE.DRONE: Update.DroneName(IBL); break;
                case UPDATE_CHOICE.STATION: Update.Station(IBL); break;
                case UPDATE_CHOICE.CUSTOMER: Update.Customer(IBL); break;
                case UPDATE_CHOICE.PARCEL_PAIRING: Update.PairParcelToDrone(IBL); break;
                case UPDATE_CHOICE.PARCEL_COLLECTED: Update.ParcelCollectedByDrone(IBL); break;
                case UPDATE_CHOICE.PARCEL_DELEIVERY: Update.ParcelDeleiveredByDrone(IBL); break;
                case UPDATE_CHOICE.DRONE_TO_CHARGE: Update.DroneToBeCharged(IBL); break;
                case UPDATE_CHOICE.DRONE_AVAILABLE: Update.DroneAvailable(IBL); break;
            }
        }

        private static void DISPLAY(BL.BL IBL)   /// Update menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                "1-Assign parcel to a drone.\n" +
                                "2-Parcel collected by a drone.\n" +
                                "3-Parcel deleivered to customer.\n" +
                                "4-Send drone to be charged.\n" +
                                "5-Realse drone from charging.");
            bool input = DISPLAY_CHOICE.TryParse(Console.ReadLine(), out DISPLAY_CHOICE DisplayOption);
            switch (DisplayOption)
            {
                case DISPLAY_CHOICE.DISPLAY_STATION: Display.Station(IBL); break;
                case DISPLAY_CHOICE.DISPLAY_DRONE: Display.Drone(IBL); break;
                case DISPLAY_CHOICE.DISPLAY_CUSTOMER: Display.Customer(IBL); break;
                case DISPLAY_CHOICE.DISPLAY_PARCEL: Display.Parcel(IBL); break;
                case DISPLAY_CHOICE.DISTANCE_STATION: Display.DistanceFromStation(IBL); break;
                    Console.WriteLine("Please enter the coordinates of a point:");
                    Console.Write("Enter longitude: ");
                    input = double.TryParse(Console.ReadLine(), out double longitude);    ///Reciving location
                    Console.Write("Enter latitude: ");
                    input = double.TryParse(Console.ReadLine(), out double latitude);
                    Console.Write("Please enter station ID: ");
                    input = int.TryParse(Console.ReadLine(), out StationID);
                    Console.WriteLine("The distance is: " + Data.DistanceFromStation(longitude, latitude, StationID) + " km");
                    break;
                case DISPLAY_CHOICE.DISTANCE_CUSTOMER: Display.DistanceFromCustomer(IBL); break;
                    Console.WriteLine("Please enter the coordinates of a point:");
                    Console.Write("Enter longitude: ");
                    input = double.TryParse(Console.ReadLine(), out longitude);    ///Reciving location
                    Console.Write("Enter latitude: ");
                    input = double.TryParse(Console.ReadLine(), out latitude);
                    Console.Write("Please enter customer ID: ");
                    input = int.TryParse(Console.ReadLine(), out CustomerID);
                    Console.WriteLine("The distance is: " + Data.DistanceFromStation(longitude, latitude, CustomerID) + " km");
                    break;
            }
        }

    }

}
