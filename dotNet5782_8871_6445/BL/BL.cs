
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IDAL;
using IDAL.DO;
using static IBL.BO.Exceptions;

namespace IBL
{
    public class BL: IBL
    {
        IDal Data = new DalObject.DalObject();

        public IEnumerable<string> DispalyAllStations()
        {
            return (IEnumerable<string>)Data.GetAllStations();
        }

        public IEnumerable<string> DispalyAllDrones()
        {
            return (IEnumerable<string>)Data.GetAllDrones();
        }

        public IEnumerable<string> DispalyAllCustomers()
        {
            return (IEnumerable<string>)Data.GetAllCustomers();
        }

        public IEnumerable<string> DispalyAllParcels()
        {
            return (IEnumerable<string>)Data.GetAllParcels();
        }

        public IEnumerable<string> DispalyAllUnassignedParcels()
        {
            IEnumerable<Parcel> AllParecels = Data.GetAllParcels();
            AllParecels.Select(x => x.ID == 0);
            return (IEnumerable<string>)AllParecels;
            //List<string> UnassignedParcels = new();
            //foreach (Parcel parcel in AllParecels)
            //    if (parcel.DroneID == 0)
            //        UnassignedParcels.Add(parcel.ToString());
            //return UnassignedParcels;
        }

        public IEnumerable<string> DispalyAllAvailableStations()
        {
            IEnumerable<Station> AllStations = Data.GetAllStations();
            List<string> AvailableStations = new();
            foreach (Station station in AllStations)
                if (station.ChargeSlots > 0)
                    AvailableStations.Add(station.ToString());
            return AvailableStations;
        }

        public BL()
        {
            Double[] BatteryUsed = Data.GetBatteryUsed();
        }

        public string DisplayStation(int StationID)
        {
            IEnumerable<Station> AllStations = Data.GetAllStations();
            foreach (Station station in AllStations)
                if (station.ID == StationID)
                    return station.ToString();
            throw new StationExistException();
        }

        public bool DisplayDrone(int v)
        {
            throw new NotImplementedException();
        }

        public bool DisplayCustomer(int v)
        {
            throw new NotImplementedException();
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

        public string DisplayParcel(int v)
        {
            throw new NotImplementedException();
        }

        public void UpdateParcelToDrone(int v)
        {
            throw new NotImplementedException();
        }

        public string DisplayDistanceFromStation(double longitude1, double latitude1, int StationID)
        {
            double longitude2 = 0, latitude2 = 0;
            IEnumerable<Station> stations = Data.GetAllStations();
            foreach (Station station in stations)
                if (station.ID == StationID)
                {
                    longitude2 = station.Longitude;
                    latitude2 = station.Latitude;
                }

            return "The distance is: " + Distance(longitude1, latitude1, longitude2, latitude2) + " km";
        }

        public string DisplayDistanceFromCustomer(double longitude1, double latitude1, int CustomerID)
        {
            double longitude2 = 0, latitude2 = 0;
            IEnumerable<Customer> customers = Data.GetAllCustomers();
            foreach (Customer customer in customers)
                if (customer.ID == CustomerID)
                {
                    longitude2 = customer.Longitude;
                    latitude2 = customer.Latitude;
                }

            return "The distance is: " + Distance(longitude1, latitude1, longitude2, latitude2) + " km";
        }

        public void UpdateParcelCollectedByDrone(int v)
        {
            throw new NotImplementedException();
        }

        public void AddNewStation(int id, string name, double longitude, double latitude, int ChargeSlots)
        {
            //רשימת הרחפנים בטעינה תאותחל לרשימה ריקה
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
