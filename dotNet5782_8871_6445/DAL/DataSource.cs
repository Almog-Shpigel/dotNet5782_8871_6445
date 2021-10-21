﻿using IDAL.DO;
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
            // MaxValue means we haven't assigned yet a drone to the parcel.
            DateTime    Requested = DateTime.Now,
                        Scedualed = DateTime.MaxValue,
                        PickedUp = DateTime.MaxValue,
                        Deliverd = DateTime.MaxValue;
            for (int i = 0; i < 2; i++)         // Initializing 2 stations.
            {
                int id = rnd.Next(99999, 1000000);
                string name = "Station " + ++config.StationCouner;
                double latitude = rnd.Next(29, 34);
                double longitude = rnd.Next(31, 35);
                int chargeSlot = rnd.Next(10);
                stations[i] = new Station(id, name, chargeSlot, longitude, latitude);
            }
            for (int i = 0; i < 5; i++)         // Initializing 5 drones.
            {
                int id = ++config.DroneCouner;
                string model = ((IDAL.DO.DroneModels)rnd.Next(3)).ToString()+ " " + config.DroneCouner;
                IDAL.DO.WeightCategories weight = (IDAL.DO.WeightCategories)rnd.Next(3);
                IDAL.DO.DroneStatuses status = (IDAL.DO.DroneStatuses)rnd.Next(3);
                double battery = rnd.Next(4,101);           // Battery between 4%-100% charged.
                drones[i] = new Drone(id, model, weight, status, battery);
            }
            for (int i = 0; i < 10; i++)        // Initializing 10 customers.
            {
                int id = ++config.CustomerCouner;
                string phone = "05" + rnd.Next(10000000, 99999999);
                string name = ((IDAL.DO.CustomerNames)rnd.Next(17)).ToString();
                double latitude = (rnd.Next(29, 35) + ((double)rnd.Next(9999, 100000) / 100000));
                double longitude = (rnd.Next(31, 36) + ((double)rnd.Next(9999, 100000) / 100000));
                customers[i] = new Customer(id, name, phone, longitude, latitude);
            }
            for (int i = 0; i < 10; i++)        // Initializing 10 parcels.
            {
                int id = ++config.ParcelsCouner;
                int sender = customers[rnd.Next(10)].ID;
                int reciver = customers[rnd.Next(10)].ID;
                while (sender == reciver)
                    reciver = customers[rnd.Next(10)].ID;
                IDAL.DO.WeightCategories weight = (IDAL.DO.WeightCategories)rnd.Next(3);
                IDAL.DO.Priorities priority = (IDAL.DO.Priorities)rnd.Next(3);
                // droneID = 0 means we haven't assigned yet a drone to the parcel.
                int droneID = 0;           
                parcels[i] = new Parcel(id, sender, reciver, droneID, weight, priority, Requested, Scedualed, PickedUp, Deliverd);
            }
            for (int i = 0; i < 5; i++)
            {
                switch (rnd.Next(2, 5))
                {
                    case 2:
                        Scedualed = Requested.AddDays(2);
                        break;
                    case 3:
                        Scedualed = Requested.AddDays(2);
                        PickedUp = Requested.AddDays(12);
                        break;
                    case 4:
                        Scedualed = Requested.AddDays(2);
                        PickedUp = Requested.AddDays(12);
                        Deliverd = Requested.AddDays(14);
                        break;
                }
                int rand1, rand2;
                do
                {
                    rand1 = rnd.Next(5);
                } while (drones[rand1].Status != IDAL.DO.DroneStatuses.Available);
                do
                {
                    rand2 = rnd.Next(10);
                } while (parcels[rand2].DroneID != 0);
                drones[rand1].Status = IDAL.DO.DroneStatuses.Delivery;
                parcels[rand2].DroneID = drones[rand1].ID;
                parcels[rand2].Scheduled = Scedualed;
                parcels[rand2].PickedUp = PickedUp;
                parcels[rand2].Delivered = Deliverd;
            }
        }
    }
}
