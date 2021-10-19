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
            string name = "Station" + id;
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

    }
}
