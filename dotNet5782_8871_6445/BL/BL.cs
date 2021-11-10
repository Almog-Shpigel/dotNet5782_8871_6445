
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IBL.BO;
using IDAL;
using IDAL.DO;
using static IBL.BO.EnumsBL;
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
            IEnumerable<Drone> AllDrones;
            AllDrones= Data.GetAllDrones();
            IEnumerable<Parcel> AllParcels;
            AllParcels = Data.GetAllParcels();
            DroneForList NewDrone = new();
            foreach (Drone drone in AllDrones)
            {
                
                NewDrone.ID = drone.ID;
                NewDrone.Model = drone.Model;
                NewDrone.MaxWeight = drone.MaxWeight;
                foreach (Parcel parcel in AllParcels)
                {
                    if (parcel.DroneID == drone.ID)
                    {
                        if (parcel.Delivered == DateTime.MaxValue)
                        {
                            NewDrone.Status = DroneStatus.Delivery;
                            NewDrone.ParcelID = parcel.ID;
                            Customer sender = Data.GetCustomer(parcel.SenderID);
                            if (parcel.PickedUp == DateTime.MaxValue)
                                NewDrone.CurrentLocation = ClosestsStation(sender.Latitude, sender.Longitude);
                            else    ///means it did get pickedup
                            {
                                NewDrone.CurrentLocation.Latitude = sender.Latitude;
                                NewDrone.CurrentLocation.Longitude = sender.Longitude;
                            }
                            Customer target = Data.GetCustomer(parcel.TargetID);
                            Location NearestStation = ClosestsStation(target.Latitude, target.Longitude);
                            double DisDroneTarget, DisTargetStation,MinBattery;
                            DisDroneTarget = Distance(NewDrone.CurrentLocation.Latitude, NewDrone.CurrentLocation.Longitude, target.Latitude, target.Longitude);
                            DisTargetStation = Distance(target.Latitude, target.Longitude,NearestStation.Latitude,NearestStation.Longitude);
                            MinBattery =(WeightMultiplier(parcel.Weight, BatteryUsed) * DisDroneTarget) + (BatteryUsed[0] * DisTargetStation);
                            Random rand = new Random();
                            NewDrone.Battery = rand.Next((int)MinBattery, 100)+rand.NextDouble();
                            //Needs to handle case of drone not in delivery, and at the end to send the new drone to the list
                        }

                    }

                }
                
            }

        }

        private Location ClosestsStation(double latitude, double longitude)
        {
            IEnumerable < Station > AllStation = Data.GetAllStations();
            double distance=0;
            Location NearestStation=new(AllStation.First().Latitude,AllStation.First().Longitude);
            double min = Distance(latitude, longitude, NearestStation.Latitude, NearestStation.Longitude);
            foreach (Station station in AllStation)
            {
                distance = Distance(latitude, longitude, station.Latitude, station.Longitude);
                if (distance<min)
                {
                    min = distance;
                    NearestStation.Latitude = station.Latitude;
                    NearestStation.Longitude = station.Longitude;
                }
            }
            return NearestStation;           
        }

        public List<string> DispalyAllStations()
        {
            List<string> stations = new();
            foreach (Station station in Data.GetAllStations())
                stations.Add(station.ToString());
            return stations;
        }

        public List<string> DispalyAllDrones()
        {
            List<string> drones = new();
            foreach (Drone drone in Data.GetAllDrones())
                drones.Add(drone.ToString());
            return drones;
        }

        public List<string> DispalyAllCustomers()
        {
            List<string> customers = new();
            foreach (Customer customer in Data.GetAllCustomers())
                customers.Add(customer.ToString());
            return customers;
        }

        public List<string> DispalyAllParcels()
        {
            List<string> parcels = new();
            foreach (Parcel parcel in Data.GetAllParcels())
                parcels.Add(parcel.ToString());
            return parcels;
        }

        public List<string> DispalyAllUnassignedParcels()
        {
            IEnumerable<Parcel> AllParecels = Data.GetAllParcels();
            List<string> UnassignedParcels = new();
            foreach (Parcel parcel in AllParecels)
                if (parcel.DroneID == 0)
                    UnassignedParcels.Add(parcel.ToString());

            return UnassignedParcels;
        }

        public List<string> DispalyAllAvailableStations()
        {
            IEnumerable<Station> AllStations = Data.GetAllStations();
            List<string> AvailableStations = new();

            foreach (Station station in AllStations)
                if (station.ChargeSlots > 0)
                    AvailableStations.Add(station.ToString());
            return AvailableStations;
        }

        public string DisplayStation(int StationID)
        {
            foreach (Station station in Data.GetAllStations())
                if (station.ID == StationID)
                    return station.ToString();
            throw new StationExistException();
        }

        public string DisplayDrone(int DroneID)
        {
            foreach (Drone drone in Data.GetAllDrones())
                if (drone.ID == DroneID)
                    return drone.ToString();
            throw new DroneExistException();
        }

        public string DisplayCustomer(int CustomerID)
        {
            foreach (Customer customer in Data.GetAllCustomers())
                if (customer.ID == CustomerID)
                    return customer.ToString();
            throw new CustomerExistException();
        }

        public string DisplayParcel(int ParcelID)
        {
            foreach (Parcel parcel in Data.GetAllParcels())
                if (parcel.ID == ParcelID)
                    return parcel.ToString();
            throw new ParcelExistException();
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
        private double WeightMultiplier(WeightCategories weight,Double [] BatteryUse)
        {
            switch((int)weight)
            {
                case 0:
                    return BatteryUse[1]; //Light
                case 1:
                    return BatteryUse[2]; //Medium
                case 2:
                    return BatteryUse[3]; //Heavy
            }
             return BatteryUse[0]; //Empty
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
