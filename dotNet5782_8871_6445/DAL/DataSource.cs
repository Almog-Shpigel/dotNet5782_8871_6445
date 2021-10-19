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
            internal static int DroneCouner = 0;
            internal static int CustomerCouner = 0;
            internal static int StationCouner = 0;
            internal static int ParcelsCouner = 0;

        }
        internal static void Initialize()
        {
            Random rnd = new Random();
            for (int i = 0; i < 2; i++)
            {
                int id = ++config.StationCouner;
                string name = "Station " + i;
                double latitude = rnd.Next(29, 34);
                double longitude = rnd.Next(31, 35);
                int chargeSlot = rnd.Next(10);
                stations[i] = new Station(id, name, chargeSlot, longitude, latitude);

            }
            for (int i = 0; i < 5; i++)
            {
                int id = ++config.DroneCouner;
                string model = "Drone " + i;
                IDAL.DO.WeightCategories weight = (IDAL.DO.WeightCategories)rnd.Next(3);
                IDAL.DO.DroneStatuses status = (IDAL.DO.DroneStatuses)rnd.Next(Enum.GetNames(typeof(IDAL.DO.DroneStatuses)).Length);
                double battery = 1000;
                drones[i] = new Drone(id, model, weight, status, battery);
            }
            for (int i = 0; i < 10; i++)
            {
                int id = ++config.CustomerCouner;
                int num = rnd.Next(10000000, 99999999);
                string phone = "05" + num;
                string name = "Customer " + i;
                double latitude = rnd.Next(29, 34);
                double longitude = rnd.Next(31, 35);
                customers[i] = new Customer(id, name, phone, longitude, latitude);

            }
            for (int i = 0; i < 10; i++)
            {
                int id = ++config.ParcelsCouner;
                int reciver = rnd.Next(10);
                int send = rnd.Next(10);
                while(send==reciver)
                    reciver = rnd.Next(10);
                IDAL.DO.WeightCategories weight = (IDAL.DO.WeightCategories)rnd.Next(3);
                IDAL.DO.Priorities priority = (IDAL.DO.Priorities)rnd.Next(3);
                DateTime Requested=DateTime.Today, Scedualed = DateTime.Today, PickedUp = DateTime.Today, Deliverd = DateTime.Today;
                int temp = rnd.Next(1, 4);
                switch(temp)
                {
                    case 1:
                        Requested = DateTime.Now;
                        break;
                    case 2:
                        Requested = DateTime.Now;
                        Scedualed = DateTime.Now.AddMinutes(20);
                        break;
                    case 3:
                        Requested = DateTime.Now;
                        Scedualed = DateTime.Now.AddMinutes(40);
                        PickedUp = DateTime.Now.AddMinutes(20);
                        break;
                    case 4:
                        Requested = DateTime.Now;
                        Scedualed = DateTime.Now.AddMinutes(40);
                        PickedUp = DateTime.Now.AddMinutes(20);
                        Deliverd = DateTime.Now.AddMinutes(40);
                        break;
                }
                int droneID = 0;
                for (int j = 0; j < 5; j++)
                {
                    if (drones[j].Status == IDAL.DO.DroneStatuses.Available)
                    {
                        drones[i].Status = IDAL.DO.DroneStatuses.Delivery;
                        droneID = drones[i].ID;
                    }
                }
                parcels[i] = new Parcel(id,send,reciver,droneID,weight,priority,Requested, Scedualed,PickedUp,Deliverd);

            }
        }
    }
}
