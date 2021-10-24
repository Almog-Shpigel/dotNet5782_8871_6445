using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DataSource
    {
        internal static Drone[] drones = new Drone[10];
        internal static Station[] stations = new Station[5];
        internal static Customer[] customers = new Customer[100];
        internal static Parcel[] parcels = new Parcel[1000];
        internal class config
        {
            internal static int DroneCounter = 0;
            internal static int CustomerCounter = 0;
            internal static int StationCounter = 0;
            internal static int ParcelsCounter = 0;
        }
        internal static void Initialize()
        {
            Random rnd = new Random();
            /// MaxValue means we haven't assigned yet a drone to the parcel.
            DateTime    Requested = DateTime.Now,
                        Scedualed = DateTime.MaxValue,
                        PickedUp = DateTime.MaxValue,
                        Deliverd = DateTime.MaxValue;
            /// Initializing 2 stations.
            for (int i = 0; i < 2; i++)         
            {
                int id = 122000 + ++config.StationCounter;      
                string name = "Station " + config.StationCounter;
                double latitude = (rnd.Next(29, 35) + ((double)rnd.Next(9999, 100000) / 100000));
                double longitude = (rnd.Next(31, 36) + ((double)rnd.Next(9999, 100000) / 100000));
                int chargeSlot = rnd.Next(10);
                stations[i] = new Station(id, name, chargeSlot, longitude, latitude);
            }
            /// Initializing 5 drones.
            for (int i = 0; i < 5; i++)         
            {
                int id = 669000 + ++config.DroneCounter;
                string model = ((IDAL.DO.DroneModels)rnd.Next(3)).ToString()+ " " + config.DroneCounter;
                IDAL.DO.WeightCategories weight = (IDAL.DO.WeightCategories)rnd.Next(3);
                IDAL.DO.DroneStatuses status = (IDAL.DO.DroneStatuses)rnd.Next(2);
                double battery = rnd.Next(4,101);           /// Battery between 4%-100% charged.
                drones[i] = new Drone(id, model, weight, status, battery);
            }
            /// Initializing 10 customers.
            for (int i = 0; i < 10; i++)        
            {
                int id = rnd.Next(99999, 1000000);
                string phone = "05" + rnd.Next(10000000, 99999999);
                string name = ((IDAL.DO.CustomerNames)rnd.Next(17)).ToString();
                double latitude = (rnd.Next(29, 35) + ((double)rnd.Next(9999, 100000) / 100000));
                double longitude = (rnd.Next(31, 36) + ((double)rnd.Next(9999, 100000) / 100000));
                customers[i] = new Customer(id, name, phone, longitude, latitude);
                ++config.CustomerCounter;
            }
            /// Initializing 10 parcels.
            for (int i = 0; i < 10; i++)        
            {
                int id = 344000 + ++config.ParcelsCounter;
                int sender = customers[rnd.Next(10)].ID;
                int reciver = customers[rnd.Next(10)].ID;
                while (sender == reciver)
                    reciver = customers[rnd.Next(10)].ID;
                IDAL.DO.WeightCategories weight = (IDAL.DO.WeightCategories)rnd.Next(3);
                IDAL.DO.Priorities priority = (IDAL.DO.Priorities)rnd.Next(3);
                /// droneID = 0 means we haven't assigned yet a drone to the parcel.
                int droneID = 0;           
                parcels[i] = new Parcel(id, sender, reciver, droneID, weight, priority, Requested, Scedualed, PickedUp, Deliverd);
            }
            /// Pairing parcels to drones.
            for (int i = 0; i < 5; i++)
            {
                switch (rnd.Next(2, 5))
                {
                    case 4: Deliverd = Requested.AddMinutes(rnd.Next(40, 60)); goto case 3; 
                    case 3: PickedUp = Requested.AddMinutes(rnd.Next(15, 30)); goto case 2;
                    case 2: Scedualed = Requested.AddMinutes(rnd.Next(3, 10)); break;
                }
                int rand;
                do
                {
                    rand = rnd.Next(10);
                } while (parcels[rand].DroneID != 0);
                if (drones[i].Status == IDAL.DO.DroneStatuses.Available)
                {
                    drones[i].Status = IDAL.DO.DroneStatuses.Delivery;
                    parcels[rand].DroneID = drones[i].ID;
                    parcels[rand].Scheduled = Scedualed;
                    parcels[rand].PickedUp = PickedUp;
                    parcels[rand].Delivered = Deliverd;
                }
            }
        }
    }
}
