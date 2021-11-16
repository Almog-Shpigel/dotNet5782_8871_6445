using IBL;
using IBL.BO;
using IDAL.DO;
using System;
namespace ConsoleUI_BL
{
    public class Program
    {
        static void Main(string[] args)
        {
            IBL.BL IBL = new();
            int MenuChoice;
            do
            {
                Console.WriteLine("Press anykey to start the program...");
                Console.ReadKey();
                Console.Clear();
                MenuChoice = FirstMenu();                                /// Receives a Menu choice from the user.
            } while (MenuChoice != 0);
        }
        #region Menues
        public static int FirstMenu()                /// Main program menu.
        {
            Console.WriteLine("Please choose one of the following options: \n" +
                                "1- Add \n" +
                                "2- Update \n" +
                                "3- Display \n" +
                                "4- Display all \n" +
                                "0- Exit");
            bool success;
            int MenuChoice;
            do
            {
                success = int.TryParse(Console.ReadLine(), out MenuChoice);
            } while (!success);

            CHOICE option = (CHOICE)MenuChoice;
            switch (option)
            {
                case CHOICE.ADD: ADD(); break;       /// Receives an add choice from the user.
                case CHOICE.UPDATE: UPDATE(); break;
                case CHOICE.DISPLAY: DISPLAY(); break;
                case CHOICE.DATA_PRINT: DISPLAY_DATA(); break;
                case CHOICE.EXIT: break;                    /// Exit from menu by choosing 0.
            }
            return MenuChoice;
        }
        public static void ADD()       /// Add menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                "1- Add new station. \n" +
                                "2- Add new drone. \n" +
                                "3- Add new customer. \n" +
                                "4- Add new parcel.");
            bool success;
            ADD_CHOICE AddOption;
            do
            {
                success = ADD_CHOICE.TryParse(Console.ReadLine(), out AddOption);
            } while (!success);

