
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IDAL;
using IDAL.DO;

namespace IBL
{
    public class BL: IBL
    {
        public BL()
        {
            IDal Data = new DalObject.DalObject();
            Double[] BatteryUsed = Data.GetBatteryUsed();

        }
        public void UpdateDroneName(int v1, string v2)
        {
            throw new NotImplementedException();
        }

        public void UpdateStation(int v1, string v2, int v3)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomer(int v1, string v2, string v3)
        {
            throw new NotImplementedException();
        }

        public void UpdateParcelToDrone(int v)
        {
            throw new NotImplementedException();
        }

        public string DisplayDistanceFromStation(double v1, double v2, int v3)
        {
            throw new NotImplementedException();
        }

        public void UpdateParcelCollectedByDrone(int v)
        {
            throw new NotImplementedException();
        }

        public void AddNewStation(int id, string name, double longitude, double latitude, int ChargeSlots)
        {
            //רשימת הרחפנים בטעינה תאותחל לרשימה ריקה
        }

        public string DisplayDistanceFromCustomer(double longitude1, double latitude1, int CustomerID)
        {
            double longitude2, latitude2;
            
            return "The distance is: " + Distance(longitude1, latitude1, longitude2, latitude2) + " km";
        }

        private double Distance(double x1, double y1, double x2, double y2)
        {
            x1 = (x1 * Math.PI) / 180;
            y1 = (y1 * Math.PI) / 180;
            x2 = (x2 * Math.PI) / 180;
            y2 = (y2 * Math.PI) / 180;
            double result1 = Math.Pow(Math.Sin((x2 - x1) / 2), 2) + Math.Cos(y1) * Math.Cos(y2) * Math.Pow(Math.Sin((y2 - y1) / 2), 2);
            double result2 = 2 * Math.Asin(Math.Sqrt(result1));
            double radius = 3956;
            return (result2 * radius);
        }

        public void UpdateParcelDeleiveredByDrone(Func<int> droneID)
        {
            throw new NotImplementedException();
        }

        public void AddNewStation()
        {
            throw new NotImplementedException();
        }

        public void UpdateDroneToBeCharged(int v)
        {
            throw new NotImplementedException();
        }

        public void AddNewDrone(int DroneId, string model, WeightCategories weight, int StationId)
        {
            //מצב סוללה יוגרל בין %20 ל-40%
            //יוסף כנמצא בתחזוקה
            //מיקום הרחפן יהיה כמיקום התחנה
        }

        public void UpdateDroneAvailable(int v1, int v2)
        {
            throw new NotImplementedException();
        }

        public List<string> PrintAllAvailableStations()
        {
            throw new NotImplementedException();
        }

        public void AddNewCustomer(int id, string name, string phone, double longitude, double latitude)
        {
            throw new NotImplementedException();
        }

        public void AddNewParcel(int sender, int receiver, WeightCategories weight, Priorities prioritie)
        {
            //ב-BL כל הזמנים יאותחלו לזמן אפס למעט תאריך יצירה שיאותחל ל-DateTime.Now
            //הרחפן יאותחל ל-null
        }
    }
}
