using ConsoleUI.IDAL.DO;
using IDAL.DO.DO;
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
        static void Initialize()
        {
            Random rnd = new Random();
            for (int i = 0; i < 2; i++)
            {
                int id = ++config.StationCouner;
                string name = "Station " + i;
                double latitude = rnd.Next(29,34);
                double longitude = rnd.Next(31,35);
                int chargeSlot = rnd.Next(10);
                stations[i] = new Station(id, name, chargeSlot, longitude, latitude);
                
            }
            for (int i = 0; i < 5; i++)
            {
                int id = ++config.DroneCouner;
                string model = "Drone " + i;
                IDAL.DO.WeightCategories weight = (IDAL.DO.WeightCategories)rnd.Next(Enum.GetNames(typeof(IDAL.DO.WeightCategories)).Length);
                IDAL.DO.DroneStatuses status = (IDAL.DO.DroneStatuses)rnd.Next(Enum.GetNames(typeof(IDAL.DO.DroneStatuses)).Length);
                double battery = 1000;
                drones[i] = new Drone(id,model,weight,status,battery);
            }
            for (int i = 0; i < 10; i++)
            {
                int id = ++config.CustomerCouner;
                int num = rnd.Next(10000000, 99999999);
                string phone = "05"+ num;
                string name = "Customer " + i;
                double latitude = rnd.Next(29, 34);
                double longitude = rnd.Next(31, 35);
                customers[i] = new Customer(id, name,phone, longitude, latitude);

            }
            //for (int i = 0; i < 10; i++)
            //{
            //    int id = ++config.ParcelsCouner;
            //    string name = "Parcel " + i;
            //    double latitude = rnd.Next(29, 34);
            //    double longitude = rnd.Next(31, 35);
            //    int chargeSlot = rnd.Next(10);
            //    stations[i] = new Station(id, name, chargeSlot, longitude, latitude);
            //    config.StationCouner++;
            //}
        }
    }
}
