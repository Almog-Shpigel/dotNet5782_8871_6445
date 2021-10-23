using IDAL.DO;
using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            int choice;
            DalObject.DalObject Data = new();
            do
            {
                Console.WriteLine("Press anykey to start the program.");
                Console.ReadKey();
                Console.Clear();
                choice = Menu();            // Receives a choice from the user.
                CHOICE option = (CHOICE)choice;
                switch (option)
                {
                    case CHOICE.ADD:
                        ADD_CHOICE OptionAdd = (ADD_CHOICE)AddMenu();   // Receives an add choice from the user.
                        switch (OptionAdd)
                        {
                            case ADD_CHOICE.ADD_STATION: Data.AddNewStation(); break;
                            case ADD_CHOICE.ADD_DRONE: Data.AddNewDrone(); break;
                            case ADD_CHOICE.ADD_CUSTOMER: Data.AddNewCustomer(); break;
                            case ADD_CHOICE.ADD_PARCEL: Data.AddNewParcel(); break;
                        }
                        break;
                    case CHOICE.UPDATE:
                        UPDATE_CHOICE OptionUpdate = (UPDATE_CHOICE)UpdateMenu();   // Receives an update choice from the user.
                        switch (OptionUpdate)
                        {
                            case UPDATE_CHOICE.PARCEL_PAIRING: Data.PairParcelToDrone(); break;
                            case UPDATE_CHOICE.PARCEL_COLLECTED: Data.ParcelCollected();  break;
                            case UPDATE_CHOICE.PARCEL_DELEIVERY: Data.ParcelDeleivery(); break;
                            case UPDATE_CHOICE.DRONE_TO_CHARGE: Data.DroneToBeCharge(); break;
                            case UPDATE_CHOICE.DRONE_AVAILABLE: Data.DroneAvailable(); break;
                        }
                        break;
                    case CHOICE.DISPLAY:
                        DISPLAY_CHOICE OptionDisplay = (DISPLAY_CHOICE)DispalyMenu();   // Receives a dispaly choice from the user.
                        switch (OptionDisplay)
                        {
                            case DISPLAY_CHOICE.DISPLAY_STATION: Data.DisplayStation(); break;
                            case DISPLAY_CHOICE.DISPLAY_DRONE: Data.DisplayDrone(); break;
                            case DISPLAY_CHOICE.DISPLAY_CUSTOMER: Data.DisplayCustomer(); break;
                            case DISPLAY_CHOICE.DISPLAY_PARCEL: Data.DisplayParcel(); break;
                        }
                        break;
                    case CHOICE.DATA_PRINT:
                        PRINT_CHOICE OptionPrint = (PRINT_CHOICE)DataPrintMenu();   // Receives a choice from the user to dispaly all data.
                        switch (OptionPrint)
                        {
                            case PRINT_CHOICE.PRINT_STATIONS: Data.PrintAllStation(); break;
                            case PRINT_CHOICE.PRINT_DRONES: Data.PrintAllDrones(); break;
                            case PRINT_CHOICE.PRINT_CUSTOMERS: Data.PrintAllCustomers(); break;
                            case PRINT_CHOICE.PRINT_PARCELS: Data.PrintAllParcels(); break;
                            case PRINT_CHOICE.PRINT_UNASSIGNED_PARCELS: Data.PrintAllUnassignedParcel(); break;
                            case PRINT_CHOICE.PRINT_AVAILABLE_STATIONS: Data.PrintAllAvailableStations(); break;
                        }
                        break;
                    case CHOICE.EXIT: break;    // Exit from menu by choosing 0.
                }
                
            } while (choice != 0);

        }

        private static int Menu()   // Main program menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                               "1- Add\n" +
                               "2- Update\n" +
                               "3- Display\n" +
                               "4- Display all\n" +
                               "0- Exit");
            return Convert.ToInt32(Console.ReadLine());
        }
        private static int AddMenu()   // Add menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                "1- Add new station.\n" +
                                "2- Add new drone.\n" +
                                "3- Add new customer.\n" +
                                "4- Add new parcel.");
            return Convert.ToInt32(Console.ReadLine()) - 1;
        }
        private static int UpdateMenu()   // Update menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                "1-Assign parcel to a drone.\n" +
                                "2-Parcel collected by a drone.\n" +
                                "3-Parcel deleivered to customer.\n" +
                                "4-Send drone to be charged.\n" +
                                "5-Realse drone from charging.");
            return Convert.ToInt32(Console.ReadLine()) - 1;
        }
        private static int DispalyMenu()    // Dispaly menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                 "1-Display station.\n" +
                                 "2-Display drone.\n" +
                                 "3-Display customer.\n" +
                                 "4-Display parcel.");
            return Convert.ToInt32(Console.ReadLine()) - 1;
        }
        private static int DataPrintMenu()  // Data print menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                            "1-Display all stations.\n" +
                            "2-Display all drones.\n" +
                            "3-Display all customers.\n" +
                            "4-Display all parcels.\n" +
                            "5-Display unassigned parcels.\n" +
                            "6-Display all available stations.");
            return Convert.ToInt32(Console.ReadLine()) - 1;
        }
    }
}
