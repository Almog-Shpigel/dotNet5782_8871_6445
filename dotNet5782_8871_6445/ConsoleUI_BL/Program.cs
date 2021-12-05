
using IBL;
using IBL.BO;
using IDAL.DO;
using System;
using System.Collections.Generic;

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
                MenuChoice = FirstMenu(IBL);                                /// Receives a Menu choice from the user.
            } while (MenuChoice != 0);
        }
        #region Menues
        public static int FirstMenu(IBL.BL IBL)                /// Main program menu.
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
                case CHOICE.ADD: ADD(IBL); break;       /// Receives an add choice from the user.
                case CHOICE.UPDATE: UPDATE(IBL); break;
                case CHOICE.DISPLAY: DISPLAY(IBL); break;
                case CHOICE.DATA_PRINT: DISPLAY_DATA(IBL); break;
                case CHOICE.EXIT: break;                    /// Exit from menu by choosing 0.
            }
            return MenuChoice;
        }
        public static void ADD(IBL.BL IBL)       /// Add menu.
        {
            bool success;
            string input;
            ADD_CHOICE AddOption;
            do
            {
                Console.WriteLine("Please choose one of the following options:\n" +
                                "1- Add new station. \n" +
                                "2- Add new drone. \n" +
                                "3- Add new customer. \n" +
                                "4- Add new parcel. \n" +
                                "r- Return to the main menu.");

                input = Console.ReadLine();
                success = ADD_CHOICE.TryParse(input, out AddOption);
            } while (!success && input != "r");

            switch (AddOption)
            {
                case ADD_CHOICE.ADD_STATION: AddNewStation(IBL); break;
                case ADD_CHOICE.ADD_DRONE: AddNewDrone(IBL); break;
                case ADD_CHOICE.ADD_CUSTOMER: AddNewCustomer(IBL); break;
                case ADD_CHOICE.ADD_PARCEL: AddNewParcel(IBL); break;
            }
        }
        public static void UPDATE(IBL.BL IBL)   /// Update menu.
        {
            bool success;
            string input;
            UPDATE_CHOICE UpdateOption;
            do
            {
                Console.WriteLine("Please choose one of the following options:\n" +
                                "1- Update Drone's name. \n" +
                                "2- Update Station. \n" +
                                "3- Update Customer. \n" +
                                "4- Assign parcel to a drone. \n" +
                                "5- Parcel collected by a drone. \n" +
                                "6- Parcel deleivered to customer. \n" +
                                "7- Send drone to be charged. \n" +
                                "8- Realse drone from charging. \n" +
                                "r- Return to the main menu.");

                input = Console.ReadLine();
                success = UPDATE_CHOICE.TryParse(input, out UpdateOption);
            } while (!success && input != "r");

            switch (UpdateOption)
            {
                case UPDATE_CHOICE.DRONE: UpdateDroneName(IBL); break;
                case UPDATE_CHOICE.STATION: UpdateStation(IBL); break;
                case UPDATE_CHOICE.CUSTOMER: UpdateCustomer(IBL); break;
                case UPDATE_CHOICE.PARCEL_PAIRING: PairParcelToDrone(IBL); break;
                case UPDATE_CHOICE.PARCEL_COLLECTED: ParcelCollectedByDrone(IBL); break;
                case UPDATE_CHOICE.PARCEL_DELEIVERY: ParcelDeleiveredByDrone(IBL); break;
                case UPDATE_CHOICE.DRONE_TO_CHARGE: DroneToBeCharged(IBL); break;
                case UPDATE_CHOICE.DRONE_AVAILABLE: DroneAvailable(IBL); break;
            }
        }
        public static void DISPLAY(IBL.BL IBL)   /// Update menu.
        {
            bool success;
            string input;
            DISPLAY_CHOICE DisplayOption;
            do
            {
                Console.WriteLine("Please choose one of the following options:\n" +
                             "1- Display station.\n" +
                             "2- Display drone.\n" +
                             "3- Display customer.\n" +
                             "4- Display parcel.\n" +
                             "5- Display distance from a station.\n" +
                             "6- Display distance from a customer. \n" +
                             "r- Return to the main menu.");

                input = Console.ReadLine();
                success = DISPLAY_CHOICE.TryParse(input, out DisplayOption);
            } while (!success && input != "r");

            switch (DisplayOption)
            {
                case DISPLAY_CHOICE.DISPLAY_STATION: DisplayStation(IBL); break;
                case DISPLAY_CHOICE.DISPLAY_DRONE: DisplayDrone(IBL); break;
                case DISPLAY_CHOICE.DISPLAY_CUSTOMER: DisplayCustomer(IBL); break;
                case DISPLAY_CHOICE.DISPLAY_PARCEL: DisplayParcel(IBL); break;
                case DISPLAY_CHOICE.DISTANCE_STATION: DisplayDistanceFromStation(IBL); break;
                case DISPLAY_CHOICE.DISTANCE_CUSTOMER: DisplayDistanceFromCustomer(IBL); break;
            }
        }

        public static void DISPLAY_DATA(IBL.BL IBL)  /// Data print menu.
        {
            bool success;
            string input;
            PRINT_CHOICE PrintOption;
            do
            {
                Console.WriteLine("Please choose one of the following options:\n" +
                                "1- Display all stations.\n" +
                                "2- Display all drones.\n" +
                                "3- Display all customers.\n" +
                                "4- Display all parcels.\n" +
                                "5- Display unassigned parcels.\n" +
                                "6- Display all available stations. \n" +
                                "r- Return to the main menu.");

                input = Console.ReadLine();
                success = PRINT_CHOICE.TryParse(input, out PrintOption);
            } while (!success && input != "r");

            switch (PrintOption)
            {
                case PRINT_CHOICE.PRINT_STATIONS:
                    List<StationToList> stations = IBL.DispalyAllStations();
                    foreach (var station in stations)
                        Console.WriteLine(station.ToString());
                    break;
                case PRINT_CHOICE.PRINT_DRONES:
                    List<DroneToList> drones = IBL.DispalyAllDrones();
                    foreach (var drone in drones)
                        Console.WriteLine(drone.ToString());
                    break;
                case PRINT_CHOICE.PRINT_CUSTOMERS:
                    List<CustomerToList> customers = IBL.DispalyAllCustomers();
                    foreach (var customer in customers)
                        Console.WriteLine(customer.ToString());
                    break;
                case PRINT_CHOICE.PRINT_PARCELS:
                    List<ParcelToList> parcels = IBL.DispalyAllParcels();
                    foreach (var parcel in parcels)
                        Console.WriteLine(parcel.ToString());
                    break;
                case PRINT_CHOICE.PRINT_UNASSIGNED_PARCELS:
                    List<ParcelToList> UnassiPars = IBL.DispalyAllUnassignedParcels();
                    foreach (var UnassiPar in UnassiPars)
                        Console.WriteLine(UnassiPar.ToString());
                    break;
                case PRINT_CHOICE.PRINT_AVAILABLE_STATIONS:
                    List<StationToList> AvailStats = IBL.DispalyAllAvailableStations();
                    foreach (var AvailStat in AvailStats)
                        Console.WriteLine(AvailStat.ToString());
                    break;
            }
        }
        #endregion

        #region Add
        public static void AddNewStation(IBL.BL IBL)
        {
            int id = RequestID("station");
            string name = RequestStationName();
            Location location = RequestLocation();
            int slots = RequestChargeSlots();
            StationBL NewStation = new(id, name, slots, location);
            try
            {
                IBL.AddNewStation(NewStation);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                AddNewStation(IBL);
            }
        }

        private static void AddNewDrone(IBL.BL IBL)
        {
            int id = RequestID("drone");
            string model = RequestModelName();
            WeightCategories weight = RequestWeightCategorie();
            int StationID = RequestID("station");
            DroneBL NewDrone = new DroneBL(id, model, weight);
            try
            {
                IBL.AddNewDrone(NewDrone, StationID);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                AddNewDrone(IBL);
            }
        }

        private static void AddNewCustomer(IBL.BL IBL)
        {
            int id = RequestID("customer");
            string name = RequestCustomerName();
            string phone = RequestPhoneNumber();
            Location location = RequestLocation();
            CustomerBL NewCustomer = new CustomerBL(id, name, phone, location);
            try
            {
                IBL.AddNewCustomer(NewCustomer);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                AddNewCustomer(IBL);
            }
        }

        private static void AddNewParcel(IBL.BL IBL)
        {
            int SenderID = RequestID("sender");
            int TargetID = RequestID("target");
            WeightCategories weight = RequestWeightCategorie();
            Priorities priority = RequestPrioritie();
            ParcelBL NewParcel = new ParcelBL(SenderID, TargetID, weight, priority);
            try
            {
                IBL.AddNewParcel(NewParcel);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                AddNewParcel(IBL);
            }
        }
        #endregion

        #region Update
        private static void UpdateDroneName(IBL.BL IBL)
        {
            int id = RequestID("drone");
            string model = RequestModelName();
            try
            {
                IBL.UpdateDroneName(id, model);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        private static void UpdateStation(IBL.BL IBL)
        {

            bool success, ChangeName = true, ChangeSlots = true;
            int slots = 0;
            string ChargeSlots;
            int id = RequestID("station");
            string name = RequestStationName();
            if (name == "")
                ChangeName = false;
            do
            {
                Console.Write("Enter number of charge slots: ");
                ChargeSlots = Console.ReadLine();
                if (ChargeSlots == "")
                {
                    ChangeSlots = false;
                    break;
                }
                success = int.TryParse(ChargeSlots, out slots);
            } while (!success);

            try
            {
                IBL.UpdateStation(id,ChangeName,ChangeSlots,name, slots);
            }
            catch (Exception exp) 
            {
                Console.WriteLine(exp.Message);
            }
        }

        private static void UpdateCustomer(IBL.BL IBL)
        {
            bool successs, ChangeName = true, ChangePhone = true ;
            string phone;
            int PhoneNumber = 0;
            int id = RequestID("customer");
            string name = RequestCustomerName();
            if (name == "")
                ChangeName = false;
            do
            {
                Console.Write("Enter phone number:");
                 phone = Console.ReadLine();
                if (phone == "")
                {
                    ChangePhone = false;
                    break;
                }
                successs = int.TryParse(phone, out PhoneNumber);
            } while (!successs);
            try
            {
                IBL.UpdateCustomer(id, ChangeName, ChangePhone, name, PhoneNumber);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        private static void PairParcelToDrone(IBL.BL IBL)
        {
            int id = RequestID("drone");
            try
            {
                IBL.UpdateParcelToDrone(id);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        private static void ParcelCollectedByDrone(IBL.BL IBL)
        {
            int id = RequestID("drone");
            try
            {
                IBL.UpdateParcelCollectedByDrone(id);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        private static void ParcelDeleiveredByDrone(IBL.BL IBL)
        {
            int id = RequestID("drone");
            try
            {
                IBL.UpdateParcelDeleiveredByDrone(id);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        private static void DroneToBeCharged(IBL.BL IBL)
        {
            int id = RequestID("drone");
            try
            {
                IBL.UpdateDroneToBeCharged(id);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        private static void DroneAvailable(IBL.BL IBL)
        {
            int id = RequestID("drone");
            try
            {
                IBL.UpdateDroneAvailable(id);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                Console.ReadKey();
            }
        }
        #endregion

        #region Display
        private static void DisplayStation(IBL.BL IBL)
        {
            try
            {
                int id = RequestID("station");
                Console.WriteLine(IBL.DisplayStation(id).ToString());
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private static void DisplayDrone(IBL.BL IBL)
        {
            try
            {
                int id = RequestID("drone");
                Console.WriteLine(IBL.DisplayDrone(id).ToString());
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private static void DisplayCustomer(IBL.BL IBL)
        {
            try
            {
                int id = RequestID("customer");
                Console.WriteLine(IBL.DisplayCustomer(id).ToString());
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private static void DisplayParcel(IBL.BL IBL)
        {
            try
            {
                int id = RequestID("parcel");
                Console.WriteLine(IBL.DisplayParcel(id).ToString());
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private static void DisplayDistanceFromStation(IBL.BL IBL)
        {
            try
            {
                int id = RequestID("station");
                Location location = RequestLocation();
                Console.WriteLine(IBL.DisplayDistanceFromStation(location.Longitude, location.Latitude, id));
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }

        private static void DisplayDistanceFromCustomer(IBL.BL IBL)
        {
            try
            {
                int id = RequestID("customer");
                Location location = RequestLocation();
                Console.WriteLine(IBL.DisplayDistanceFromCustomer(location.Longitude, location.Latitude, id));
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
        }
        #endregion

        #region Display_Lists
        private static void Stations(IBL.BL IBL)
        {
            List<StationToList> stations = IBL.DispalyAllStations();
            foreach (var station in stations)
                Console.WriteLine(station.ToString());
        }

        private static void Drones(IBL.BL IBL)
        {
            List<DroneToList> drones = IBL.DispalyAllDrones();
            foreach (var drone in drones)
                Console.WriteLine(drone.ToString());
        }

        private static void Customers(IBL.BL IBL)
        {

            List<CustomerToList> customers = IBL.DispalyAllCustomers();
            foreach (var customer in customers)
                Console.WriteLine(customer.ToString());
        }

        private static void Parcels(IBL.BL IBL)
        {
            List<ParcelToList> parcels = IBL.DispalyAllParcels();
            foreach (var parcel in parcels)
                Console.WriteLine(parcel.ToString());
        }

        private static void UnassignedParcels(IBL.BL IBL)
        {
            List<ParcelToList> UnassiPars = IBL.DispalyAllUnassignedParcels();
            foreach (var UnassiPar in UnassiPars)
                Console.WriteLine(UnassiPar.ToString());
        }

        private static void AvailableStations(IBL.BL IBL)
        {
            List<StationToList> AvailStats = IBL.DispalyAllAvailableStations();
            foreach (var AvailStat in AvailStats)
                Console.WriteLine(AvailStat.ToString());
        }
        #endregion

        #region Request
        private static int RequestID(string entity)
        {
            bool success;
            int StationId;
            do
            {
                Console.Write($"Please enter {entity} ID: ");                             ///Reciving ID
                success = int.TryParse(Console.ReadLine(), out StationId);
            } while (!success);
            return StationId;
        }

        private static string RequestStationName()
        {
            Console.Write("Please enter station name: ");                       ///Reciving name
            return Console.ReadLine();
        }

        private static Location RequestLocation()
        {
            bool lon, lat;
            double latitude, longitude;
            do
            {
                Console.Write("Enter latitude: ");
                lat = double.TryParse(Console.ReadLine(), out latitude);
                Console.Write("Enter longitude: ");
                lon = double.TryParse(Console.ReadLine(), out longitude);       ///Reciving location
            } while (!lat || !lon);
            Location location = new Location(latitude, longitude);
            return location;
        }
            
        private static int RequestChargeSlots()
        {
            bool success;
            int ChargeSlots;
            do
            {
                Console.Write("Enter number of charge slots: ");
                success = int.TryParse(Console.ReadLine(), out ChargeSlots);
            } while (!success);
            return ChargeSlots;
        }

        private static string RequestModelName()
        {
            Console.Write("Enter model name: ");
            return Console.ReadLine();
        }

        private static WeightCategories RequestWeightCategorie()
        {
            bool success;
            int weight;
            do
            {
                Console.WriteLine("Enter weight category:\n" +
                "0- Light \n1- Medium \n2- Heavy");
                success = int.TryParse(Console.ReadLine(), out weight);       ///Reciving weight category
            } while (!success || weight < 0 || weight > 2);
            
            return (WeightCategories)weight;
        }

        private static Priorities RequestPrioritie()
        {
            bool success;
            int priority;
            do
            {
                Console.WriteLine("Enter priority category:\n" +
                    "0- Regular\n 1- Express\n 2- Urgent");
                success = int.TryParse(Console.ReadLine(), out priority);
            } while (!success || priority < 0 || priority > 2);
                
            return (Priorities)priority;                                    ///Choosing a priority category for the parcel
        }

        private static string RequestCustomerName()
        {
            Console.Write("Please enter full name: ");                      ///Reciving name
            return Console.ReadLine();
        }

        private static string RequestPhoneNumber()
        {
            Console.Write("Please enter phone number: ");                   ///Reciving phone number
            return Console.ReadLine();
        }
        
        private static int RequestMinutesCharged()
        {
            bool success;
            int minutes;
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
