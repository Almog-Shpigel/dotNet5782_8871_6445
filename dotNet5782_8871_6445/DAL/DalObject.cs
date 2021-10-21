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
            string longitude = Console.ReadLine();
            double Longitude = Convert.ToDouble(longitude);
            Console.WriteLine("Enter lattitude:");
            string lattitude = Console.ReadLine();
            double Lattitude = Convert.ToDouble(lattitude);
            Console.WriteLine("Enter number of charge slots:");
            string chargeSlots = Console.ReadLine();
            int ChargeSlots = Convert.ToInt32(chargeSlots);
            DataSource.stations[id] = new IDAL.DO.Station(id, name, ChargeSlots, Longitude, Lattitude);
        }

        public void AddNewDrone()
        {
            int id = ++DataSource.config.DroneCouner;
            Console.WriteLine("Enter model name: ");
            string model = Console.ReadLine() + id;
            Console.WriteLine("Enter weight category:" +
                "0- Light \n" +
                "1- Medium \n" +
                "2- Heavy \n ");
            string temp = Console.ReadLine();
            int weight = Convert.ToInt32(temp);
            WeightCategories Weight = (WeightCategories)weight;
            DataSource.drones[id] = new IDAL.DO.Drone(id, model, Weight, DroneStatuses.Available, 100);
        }

        public void PrintAllStation()
        {
            for (int i = 0; i < DataSource.config.StationCouner; i++)
            {
                DataSource.stations[i].print();
                //Console.WriteLine(DataSource.stations[i].print());
            }
            Console.ReadKey();
        }
    }
}