            switch (AddOption)
            {
                case ADD_CHOICE.ADD_STATION: AddNewStation(IBL); break;
                case ADD_CHOICE.ADD_DRONE: Add.NewDrone(IBL); break;
                case ADD_CHOICE.ADD_CUSTOMER: Add.NewCustomer(IBL); break;
                case ADD_CHOICE.ADD_PARCEL: Add.NewParcel(IBL); break;
            }
        }
        public static void UPDATE()   /// Update menu.
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
            bool success;
            UPDATE_CHOICE UpdateOption;
            do
            {
                success = UPDATE_CHOICE.TryParse(Console.ReadLine(), out UpdateOption);
            } while (!success);

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
        public static void DISPLAY()   /// Update menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                 "1- Display station.\n" +
                                 "2- Display drone.\n" +
                                 "3- Display customer.\n" +
                                 "4- Display parcel.\n" +
                                 "5- Display distance from a station.\n" +
                                 "6- Display distance from a customer.");
            bool success;
            DISPLAY_CHOICE DisplayOption;
            do
            {
                success = DISPLAY_CHOICE.TryParse(Console.ReadLine(), out DisplayOption);
            } while (!success);

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

        public static void DISPLAY_DATA()  /// Data print menu.
        {
            Console.WriteLine("Please choose one of the following options:\n" +
                                "1- Display all stations.\n" +
                                "2- Display all drones.\n" +
                                "3- Display all customers.\n" +
                                "4- Display all parcels.\n" +
                                "5- Display unassigned parcels.\n" +
                                "6- Display all available stations.");
            bool success;
            PRINT_CHOICE PrintOption;
            do
            {
                success = PRINT_CHOICE.TryParse(Console.ReadLine(), out PrintOption);
            } while (!success);

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
        #endregion
        #region Add
      private void AddNewStation(IBL.BL IBL)
            {
                int id = RequestID();
                string name = RequestStationName();
                Location location = RequestLocation();
                int slots = RequestChargeSlots();
                StationBL NewStation = new StationBL(id, name, slots, location);
                IBL.AddNewStation(NewStation);
            }

    //private void NewDrone(IBL.BL IBL)
            //{
            //    int id = RequestID();
            //    string model = RequestModelName();
            //    WeightCategories weight = RequestWeightCategorie();
            //    int StationID = RequestID();
            //    DroneBL NewDrone = new DroneBL(id,model,weight);
            //    IBL.AddNewDrone(NewDrone,StationID);
            //}

      private void AddNewCustomer(IBL.BL IBL)
            {
                int id = RequestID();
                string name = RequestCustomerName();
                string phone = RequestPhoneNumber();
                Location location = RequestLocation();
                CustomerBL NewCustomer = new CustomerBL(id, name, phone, location);
                IBL.AddNewCustomer(NewCustomer);
            }
      private void AddNewParcel(IBL.BL IBL)
            {
                int SenderID = RequestID();
                int TargetID = RequestID();
                WeightCategories weight = RequestWeightCategorie();
                Priorities priority = RequestPrioritie();
                ParcelBL NewParcel = new ParcelBL(SenderID, TargetID, weight, priority);
                IBL.AddNewParcel(NewParcel);
            }
        #endregion
        #region Update
        private void UpdateDroneName(IBL.BL IBL)
            {
                int id = RequestID();
                string model = RequestModelName();
                IBL.UpdateDroneName(id, model);
            }
        private void UpdateStation(IBL.BL IBL)
            {
                int id = RequestID();
                string name = RequestStationName();
                int slots = RequestChargeSlots();
                IBL.UpdateStation(id, name, slots);
            }
        private void UpdateCustomer(IBL.BL IBL)
            {
                int id = RequestID();
                string name = RequestCustomerName();
                string phone = RequestPhoneNumber();
                IBL.UpdateCustomer(id, name, phone);
            }
        private void PairParcelToDrone(IBL.BL IBL)
            {
                int id = RequestID();
                IBL.UpdateParcelToDrone(id);
            }

        private void ParcelCollectedByDrone(IBL.BL IBL)
            {
                int id = RequestID();
                IBL.UpdateParcelCollectedByDrone(id);
            }

        private void ParcelDeleiveredByDrone(IBL.BL IBL)
            {
                int id = RequestID();
                IBL.UpdateParcelDeleiveredByDrone(id);
            }

        private void DroneToBeCharged(IBL.BL IBL)
            {
                int id = RequestID();
                IBL.UpdateDroneToBeCharged(id);
            }

        private void DroneAvailable(IBL.BL IBL)
            {
                int id = RequestID();
                int MinutesInCharge = RequestMinutesInCharge();
                IBL.UpdateDroneAvailable(id, MinutesInCharge);
            }
        #endregion
        #region Request
        private int RequestID()
            {
                Console.Write("Please enter ID: ");    ///Reciving ID
                bool input = int.TryParse(Console.ReadLine(), out int StationId);
                return StationId;
            }
        private string RequestStationName()
            {
                Console.Write("Please enter station name: ");                 ///Reciving name
                return Console.ReadLine();
            }
        private Location RequestLocation()
            {
                Console.Write("Enter longitude: ");
                bool lon = double.TryParse(Console.ReadLine(), out double longitude);    ///Reciving location
                Console.Write("Enter latitude: ");
                bool lat = double.TryParse(Console.ReadLine(), out double latitude);
                Location location = new Location(latitude, longitude);
                return location;
            }
            
        private int RequestChargeSlots()
            {
                Console.Write("Enter number of charge slots:");
                bool input = int.TryParse(Console.ReadLine(), out int ChargeSlots);
                return ChargeSlots;
            }


        private string RequestModelName()
            {
                Console.WriteLine("Enter model name: ");
                return Console.ReadLine();
            }
        private WeightCategories RequestWeightCategorie()
            {
                Console.WriteLine("Enter weight category:\n" +
                    "0- Light \n1- Medium \n2- Heavy");
                bool input = int.TryParse(Console.ReadLine(), out int weight);       ///Reciving weight category
                return (WeightCategories)weight;
            }
        private Priorities RequestPrioritie()
            {
                Console.WriteLine("Enter priority category:\n" +
                   "0- Regular\n 1- Express\n 2- Urgent");
                bool input = int.TryParse(Console.ReadLine(), out int priority);
                return (Priorities)priority;      ///Choosing a priority category for the parcel
            }
        private string RequestCustomerName()
            {
                Console.Write("Please enter full name: ");                 ///Reciving name
                return Console.ReadLine();
            }
        private string RequestPhoneNumber()
            {
                Console.Write("Please enter phone number: ");  ///Reciving phone number
                return Console.ReadLine();
            }
        

        private int RequestMinutesInCharge()
        {
            bool success;
            int minutes = 0;
            do
            {
                Console.WriteLine("Please enter amount of minutes in charging");
                success = int.TryParse(Console.ReadLine(), out minutes);
            } while (success);
            return minutes;
        }
        #endregion
    }
}
