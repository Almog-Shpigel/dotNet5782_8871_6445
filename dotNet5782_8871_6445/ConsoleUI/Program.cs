using IDAL.DO;
using System;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            int choice;
            bool input;
            DalObject.DalObject Data = new();
            do
            {
                Console.WriteLine("Press anykey to start the program.");
                Console.ReadKey();
                Console.Clear();
                choice = Menu();            /// Receives a choice from the user.
                CHOICE option = (CHOICE)choice;
                switch (option)
                {
                    case CHOICE.ADD:
                        ADD_CHOICE OptionAdd = (ADD_CHOICE)AddMenu();   /// Receives an add choice from the user.
                        switch (OptionAdd)
                        {
                            case ADD_CHOICE.ADD_STATION: AddNewStation(Data); break;
                            case ADD_CHOICE.ADD_DRONE: AddNewDrone(Data); break;
                            case ADD_CHOICE.ADD_CUSTOMER: AddNewCustomer(Data); break;
                            case ADD_CHOICE.ADD_PARCEL: AddNewParcel(Data); break;
                        }
                        break;
                    case CHOICE.UPDATE:
                        UPDATE_CHOICE OptionUpdate = (UPDATE_CHOICE)UpdateMenu();   /// Receives an update choice from the user.
                        switch (OptionUpdate)
                        {
                            case UPDATE_CHOICE.PARCEL_PAIRING:
                                Console.Write("Please enter the ID number of Parcel (6 digits): ");
                                input = int.TryParse(Console.ReadLine(), out int ParcelID);
                                Console.Write("Please enter the ID number of Drone (6 digits): ");
                                input = int.TryParse(Console.ReadLine(), out int DroneId);
                                Data.PairParcelToDrone(ParcelID, DroneId);
                                break;
                            case UPDATE_CHOICE.PARCEL_COLLECTED:
                                Console.Write("Please enter the ID number of Parcel (6 digits): ");
                                input = int.TryParse(Console.ReadLine(), out int id);
                                Data.ParcelCollected(id);
                                break;
                            case UPDATE_CHOICE.PARCEL_DELEIVERY:
                                Console.Write("Please enter the ID number of Parcel (6 digits): ");
                                input = int.TryParse(Console.ReadLine(), out int idNum);
                                Data.ParcelDeleivery(idNum);
                                break;
                            case UPDATE_CHOICE.DRONE_TO_CHARGE:
                                Console.Write("Please enter the ID number of Drone (6 digits): ");
                                input = int.TryParse(Console.ReadLine(), out int droneID );
                                Console.WriteLine("Choose a station ID from the list of availbale stations:");
                                string[] stations = Data.PrintAllAvailableStations();
                                foreach (string item in stations)
                                    Console.WriteLine(item);
                                input = int.TryParse(Console.ReadLine(), out int station);
                                Data.DroneToBeCharge(droneID,station);
                                break;
                            case UPDATE_CHOICE.DRONE_AVAILABLE:
                                Console.Write("Please enter the ID number of Drone (6 digits): ");
                                input = int.TryParse(Console.ReadLine(), out int DroneID);
                                Data.DroneAvailable(DroneID);
                                break;
                        }
                        break;
                    case CHOICE.DISPLAY:
                        DISPLAY_CHOICE OptionDisplay = (DISPLAY_CHOICE)DispalyMenu();   /// Receives a dispaly choice from the user.
                        switch (OptionDisplay)
                        {
                            case DISPLAY_CHOICE.DISPLAY_STATION:
                                Console.Write("Please enter the ID number of station (6 digits): ");
                                input = int.TryParse(Console.ReadLine(), out int StationID);
                                Console.WriteLine(Data.DisplayStation(StationID));
                                break;
                            case DISPLAY_CHOICE.DISPLAY_DRONE:
                                Console.Write("Please enter the ID number of Drone (6 digits): ");
                                input = int.TryParse(Console.ReadLine(), out int DroneID);
                                Console.WriteLine(Data.DisplayDrone(DroneID));
                                break;
                            case DISPLAY_CHOICE.DISPLAY_CUSTOMER:
                                Console.Write("Please enter the ID number of customer (6 digits): ");
                                input = int.TryParse(Console.ReadLine(), out int CustomerID);
                                Console.WriteLine(Data.DisplayCustomer(CustomerID));
                                break;
                            case DISPLAY_CHOICE.DISPLAY_PARCEL:
                                Console.Write("Please enter the ID number of parcel (6 digits): ");
                                input = int.TryParse(Console.ReadLine(), out int ParcelID);
                                Console.WriteLine(Data.DisplayParcel(ParcelID));
                                break;
                            case DISPLAY_CHOICE.DISTANCE_STATION:
                                Console.WriteLine("Please enter the coordinates of a point:");
                                Console.Write("Enter longitude: ");
                                input = double.TryParse(Console.ReadLine(), out double longitude);    ///Reciving location
                                Console.Write("Enter latitude: ");
                                input = double.TryParse(Console.ReadLine(), out double latitude);
                                Console.Write("Please enter station ID: ");
                                input = int.TryParse(Console.ReadLine(), out StationID);
                                Console.WriteLine("The distance is: " + Data.DistanceFromStation(longitude, latitude, StationID) + " km");
                                break;
                            case DISPLAY_CHOICE.DISTANCE_CUSTOMER:
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
                        break;
                    case CHOICE.DATA_PRINT:
                        PRINT_CHOICE OptionPrint = (PRINT_CHOICE)DataPrintMenu();   /// Receives a choice from the user to dispaly all data.
                        switch (OptionPrint)
                        {
                            case PRINT_CHOICE.PRINT_STATIONS:
                                string[] stations = Data.PrintAllStations();
                                foreach (string item in stations)
                                    Console.WriteLine(item);
                                break;
                            case PRINT_CHOICE.PRINT_DRONES: 
                                string[] drones = Data.PrintAllDrones();
                                foreach (string item in drones)
                                    Console.WriteLine(item);
                                break;
                            case PRINT_CHOICE.PRINT_CUSTOMERS:
                                string[] customers = Data.PrintAllCustomers();
                                foreach (string item in customers)
                                    Console.WriteLine(item);
                                break;
                            case PRINT_CHOICE.PRINT_PARCELS: 
                                string[] parcels = Data.PrintAllParcels();
                                foreach (string item in parcels)
                                    Console.WriteLine(item);
                                break;
                            case PRINT_CHOICE.PRINT_UNASSIGNED_PARCELS: 
                                string[] AvaParcels = Data.PrintAllUnassignedParcels();
                                foreach (string item in AvaParcels)
                                    Console.WriteLine(item);
                                break;
                            case PRINT_CHOICE.PRINT_AVAILABLE_STATIONS: 
                                string[] AvaStations = Data.PrintAllAvailableStations();
                                foreach (string item in AvaStations)
                                    Console.WriteLine(item);
                                break;
                        }
                        break;
                    case CHOICE.EXIT: break;    /// Exit from menu by choosing 0.
                }
                
            } while (choice != 0);

        }

        private static int Menu()   /// Main program menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                               "1- Add\n" +
                               "2- Update\n" +
                               "3- Display\n" +
                               "4- Display all\n" +
                               "0- Exit");
            bool input = int.TryParse(Console.ReadLine(), out int choice);
            return choice;
        }
        private static int AddMenu()   /// Add menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                "1- Add new station.\n" +
                                "2- Add new drone.\n" +
                                "3- Add new customer.\n" +
                                "4- Add new parcel.");
            bool input = int.TryParse(Console.ReadLine(), out int choice);
            return choice - 1;
        }
        private static int UpdateMenu()   /// Update menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                "1-Assign parcel to a drone.\n" +
                                "2-Parcel collected by a drone.\n" +
                                "3-Parcel deleivered to customer.\n" +
                                "4-Send drone to be charged.\n" +
                                "5-Realse drone from charging.");
            bool input = int.TryParse(Console.ReadLine(), out int choice);
            return choice - 1;
        }
        private static int DispalyMenu()    /// Dispaly menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                 "1- Display station.\n" +
                                 "2- Display drone.\n" +
                                 "3- Display customer.\n" +
                                 "4- Display parcel.\n" +
                                 "5- Display distance from a station.\n" +
                                 "6- Display distance from a customer.");

            bool input = int.TryParse(Console.ReadLine(), out int choice);
            return choice - 1;
        }
        private static int DataPrintMenu()  /// Data print menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                            "1- Display all stations.\n" +
                            "2- Display all drones.\n" +
                            "3- Display all customers.\n" +
                            "4- Display all parcels.\n" +
                            "5- Display unassigned parcels.\n" +
                            "6- Display all available stations.");
            bool input = int.TryParse(Console.ReadLine(), out int choice);
            return choice - 1;
        }
        private static void AddNewStation(DalObject.DalObject Data)
        {
            bool input;
            Console.Write("Enter longitude: ");
            input = double.TryParse(Console.ReadLine(), out double longitude);    ///Reciving location
            Console.Write("Enter latitude: ");
            input = double.TryParse(Console.ReadLine(), out double latitude);
            Console.WriteLine("Enter number of charge slots:");
            input = int.TryParse(Console.ReadLine(), out int ChargeSlots);
            ///Adding new object to the array
            Data.AddNewStation(longitude, latitude, ChargeSlots);
        }
        private static void AddNewDrone(DalObject.DalObject Data)
        {
            Console.WriteLine("Enter model name: ");
            string model = Console.ReadLine();
            Console.WriteLine("Enter weight category:\n" +
                "0- Light\n" +
                "1- Medium\n" +
                "2- Heavy");
            bool input = int.TryParse(Console.ReadLine(), out int weight);       ///Reciving weight category
            WeightCategories Weight = (WeightCategories)weight;
            Data.AddNewDrone(model, Weight);
        }
        private static void AddNewCustomer(DalObject.DalObject Data)
        {
            bool input;
            Console.Write("Please enter your Customer ID (6 digits): ");    ///Reciving ID
            input = int.TryParse(Console.ReadLine(), out int id);
            Console.Write("Please enter your full name: ");                 ///Reciving name
            string name = Console.ReadLine();
            Console.Write("Please enter your phone number (10 digits): ");  ///Reciving phone number
            string phone = Console.ReadLine();
            Console.WriteLine("Please enter your location:");               ///Reciving location
            Console.Write("Longitude: ");
            input = double.TryParse(Console.ReadLine(), out double longitude);
            Console.Write("Latitude: ");
            input = double.TryParse(Console.ReadLine(), out double latitude);
            Data.AddNewCustomer(id, name, phone, longitude, latitude);
        }
        private static void AddNewParcel(DalObject.DalObject Data)
        {
            bool input;
            Console.Write("Please enter sender ID (6 digits): ");
            input = int.TryParse(Console.ReadLine(), out int sender);
            Console.Write("Please enter receiver ID (6 digits): ");
            input = int.TryParse(Console.ReadLine(), out int target);
            Console.WriteLine("Enter weight category:\n" +
                "0- Light\n" +
                "1- Medium\n" +
                "2- Heavy");
            input = int.TryParse(Console.ReadLine(), out int weight);
            WeightCategories Weight = (WeightCategories)weight; ///Choosing a weight category for the parcel
            Console.WriteLine("Enter priority category:\n" +
               "0- Regular\n" +
               "1- Express\n" +
               "2- Urgent");
            input = int.TryParse(Console.ReadLine(), out int priority);
            Priorities Priority = (Priorities)priority;      ///Choosing a priority category for the parcel
            Data.AddNewParcel(sender, target, Weight, Priority);
        }
    }
}
