using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DalObject
    {
        public DalObject()
        {
            DataSource.Initialize();
        }
        #region Add
        public void AddNewStation()
        {
            int id = 122000 + ++DataSource.config.StationCounter;        ///Every ID number will be 6 digits starting with 12200 (changeable)
            string name = "Station " + DataSource.config.StationCounter; ///Marking the stations according to the counter, also changable
            Console.WriteLine("Enter longitude:");
            double longitude = Convert.ToDouble(Console.ReadLine());    ///Reciving location
            Console.WriteLine("Enter lattitude:");
            double lattitude = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter number of charge slots:");
            int ChargeSlots = Convert.ToInt32(Console.ReadLine());
            ///Adding new object to the array
            DataSource.stations[DataSource.config.StationCounter - 1] = new IDAL.DO.Station(id, name, ChargeSlots, longitude, lattitude);
        }
        public void AddNewCustomer()
        {
            Console.Write("Please enter your Customer ID (6 digits): ");    ///Reciving ID
            int id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Please enter your full name: ");                 ///Reciving name
            string name = Console.ReadLine();
            Console.Write("Please enter your phone number (10 digits): ");  ///Reciving phone number
            string phone = Console.ReadLine();
            Console.WriteLine("Please enter your location:");               ///Reciving location
            Console.Write("Longitude: ");
            double longitude = Convert.ToDouble(Console.ReadLine());
            Console.Write("Latitude: ");
            double lattitude = Convert.ToDouble(Console.ReadLine());
            ///Adding new object to the array
            DataSource.customers[DataSource.config.CustomerCounter++] = new IDAL.DO.Customer(id, name, phone, longitude, lattitude);
        }
        public void AddNewParcel()
        {
            int id = 344000 + ++DataSource.config.ParcelsCounter;    ///Every parcel id will begin with 34400(changeable) and will be 6 digits
            Console.Write("Please enter sender ID (6 digits): ");
            int sender = Convert.ToInt32(Console.ReadLine());
            Console.Write("Please enter receiver ID (6 digits): ");
            int target = Convert.ToInt32(Console.ReadLine());
            int DroneID = 0;
            Console.WriteLine("Enter weight category:\n" +
                "0- Light\n" +
                "1- Medium\n" +
                "2- Heavy");
            WeightCategories weight = (WeightCategories)Convert.ToInt32(Console.ReadLine()); ///Choosing a weight category for the parcel
            Console.WriteLine("Enter priority category:\n" +
               "0- Regular\n" +
               "1- Express\n" +
               "2- Urgent");
            Priorities priority = (Priorities)Convert.ToInt32(Console.ReadLine());      ///Choosing a priority category for the parcel
            ///Setting all the times to be dufault untill they will recive any updates
            DateTime TimeRequested = DateTime.Now,
                        Scheduled = DateTime.MaxValue,
                        PickedUp = DateTime.MaxValue,
                        Delivered = DateTime.MaxValue;
            ///Adding new object to the array
            DataSource.parcels[DataSource.config.ParcelsCounter - 1] = new IDAL.DO.Parcel(id, sender, target, DroneID, weight, priority, TimeRequested, Scheduled, PickedUp, Delivered);
        }
        public void AddNewDrone()
        {
            int id = 669000 + ++DataSource.config.DroneCounter;      ///Every drone id will begin with 66900(changeable) and will be 6 digits
            Console.WriteLine("Enter model name: ");
            string model = Console.ReadLine() + " " + DataSource.config.DroneCounter;
            Console.WriteLine("Enter weight category:\n" +
                "0- Light\n" +
                "1- Medium\n" +
                "2- Heavy");
            int weight = Convert.ToInt32(Console.ReadLine());       ///Reciving weight category
            WeightCategories Weight = (WeightCategories)weight;
            ///Adding new object to the array
            DataSource.drones[DataSource.config.DroneCounter - 1] = new IDAL.DO.Drone(id, model, Weight, DroneStatuses.Available, 100);
        }
        #endregion
        #region Update
        public void DroneAvailable()
        {
            Console.Write("Please enter the ID number of Drone (6 digits): ");
            int DroneID = Convert.ToInt32(Console.ReadLine());
            int j = 0;
            while (DataSource.drones[j].ID != DroneID && j <= DataSource.config.DroneCounter) ///Going through the array to find the wanted drone
                ++j;
            DataSource.drones[j].Status = DroneStatuses.Available;           ///Changing the status of the drone 
        }
        public void DroneToBeCharge()
        {
            Console.Write("Please enter the ID number of Drone (6 digits): ");
            int DroneID = Convert.ToInt32(Console.ReadLine());
            int i = 0, j = 0, station = 0;
            while (DataSource.drones[j].ID != DroneID && j <= DataSource.config.DroneCounter)    ///Finding the wanted drone
                ++j;
            Console.WriteLine("Choose a station from the list of availbale stations:");
            PrintAllAvailableStations();
            station = Convert.ToInt32(Console.ReadLine());
            DroneCharge charge = new DroneCharge(DataSource.drones[j].ID, station);
            station %= 1000;                                                                    /// Assuming we have less than 1000 stations because our ID is 122xxx
            --DataSource.stations[station].ChargeSlots;
            ///Checking to see if our drone was in the middle of a delivery
            if (DataSource.drones[j].Status == DroneStatuses.Delivery)                          ///Checking to see if our drone was in the middle of a delivery
            {
                ///If so, searching the parcel that paired with this drone so we can unassign them
                while (DataSource.parcels[i].DroneID != DroneID && i <= DataSource.config.ParcelsCounter)
                    ++i;
                DataSource.parcels[i].DroneID = 0;
            }
            DataSource.drones[j].Status = DroneStatuses.Charging;       ///Changing the drone status
        }
        public void ParcelDeleivery()
        {
            Console.Write("Please enter the ID number of Parcel (6 digits): ");
            int idNum = Convert.ToInt32(Console.ReadLine());
            int i = 0;
            while (DataSource.parcels[i].ID != idNum && i <= DataSource.config.ParcelsCounter) ///Finding the wanted parcel
                ++i;
            /// In case the ID isn't in the array
            if (i == DataSource.config.ParcelsCounter)
            {
                Console.WriteLine("ID couldn't be found. Please enter a valid ID number.");
                PairParcelToDrone();
            }
            DataSource.parcels[i].Delivered = DateTime.Now;         ///Changing the time of the parcel to update it's been delivered now

        }
        public void ParcelCollected()
        {
            Console.Write("Please enter the ID number of Parcel (6 digits): ");
            int idNum = Convert.ToInt32(Console.ReadLine());
            int i = 0, j = 0;
            while (DataSource.parcels[i].ID != idNum && i <= DataSource.config.ParcelsCounter) ///Searching for the wanted parcel
                ++i;
            /// In case the ID isn't in the array
            if (i == DataSource.config.ParcelsCounter)
            {
                Console.WriteLine("ID couldn't be found. Please enter a valid ID number.");
                PairParcelToDrone();
            }
            int DroneID = DataSource.parcels[i].DroneID;
            while (DataSource.drones[j].ID != DroneID && j <= DataSource.config.DroneCounter)
                ++j;
            DataSource.parcels[i].PickedUp = DateTime.Now; ///Updating the time of the pickup by the drone
        }
        public void PairParcelToDrone()
        {
            Console.Write("Please enter the ID number of Parcel (6 digits): ");
            int ParcelID = Convert.ToInt32(Console.ReadLine());
            int i = 0, j = 0;
            while (DataSource.parcels[i].ID != ParcelID && i <= DataSource.config.ParcelsCounter) ///Searching for the wanted parcel
                ++i;
            /// In case the ID isn't in the array
            if (i == DataSource.config.ParcelsCounter)
            {
                Console.WriteLine("ID couldn't be found. Please enter a valid ID number.");
                PairParcelToDrone();
            }
            while (j <= DataSource.config.DroneCounter &&
                    (DataSource.drones[j].Status != DroneStatuses.Available ||
                    (int)DataSource.parcels[i].Weight > (int)DataSource.drones[j].MaxWeight)) ///Searching a drone that is available and also can carry the parcel according to his max wight category
                ++j;
            if (j == DataSource.config.DroneCounter)     ///In case there is no drone available to carry this parcel
                Console.WriteLine("There are no drones available at this time.\nPlease try again later.");
            else
            {
                DataSource.parcels[i].DroneID = DataSource.drones[j].ID;    ///Pairing the parcel with the ID of the drone chose to take it
                DataSource.drones[j].Status = DroneStatuses.Delivery;       ///Changing the status of the drone
                DataSource.parcels[i].Scheduled = DateTime.Now;             ///Updating the scheduled time for the parcel
            }
        }
        #endregion
        #region Display
        public void DisplayDrone()
        {
            Console.Write("Please enter the ID number of Drone (6 digits): ");
            int DroneID = Convert.ToInt32(Console.ReadLine());
            int j = 0;
            while (DataSource.drones[j].ID != DroneID && j <= DataSource.config.DroneCounter)  ///Going through the array to find the wanted drone
                ++j;
            DataSource.drones[j].print();
        }

        public void DisplayCustomer()
        {
            Console.Write("Please enter the ID number of customer (6 digits): ");
            int CustomerID = Convert.ToInt32(Console.ReadLine());
            int j = 0;
            while (DataSource.customers[j].ID != CustomerID && j <= DataSource.config.CustomerCounter) ///Going through the array to find the wanted customer
                ++j;
            DataSource.customers[j].print();
        }

        public void DisplayParcel()
        {
            Console.Write("Please enter the ID number of parcel (6 digits): ");
            int ParcelID = Convert.ToInt32(Console.ReadLine());
            int j = 0;
            while (DataSource.parcels[j].ID != ParcelID && j <= DataSource.config.ParcelsCounter) ///Going through the array to find the wanted parcel
                ++j;
            DataSource.parcels[j].print();
        }

        public void DisplayStation()
        {
            Console.Write("Please enter the ID number of station (6 digits): ");
            int StationID = Convert.ToInt32(Console.ReadLine());
            int j = 0;
            while (DataSource.stations[j].ID != StationID && j <= DataSource.config.StationCounter) ///Going through the array to find the wanted station
                ++j;
            DataSource.stations[j].print();
        }
        #endregion
        #region Print
        public void PrintAllStations()
        {
            for (int i = 0; i < DataSource.config.StationCounter; i++)
                DataSource.stations[i].print();
        }
        public void PrintAllDrones()
        {
            for (int i = 0; i < DataSource.config.DroneCounter; i++)
                DataSource.drones[i].print();
        }
        public void PrintAllCustomers()
        {
            for (int i = 0; i < DataSource.config.CustomerCounter; i++)
                DataSource.customers[i].print();
        }
        public void PrintAllParcels()
        {
            for (int i = 0; i < DataSource.config.ParcelsCounter; i++)
                DataSource.parcels[i].print();
        }
        public void PrintAllUnassignedParcels()
        {
            for (int i = 0; i < DataSource.config.ParcelsCounter; i++)
                if (DataSource.parcels[i].DroneID == 0) ///If DroneID is 0 it means the parcel hasn't been assigned yet
                    DataSource.parcels[i].print();
        }
        public void PrintAllAvailableStations()
        {
            for (int i = 0; i < DataSource.config.StationCounter; i++)
                if(DataSource.stations[i].ChargeSlots > 0)
                DataSource.stations[i].print();
        }
        #endregion Print
    }
}
