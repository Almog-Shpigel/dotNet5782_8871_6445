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
          public  DalObject()
        {
            DataSource.Initialize();
        }
        public void AddNewStation()
        {
            int id = ++DataSource.config.StationCouner;
            string name = "Station " + id;
            Console.WriteLine("Enter longitude:");
            double longitude = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter lattitude:");
            double lattitude = Convert.ToDouble(Console.ReadLine());
            Console.WriteLine("Enter number of charge slots:");
            int chargeSlots = Convert.ToInt32(Console.ReadLine());
            DataSource.stations[id-1] = new IDAL.DO.Station(id, name, chargeSlots, longitude, lattitude);
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

        public void AddNewParcel()
        {
            int id = ++DataSource.config.ParcelsCouner;
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
            DataSource.parcels[id-1] = new IDAL.DO.Parcel(id, sender, target, droneID, weight, priority, TimeRequested, Scheduled, PickedUp, Delivered);
        }
        public void AddNewDrone()
        {
            int id = ++DataSource.config.DroneCouner;
            Console.WriteLine("Enter model name: ");
            string model = Console.ReadLine()+ " " + id;
            Console.WriteLine("Enter weight category:\n" +
                "0- Light\n" +
                "1- Medium\n" +
                "2- Heavy");
            string temp = Console.ReadLine();
            int weight = Convert.ToInt32(temp);
            WeightCategories Weight = (WeightCategories)weight;
            DataSource.drones[id-1] = new IDAL.DO.Drone(id, model, Weight, DroneStatuses.Available, 100);
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
