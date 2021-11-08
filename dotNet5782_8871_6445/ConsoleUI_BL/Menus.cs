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
        public static int Main(IBL.BL IBL)                /// Main program menu.
        {
            Console.WriteLine("Please choose one of the following options: \n" +
                                "1- Add \n" +
                                "2- Update \n" +
                                "3- Display \n" +
                                "4- Display all \n" +
                                "0- Exit");
            bool input = int.TryParse(Console.ReadLine(), out int choice);

            CHOICE option = (CHOICE)choice;
            switch (option)
            {
                case CHOICE.ADD: ADD(IBL); break;       /// Receives an add choice from the user.
                case CHOICE.UPDATE: UPDATE(IBL); break;
                case CHOICE.DISPLAY: DISPLAY(IBL); break;
                case CHOICE.DATA_PRINT: DISPLAY_DATA(IBL); break;
                case CHOICE.EXIT: break;                    /// Exit from menu by choosing 0.
            }
            return choice;
        }
        public static void ADD(IBL.BL IBL)       /// Add menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                "1- Add new station. \n" +
                                "2- Add new drone. \n" +
                                "3- Add new customer. \n" +
                                "4- Add new parcel.");
            bool input = ADD_CHOICE.TryParse(Console.ReadLine(), out ADD_CHOICE AddOption);
            switch (AddOption)
            {
                case ADD_CHOICE.ADD_STATION: Add.NewStation(IBL); break;
                case ADD_CHOICE.ADD_DRONE: Add.NewDrone(IBL); break;
                case ADD_CHOICE.ADD_CUSTOMER: Add.NewCustomer(IBL); break;
                case ADD_CHOICE.ADD_PARCEL: Add.NewParcel(IBL); break;
            }
        }

        public static void UPDATE(IBL.BL IBL)   /// Update menu.
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

        private static void DISPLAY(IBL.BL IBL)   /// Update menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                 "1- Display station.\n" +
                                 "2- Display drone.\n" +
                                 "3- Display customer.\n" +
                                 "4- Display parcel.\n" +
                                 "5- Display distance from a station.\n" +
                                 "6- Display distance from a customer.");
            bool input = DISPLAY_CHOICE.TryParse(Console.ReadLine(), out DISPLAY_CHOICE DisplayOption);
            switch (DisplayOption)
            {
                case DISPLAY_CHOICE.DISPLAY_STATION: Display.Station(IBL); break;
                case DISPLAY_CHOICE.DISPLAY_DRONE: Display.Drone(IBL); break;
                case DISPLAY_CHOICE.DISPLAY_CUSTOMER: Display.Customer(IBL); break;
                case DISPLAY_CHOICE.DISPLAY_PARCEL: Display.Parcel(IBL); break;
                case DISPLAY_CHOICE.DISTANCE_STATION: Display.DistanceFromStation(IBL); break;
                case DISPLAY_CHOICE.DISTANCE_CUSTOMER: Display.DistanceFromCustomer(IBL); break;
            }
        }

        private static void DISPLAY_DATA(IBL.BL IBL)  /// Data print menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                "1- Display all stations.\n" +
                                "2- Display all drones.\n" +
                                "3- Display all customers.\n" +
                                "4- Display all parcels.\n" +
                                "5- Display unassigned parcels.\n" +
                                "6- Display all available stations.");
            bool input = PRINT_CHOICE.TryParse(Console.ReadLine(), out PRINT_CHOICE PrintOption);
            switch (PrintOption)
            {
                case PRINT_CHOICE.PRINT_STATIONS: DisplayData.Stations(IBL); break;
                case PRINT_CHOICE.PRINT_DRONES: DisplayData.Drones(IBL); break;
                case PRINT_CHOICE.PRINT_CUSTOMERS: DisplayData.Customers(IBL); break;
                case PRINT_CHOICE.PRINT_PARCELS: DisplayData.Parcels(IBL); break;
                case PRINT_CHOICE.PRINT_UNASSIGNED_PARCELS: DisplayData.UnassignedParcels(IBL); break;
                case PRINT_CHOICE.PRINT_AVAILABLE_STATIONS: DisplayData.AvailableStations(IBL); break;
            }
        }
    }
}
