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
                Console.WriteLine("Press enter to start.");
                Console.ReadKey();
                Console.Clear();
                choice = Menu();
                CHOICE option = (CHOICE)choice;
                switch (option)
                {
                    case CHOICE.ADD:
                        ADD_CHOICE optionAdd = (ADD_CHOICE)AddMenu();
                        switch(optionAdd)
                        {
                            case ADD_CHOICE.ADD_STATION: Data.AddNewStation(); break;
                            case ADD_CHOICE.ADD_DRONE: Data.AddNewDrone(); break;
                            case ADD_CHOICE.ADD_CUSTOMER: Data.AddNewCustomer(); break;
                            case ADD_CHOICE.ADD_PARCEL: Data.AddNewParcel(); break;
                        }
                        break;
                    case CHOICE.UPDATE:
                        UPDATE_CHOICE optionUpdate = (UPDATE_CHOICE)UpdateMenu();
                        switch (optionUpdate)
                        {
                            case UPDATE_CHOICE.PARCEL_PAIRING: break;
                            case UPDATE_CHOICE.PARCEL_COLLECTED: break;
                            case UPDATE_CHOICE.PARCEL_DELEIVERY: break;
                            case UPDATE_CHOICE.DRONE_TO_CHARGE: break;
                            case UPDATE_CHOICE.DRONE_AVAILABLE: break;
                        }
                        break;
                    case CHOICE.DISPLAY:
                        DISPLAY_CHOICE optionDisplay = (DISPLAY_CHOICE)DispalyMenu();
                        switch (optionDisplay)
                        {
                            case DISPLAY_CHOICE.DISPLAY_STATION: break;
                            case DISPLAY_CHOICE.DISPLAY_DRONE: break;
                            case DISPLAY_CHOICE.DISPLAY_CUSTOMER: break;
                            case DISPLAY_CHOICE.DISPLAY_PARCEL: break;
                        }
                        break;
                    case CHOICE.DATA_PRINT:
                        PRINT_CHOICE optionPrint = (PRINT_CHOICE)DataPrintMenu();
                        switch (optionPrint)
                        {
                            case PRINT_CHOICE.PRINT_STATIONS: Data.PrintAllStation(); break;
                            case PRINT_CHOICE.PRINT_DRONES: Data.PrintAllDrones(); break;
                            case PRINT_CHOICE.PRINT_CUSTOMERS: Data.PrintAllCustomers(); break;
                            case PRINT_CHOICE.PRINT_PARCELS: Data.PrintAllParcels(); break;
                            case PRINT_CHOICE.PRINT_UNASSIGNED_PARCELS: Data.PrintAllUnassignedParcel(); break;
                            case PRINT_CHOICE.PRINT_AVAILABLE_STATIONS: Data.PrintAllAvailableStations(); break;
                        }
                        break;
                    case CHOICE.EXIT: break;
                    default:
                        break;
                }
                
            } while (choice != 0);

        }
        private static int Menu()
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                               "1- Add\n" +
                               "2- Update\n" +
                               "3- Display\n" +
                               "4- Display all\n" +
                               "0- Exit");
            string temp = Console.ReadLine();
            return Convert.ToInt32(temp);
        }
        private static int AddMenu()
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                "1- Add new station.\n" +
                                "2- Add new drone.\n" +
                                "3- Add new customer.\n" +
                                "4- Add new parcel.");
            string temp = Console.ReadLine();
            return Convert.ToInt32(temp) - 1;
        }
        private static int UpdateMenu()
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                "1-Assign parcel to a drone.\n" +
                                "2-Parcel collected by a drone.\n" +
                                "3-Parcel deleivered to customer.\n" +
                                "4-Send drone to be charged.\n" +
                                "5-Realse drone from charging.");
            string temp = Console.ReadLine();
            return Convert.ToInt32(temp) - 1;
        }
        private static int DispalyMenu()
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                 "1-Display station.\n" +
                                 "2-Display drone.\n" +
                                 "3-Display customer.\n" +
                                 "4-Display parcel.");
                string temp = Console.ReadLine();
            return Convert.ToInt32(temp) - 1;
        }
        private static int DataPrintMenu()
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                            "1-Display all stations.\n" +
                            "2-Display all drones.\n" +
                            "3-Display all customers.\n" +
                            "4-Display all parcels.\n" +
                            "5-Display unassigned parcels.\n" +
                            "6-Display all available stations.");
            string temp = Console.ReadLine();
            return Convert.ToInt32(temp) - 1;
        }
    }
}
