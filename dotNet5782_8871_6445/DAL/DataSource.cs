using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DataSource
    {
        internal static List<Drone> drones = new();
        internal static List<Station> stations = new();
        internal static List<Customer> customers = new();
        internal static List<Parcel> parcels = new();
        internal static List<DroneCharge> DroneCharges = new();
        internal class Config
        {
            internal static int ParcelsCounter = 0;
            /// <summary>
            /// According to our reasearch, a drone with full battery and empty cargo can fly 30 km
            /// A drone with full battery and light cargo can fly 25 km
            /// A drone with full battery and medium cargo can fly 20 km
            /// A drone with full battery and heavy cargo can fly 15 km
            /// </summary>
            internal static double Empty = 3.3;         
            internal static double LightWight = 4;
            internal static double MediumWight = 5;
            internal static double HaevyWight = 6.6;
            internal static double ChargeRate = 60;     ///1% per minute
        }
        internal static void Initialize()
        {
            Random rnd = new();
            /// MinValue means we haven't assigned yet a drone to the parcel.
            DateTime?    Requested = DateTime.Now,
                        Scedualed = null,
                        PickedUp =null,
                        Deliverd = null;
            /// Initializing 10 stations.
            for (int i = 0; i < 10; i++)         
            {
                int id = 122000 + stations.Count;      
                string name = "Station " + stations.Count;
                double latitude = (31 + ((double)rnd.Next(7300, 8300) / 10000)); ///Jerusalem area
                double longitude = (35 + ((double)rnd.Next(1400, 2700) / 10000)); ///Jerusalem area
                int chargeSlot = rnd.Next(10);
                Station NewStation = new Station(id, name, chargeSlot, longitude, latitude);
                stations.Add(NewStation);
            }
            /// Initializing 5 drones.
            for (int i = 0; i < 5; i++)         
            {
                int id = 669000 + drones.Count();
                string model = ((DO.DroneModels)rnd.Next(3)) + " " + drones.Count;
                DO.WeightCategories weight = (DO.WeightCategories)rnd.Next(3);
                Drone NewDrone = new Drone(id, model, weight);
                drones.Add(NewDrone);
            }
            /// Initializing 10 customers.
            for (int i = 0; i < 10; i++)        
            {
                int id = rnd.Next(100000000, 1000000000);
                string phone = "05" + rnd.Next(10000000, 99999999);
                string name = ((DO.CustomerNames)rnd.Next(17)).ToString();
                double latitude = (31 + ((double)rnd.Next(7300, 8300) / 10000)); ///Jerusalem area
                double longitude = (35 + ((double)rnd.Next(1400, 2700) / 10000)); ///Jerusalem area
                Customer NewCustomer = new Customer(id, name, phone, longitude, latitude);
                customers.Add(NewCustomer);
            }
            /// Initializing 10 parcels.
            for (int i = 0; i < 10; i++)
            {
                int id = 344000 + ++Config.ParcelsCounter;
                int sender = customers[rnd.Next(10)].ID;
                int reciver = customers[rnd.Next(10)].ID;
                while (sender == reciver)
                    reciver = customers[rnd.Next(10)].ID;
                DO.WeightCategories weight = (DO.WeightCategories)rnd.Next(3);
                DO.Priorities priority = (DO.Priorities)rnd.Next(3);
                /// droneID = 0 means we haven't assigned yet a drone to the parcel.
                int droneID = 0;
                Parcel NewParcel = new Parcel(id, sender, reciver, droneID, weight, priority, Requested, Scedualed, PickedUp, Deliverd);
                parcels.Add(NewParcel);
            }
            /// Pairing parcels to drones.
            for (int i = 0; i < 5; i++)
            {
                int rand;
                Parcel NewParcel = new Parcel();
                do
                {
                    rand = rnd.Next(10);
                    NewParcel = parcels[rand];
                } while (parcels[rand].DroneID != 0 );

                /// Making sure the drones weight can carry the parcel.
                Drone drone = new(drones[i].ID, drones[i].Model, drones[i].MaxWeight);
                if (drone.MaxWeight < NewParcel.Weight)
                {
                    drone.MaxWeight = NewParcel.Weight;
                    drones[i] = drone;
                }
                //switch (rnd.Next(2, 5)) ///need to fix
                ////{
                ////    case 4: Deliverd = Requested.AddMinutes(rnd.Next(40, 60)); goto case 3;
                ////    case 3: PickedUp = Requested.AddMinutes(rnd.Next(15, 30)); goto case 2;
                ////    case 2: Scedualed = Requested.AddMinutes(rnd.Next(3, 10)); break;
                ////}
                NewParcel.DroneID = drones[i].ID;
                NewParcel.Scheduled = Scedualed;
                NewParcel.PickedUp = PickedUp;
                NewParcel.Delivered = Deliverd;
                parcels[rand] = NewParcel;
            }
        }
    }
}
