
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IBL.BO;
using IDAL;
using static IBL.BO.Enums;
using static IBL.BO.Exceptions;

namespace IBL
{
    public class BL: IBL
    {
        IDal Data = new DalObject.DalObject();
        List<DroneForList> DroneList = new();
        public BL()
        {
            Double[] BatteryUsed = Data.GetBatteryUsed();
            IEnumerable<IDAL.DO.Drone> AllDrones;
            AllDrones= Data.GetAllDrones();

        }

        public IEnumerable<string> DispalyAllStations()
        {
            List<string> stations = new();
            foreach (IDAL.DO.Station station in Data.GetAllStations())
                stations.Add(station.ToString());
            return stations;
        }

        public List<string> DispalyAllDrones()
        {
            List<string> drones = new();
            foreach (IDAL.DO.Drone drone in Data.GetAllDrones())
                drones.Add(drone.ToString());
            return drones;
        }

        public IEnumerable<string> DispalyAllCustomers()
        {
            List<string> customers = new();
            foreach (IDAL.DO.Customer customer in Data.GetAllCustomers())
                customers.Add(customer.ToString());
            return customers;
        }

        public IEnumerable<string> DispalyAllParcels()
        {
            List<string> parcels = new();
            foreach (IDAL.DO.Parcel parcel in Data.GetAllParcels())
                parcels.Add(parcel.ToString());
            return parcels;
        }

        public IEnumerable<string> DispalyAllUnassignedParcels()
        {
            IEnumerable<IDAL.DO.Parcel> AllParecels = Data.GetAllParcels();
            AllParecels.Select(x => x.ID == 0);
            List<string> parcels = new();
            foreach (IDAL.DO.Parcel parcel in AllParecels)
                parcels.Add(parcel.ToString());
            return parcels;
        }

        public IEnumerable<string> DispalyAllAvailableStations()
        {
            IEnumerable<IDAL.DO.Station> AllStations = Data.GetAllStations();
            AllStations.Select(x => x.ChargeSlots > 0);
            List<string> stations = new();
            foreach (IDAL.DO.Station station in AllStations)
                stations.Add(station.ToString());
            return stations;
        }

        public string DisplayStation(int StationID)
        {
            foreach (IDAL.DO.Station station in Data.GetAllStations())
                if (station.ID == StationID)
                    return station.ToString();
            throw new StationExistException();
        }

        public string DisplayDrone(int DroneID)
        {
            foreach (IDAL.DO.Drone drone in Data.GetAllDrones())
                if (drone.ID == DroneID)
                    return drone.ToString();
            throw new DroneExistException();
        }

        public string DisplayCustomer(int CustomerID)
        {
            foreach (IDAL.DO.Customer customer in Data.GetAllCustomers())
                if (customer.ID == CustomerID)
                    return customer.ToString();
            throw new CustomerExistException();
        }

        public string DisplayParcel(int ParcelID)
        {
            foreach (IDAL.DO.Parcel parcel in Data.GetAllParcels())
                if (parcel.ID == ParcelID)
                    return parcel.ToString();
            throw new ParcelExistException();
        }
        public string DisplayDistanceFromStation(double longitude1, double latitude1, int StationID)
        {
            double longitude2 = 0, latitude2 = 0;
            IEnumerable<IDAL.DO.Station> stations = Data.GetAllStations();
            foreach (IDAL.DO.Station station in stations)
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
            IEnumerable<IDAL.DO.Customer> customers = Data.GetAllCustomers();
            foreach (IDAL.DO.Customer customer in customers)
                if (customer.ID == CustomerID)
                {
                    longitude2 = customer.Longitude;
                    latitude2 = customer.Latitude;
                }

            return "The distance is: " + Distance(longitude1, latitude1, longitude2, latitude2) + " km";
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

        internal void AddNewDrone(int DroneId, string model, WeightCategories weight, int StationId)
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

        internal void AddNewParcel(int sender, int receiver, WeightCategories weight, Priorities prioritie)
        {
            //ב-BL כל הזמנים יאותחלו לזמן אפס למעט תאריך יצירה שיאותחל ל-DateTime.Now
            //הרחפן יאותחל ל-null
        }
    }
}
