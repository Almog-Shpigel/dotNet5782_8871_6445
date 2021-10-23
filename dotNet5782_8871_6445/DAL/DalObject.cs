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
        public void AddNewStation()
        {
            int id = 122000 + ++DataSource.config.StationCouner;
            string name = "Station " + DataSource.config.StationCouner;
            Console.WriteLine("Enter longitude:");
            double longitude = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter lattitude:");
            double lattitude = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter number of charge slots:");
            int chargeSlots = Convert.ToInt32(Console.ReadLine());
            DataSource.stations[DataSource.config.StationCouner - 1] = new IDAL.DO.Station(id, name, chargeSlots, longitude, lattitude);
        }
        public void AddNewCustomer()
        {
            Console.Write("Please enter your Customer ID (6 digits): ");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.Write("Please enter your full name: ");
            string name = Console.ReadLine();
            Console.Write("Please enter your phone number (10 digits): ");
            string phone = Console.ReadLine();
            Console.WriteLine("Please enter your location:");
            Console.Write("Longitude: ");
            double longitude = Convert.ToDouble(Console.ReadLine());
            Console.Write("Latitude: ");
            double lattitude = Convert.ToDouble(Console.ReadLine());
            DataSource.customers[DataSource.config.CustomerCouner++] = new IDAL.DO.Customer(id, name, phone, longitude, lattitude);
        }
        public void DroneAvailable()
        {
            Console.Write("Please enter the ID number of Drone (6 digits): ");
            int droneID = Convert.ToInt32(Console.ReadLine());
            int j = 0;
            while (DataSource.drones[j].ID != droneID && j <= DataSource.config.DroneCouner)
                ++j;
            DataSource.drones[j].Status = DroneStatuses.Available;
        }

        public void DisplayDrone()
        {
            Console.Write("Please enter the ID number of Drone (6 digits): ");
            int droneID = Convert.ToInt32(Console.ReadLine());
            int j = 0;
            while (DataSource.drones[j].ID != droneID && j <= DataSource.config.DroneCouner)
                ++j;
            DataSource.drones[j].print();
        }

        public void DisplayCustomer()
        {
            Console.Write("Please enter the ID number of customer (6 digits): ");
            int CustomerID = Convert.ToInt32(Console.ReadLine());
            int j = 0;
            while (DataSource.customers[j].ID != CustomerID && j <= DataSource.config.CustomerCouner)
                ++j;
            DataSource.customers[j].print();
        }

        public void DisplayParcel()
        {
            Console.Write("Please enter the ID number of parcel (6 digits): ");
            int ParcelID = Convert.ToInt32(Console.ReadLine());
            int j = 0;
            while (DataSource.parcels[j].ID != ParcelID && j <= DataSource.config.ParcelsCouner)
                ++j;
            DataSource.parcels[j].print();
        }

        public void DisplayStation()
        {
            Console.Write("Please enter the ID number of station (6 digits): ");
            int StationID = Convert.ToInt32(Console.ReadLine());
            int j = 0;
            while (DataSource.stations[j].ID != StationID && j <= DataSource.config.StationCouner)
                ++j;
            DataSource.stations[j].print();
        }

        public void DroneToBeCharge()
        {
            Console.Write("Please enter the ID number of Drone (6 digits): ");
            int droneID = Convert.ToInt32(Console.ReadLine());
            int i = 0, j = 0, k = 0;
            while (DataSource.drones[j].ID != droneID && j <= DataSource.config.DroneCouner)
                ++j;
            while (DataSource.stations[k].ChargeSlots == 0 && k <= DataSource.config.StationCouner)
                ++k;
            DroneCharge charge = new DroneCharge(DataSource.drones[j].ID, DataSource.stations[k].ID);
            if (DataSource.drones[j].Status == DroneStatuses.Delivery)
            {
                while (DataSource.parcels[i].DroneID != droneID && i <= DataSource.config.ParcelsCouner)
                    ++i;
                DataSource.parcels[i].DroneID = 0;
            }
            DataSource.drones[j].Status = DroneStatuses.Charging;
        }
        public void ParcelDeleivery()
        {
            Console.Write("Please enter the ID number of Parcel (6 digits): ");
            int idNum = Convert.ToInt32(Console.ReadLine());
            int i = 0;
            while (DataSource.parcels[i].ID != idNum && i <= DataSource.config.ParcelsCouner)
                ++i;
            if (i == DataSource.config.ParcelsCouner)
            {
                Console.WriteLine("ID couldn't be found. Please enter a valid ID number.");
                PairParcelToDrone();
            }
            DataSource.parcels[i].Delivered = DateTime.Now;

        }
        public void ParcelCollected()
        {
            Console.Write("Please enter the ID number of Parcel (6 digits): ");
            int idNum = Convert.ToInt32(Console.ReadLine());
            int i = 0, j = 0;
            while (DataSource.parcels[i].ID != idNum && i <= DataSource.config.ParcelsCouner)
                ++i;
            if (i == DataSource.config.ParcelsCouner)
            {
                Console.WriteLine("ID couldn't be found. Please enter a valid ID number.");
                PairParcelToDrone();
            }
            DataSource.parcels[i].PickedUp = DateTime.Now;
            int droneID = DataSource.parcels[i].DroneID;
            while (DataSource.drones[j].ID != droneID && j <= DataSource.config.DroneCouner)
                ++j;
            DataSource.drones[j].Status = DroneStatuses.Available;
        }
        public void PairParcelToDrone()
        {
            Console.Write("Please enter the ID number of Parcel (6 digits): ");
            int idNum = Convert.ToInt32(Console.ReadLine());
            int i = 0, j = 0;
            while (DataSource.parcels[i].ID != idNum && i <= DataSource.config.ParcelsCouner)
                ++i;
            if (i == DataSource.config.ParcelsCouner)
            {
                Console.WriteLine("ID couldn't be found. Please enter a valid ID number.");
                PairParcelToDrone();
            }
            while   (j <= DataSource.config.DroneCouner && 
                    (DataSource.drones[j].Status != DroneStatuses.Available ||
                    (int)DataSource.parcels[i].Weight > (int)DataSource.drones[j].MaxWeight))
                ++j;
            if (j == DataSource.config.DroneCouner)
                Console.WriteLine("There are no drones available at this time.\nPlease try again later.");
            else
            {
                DataSource.parcels[i].DroneID = DataSource.drones[j].ID;
                DataSource.drones[j].Status = DroneStatuses.Delivery;
                DataSource.parcels[i].Scheduled = DateTime.Now;
            }
        }
        public void AddNewParcel()
        {
            int id = 344000 + ++DataSource.config.ParcelsCouner;
            Console.Write("Please enter sender ID (6 digits): ");
            int sender = Convert.ToInt32(Console.ReadLine());
            Console.Write("Please enter receiver ID (6 digits): ");
            int target = Convert.ToInt32(Console.ReadLine());
            int droneID = 0;
            Console.WriteLine("Enter weight category:\n" +
                "0- Light\n" +
                "1- Medium\n" +
                "2- Heavy");
            WeightCategories weight = (WeightCategories)Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter priority category:\n" +
               "0- Regular\n" +
               "1- Express\n" +
               "2- Urgent");
            Priorities priority = (Priorities)Convert.ToInt32(Console.ReadLine());
            DateTime    TimeRequested = DateTime.Now,
                        Scheduled = TimeRequested.AddDays(2),
                        PickedUp = DateTime.MaxValue,
                        Delivered = DateTime.MaxValue;
            DataSource.parcels[DataSource.config.ParcelsCouner - 1] = new IDAL.DO.Parcel(id, sender, target, droneID, weight, priority, TimeRequested, Scheduled, PickedUp, Delivered);
        }
        public void AddNewDrone()
        {
            int id = 669000 + ++DataSource.config.DroneCouner;
            Console.WriteLine("Enter model name: ");
            string model = Console.ReadLine()+ " " + DataSource.config.DroneCouner;
            Console.WriteLine("Enter weight category:\n" +
                "0- Light\n" +
                "1- Medium\n" +
                "2- Heavy");
            string temp = Console.ReadLine();
            int weight = Convert.ToInt32(temp);
            WeightCategories Weight = (WeightCategories)weight;
            DataSource.drones[DataSource.config.DroneCouner - 1] = new IDAL.DO.Drone(id, model, Weight, DroneStatuses.Available, 100);
        }
        public void PrintAllStation()
        {
            for (int i = 0; i < DataSource.config.StationCouner; i++)
                DataSource.stations[i].print();
        }
        public void PrintAllDrones()
        {
            for (int i = 0; i < DataSource.config.DroneCouner; i++)
                DataSource.drones[i].print();
        }
        public void PrintAllCustomers()
        {
            for (int i = 0; i < DataSource.config.CustomerCouner; i++)
                DataSource.customers[i].print();
        }
        public void PrintAllParcels()
        {
            for (int i = 0; i < DataSource.config.ParcelsCouner; i++)
                DataSource.parcels[i].print();
        }
        public void PrintAllUnassignedParcel()
        {
            for (int i = 0; i < DataSource.config.ParcelsCouner; i++)
                if (DataSource.parcels[i].DroneID == 0)
                    DataSource.parcels[i].print();
        }
        public void PrintAllAvailableStations()
        {
            for (int i = 0; i < DataSource.config.StationCouner; i++)
                if(DataSource.stations[i].ChargeSlots > 0)
                DataSource.stations[i].print();
        }
    }
}
