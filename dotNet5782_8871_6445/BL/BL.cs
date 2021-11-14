﻿
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
//using static IBL.BO.Exceptions;

namespace IBL
{
    public class BL: IBL
    {
        private IDal Data;
        private List<DroneToList> DroneList;
        private Double[] BatteryUsed;
        private static Random rand = new Random();
        public BL()
        {
            Data = new DalObject.DalObject();
            DroneList = new List<DroneToList>();
            BatteryUsed = Data.GetBatteryUsed();

            DroneToList NewDrone = new();
            foreach (Drone drone in Data.GetAllDrones())
            {
                NewDrone.ID = drone.ID;
                NewDrone.Model = drone.Model;
                NewDrone.MaxWeight = drone.MaxWeight;
                foreach (Parcel parcel in Data.GetAllParcels())
                {
                    if (parcel.DroneID == drone.ID && parcel.Delivered == DateTime.MaxValue)
                        NewDrone = InitDroneInDelivery(NewDrone,  parcel);
                    else ///Not in delivery
                        NewDrone = InitDroneNOTinDelivery(NewDrone);
                }
                DroneList.Add(NewDrone); 
            }
        }
        private DroneToList InitDroneInDelivery(DroneToList NewDrone, Parcel parcel)
        {
            double DisDroneTarget, DisTargetStation, MinBattery;
            Random rand = new Random();
            NewDrone.Status = DroneStatus.Delivery;
            NewDrone.ParcelID = parcel.ID;
            Customer sender = Data.GetCustomer(parcel.SenderID), target = Data.GetCustomer(parcel.TargetID);
            Location NearestStat = NearestStation(target.Latitude, target.Longitude);

            if (parcel.PickedUp == DateTime.MaxValue)
                    NewDrone.CurrentLocation = NearestStation(sender.Latitude, sender.Longitude);
            else    ///means it did get pickedup
            {
                NewDrone.CurrentLocation.Latitude = sender.Latitude;
                NewDrone.CurrentLocation.Longitude = sender.Longitude;
            }
            DisDroneTarget = Distance(NewDrone.CurrentLocation.Latitude, NewDrone.CurrentLocation.Longitude, target.Latitude, target.Longitude);
            DisTargetStation = Distance(target.Latitude, target.Longitude, NearestStat.Latitude, NearestStat.Longitude);
            MinBattery = (WeightMultiplier(parcel.Weight, BatteryUsed) * DisDroneTarget) + (BatteryUsed[0] * DisTargetStation);
            NewDrone.BatteryStatus = RandBatteryStatus(MinBattery, 100);
            return NewDrone;
        }
        private DroneToList InitDroneNOTinDelivery(DroneToList NewDrone)
        {
            Random rand = new Random();
            Location nearest;
            int RandomStation, RandomCustomer;
            DroneStatus option = (DroneStatus) rand.Next(0, 2);
            List<Station> AllAvailableStations = GetAllAvailableStations();
            List<Customer> AllPastCustomers = GetPastCustomers();
            if (AllPastCustomers.Count() == 0)      ///We are assuming that the odds that there are no available stations are very unlikley
                option = DroneStatus. Charging;
            switch (option)
            {
                case DroneStatus.Available:
                    NewDrone.Status = DroneStatus.Available;
                    RandomCustomer = rand.Next(0, AllPastCustomers.Count());
                    NewDrone.CurrentLocation.Latitude = AllPastCustomers[RandomCustomer].Latitude;
                    NewDrone.CurrentLocation.Longitude = AllPastCustomers[RandomCustomer].Longitude;
                    nearest = NearestStation(NewDrone.CurrentLocation.Latitude, NewDrone.CurrentLocation.Longitude);
                    NewDrone.BatteryStatus = RandBatteryToStation(NewDrone, nearest, BatteryUsed[0]);
                    break;
                case DroneStatus.Charging:
                    NewDrone.Status = DroneStatus.Charging;
                    RandomStation = rand.Next(0, AllAvailableStations.Count());
                    NewDrone.CurrentLocation.Latitude = AllAvailableStations[RandomStation].Latitude;
                    NewDrone.CurrentLocation.Longitude = AllAvailableStations[RandomStation].Longitude;
                    NewDrone.BatteryStatus = RandBatteryStatus(0,21);
                    break;
            }
            NewDrone.Status =(DroneStatus)rand.Next(0,2);

            return NewDrone;
        }

        private double RandBatteryToStation(DroneToList drone, Location location, double battery)
        {
            battery = Distance(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, location.Latitude, location.Longitude) * battery;
            return RandBatteryStatus(battery, 100);
        }

        private double RandBatteryStatus(double min, double max)
        {
            Random rand = new Random();
            double MinBattery = 0, MaxBattery = 100, swap; 
            if (min > max) { swap = min; min = max; max = swap; }
            if (min >= MaxBattery)
                return MaxBattery;
            if (max <= MinBattery)
                return MinBattery;
            return rand.Next((int)min, (int)max) + (min + max) % 1; ;
        }

        private List<Customer> GetPastCustomers()
        {
            List<Customer> PastCustomersList = new();
            foreach (Parcel parcel in Data.GetAllParcels())
                if (parcel.Delivered !=DateTime.MaxValue)
                    PastCustomersList.Add(Data.GetCustomer(parcel.TargetID));
            return PastCustomersList;
        }

        private List<Station> GetAllAvailableStations()
        {
            List<Station> AvailableStationsList = new();
            foreach (Station station in Data.GetAllStations())
                if (station.ChargeSlots > 0)
                    AvailableStationsList.Add(station);
            return AvailableStationsList;
        }
        /// <summary>
        /// The function returns the location of the nearest available station.
        /// <para>
        /// If there is no available station, the function will <b>throw</b> an StationExistException.
        /// </para>
        /// </summary>
        /// <returns></returns>
        private Location NearestStation(double latitude, double longitude)
        {
            IEnumerable <Station> AllStation = Data.GetAllStations();
            Location NearestStation = new(AllStation.First().Latitude, AllStation.First().Longitude);
            double distance, MinDistance;
            MinDistance = Distance(latitude, longitude, NearestStation.Latitude, NearestStation.Longitude);
            foreach (Station station in AllStation)
            {
                distance = Distance(latitude, longitude, station.Latitude, station.Longitude);
                if (distance < MinDistance)
                {
                    MinDistance = distance;
                    NearestStation.Latitude = station.Latitude;
                    NearestStation.Longitude = station.Longitude;
                }
            }
            return NearestStation;           
        }

        public List<StationToList> DispalyAllStations()
        {
            List<StationToList> stations = new();
            StationToList NewStation = new ();
            foreach (Station station in Data.GetAllStations())
            {
                NewStation.ID = station.ID;
                NewStation.Name = station.Name;
                NewStation.AvailableChargeSlots = station.ChargeSlots;
                foreach (IDAL.DO.DroneCharge drone in Data.GetAllDronesCharge())
                {
                    if (drone.StationID == station.ID)
                        NewStation.UsedChargeSlots++;
                }
                stations.Add(NewStation);
            }
            return stations;
        }

        public List<DroneToList> DispalyAllDrones()
        {
            return DroneList;
        }

        public List<CustomerToList> DispalyAllCustomers()
        {
            List<CustomerToList> customers = new();
            CustomerToList NewCustomer = new();
            foreach (Customer customer in Data.GetAllCustomers())
            {
                NewCustomer.ID = customer.ID;
                NewCustomer.Name = customer.Name;
                NewCustomer.Phone = customer.Phone;
                foreach (Parcel parcel in Data.GetAllParcels())
                {
                    if (parcel.TargetID == customer.ID)
                    {
                        if (parcel.Delivered == DateTime.MaxValue)
                            NewCustomer.ParcelsOnTheWay++;
                        else
                            NewCustomer.ParcelsRecived++;
                    }
                    if (parcel.SenderID == customer.ID)
                    {
                        if (parcel.Delivered == DateTime.MaxValue)
                            NewCustomer.SentAndNOTDeliverd++;
                        else
                            NewCustomer.SentAndDeliverd++;
                    }
                }
                customers.Add(NewCustomer);
            }
            return customers;
        }

        public List<ParcelToList> DispalyAllParcels()
        {
            List<ParcelToList> parcels = new();
            ParcelToList NewParcel = new();
            foreach (Parcel parcel in Data.GetAllParcels())
            {
                NewParcel.ID = parcel.ID;
                NewParcel.Priority = parcel.Priority;
                NewParcel.TargetName = Data.GetCustomer(parcel.TargetID).Name;
                NewParcel.SenderName = Data.GetCustomer(parcel.SenderID).Name;
                NewParcel.Weight = parcel.Weight;
                if (parcel.Delivered != DateTime.MaxValue)
                    NewParcel.Status = ParcelStatus.Delivered;
                else if (parcel.PickedUp != DateTime.MaxValue)
                    NewParcel.Status = ParcelStatus.PickedUp;
                else if (parcel.Scheduled != DateTime.MaxValue)
                    NewParcel.Status = ParcelStatus.Scheduled;
                else
                    NewParcel.Status = ParcelStatus.Requested;
                parcels.Add(NewParcel);
            }
            return parcels;
        }

        public List<ParcelToList> DispalyAllUnassignedParcels()
        {
            List<ParcelToList> UnassignedParcels = new();
            ParcelToList NewParcel = new();
            foreach (Parcel parcel in Data.GetAllParcels())
            {
                if (parcel.DroneID != 0)
                {
                    NewParcel.ID = parcel.ID;
                    NewParcel.Priority = parcel.Priority;
                    NewParcel.TargetName = Data.GetCustomer(parcel.TargetID).Name;
                    NewParcel.SenderName = Data.GetCustomer(parcel.SenderID).Name;
                    NewParcel.Weight = parcel.Weight;
                    if (parcel.Delivered != DateTime.MaxValue)
                        NewParcel.Status = ParcelStatus.Delivered;
                    else if (parcel.PickedUp != DateTime.MaxValue)
                        NewParcel.Status = ParcelStatus.PickedUp;
                    else if (parcel.Scheduled != DateTime.MaxValue)
                        NewParcel.Status = ParcelStatus.Scheduled;
                    else
                        NewParcel.Status = ParcelStatus.Requested;
                    UnassignedParcels.Add(NewParcel);
                }
            }
            return UnassignedParcels;
        }

        public List<StationToList> DispalyAllAvailableStations()
        {
            List<StationToList> AvailableStations = new();
            StationToList NewStation = new();
            foreach (Station station in Data.GetAllStations())
            {
                NewStation.ID = station.ID;
                NewStation.Name = station.Name;
                NewStation.AvailableChargeSlots = station.ChargeSlots;
                foreach (IDAL.DO.DroneCharge drone in Data.GetAllDronesCharge())
                {
                    if (drone.StationID == station.ID)
                        NewStation.UsedChargeSlots++;
                }
                AvailableStations.Add(NewStation);
            }
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
            if (id < 0)
                throw new WrongInputException("ID can't be a negative number.");
            if(ChargeSlots < 0)
                throw new WrongInputException("Number of charge slots can't be a negative number.");
            StationBL NewStation = new(id, name, ChargeSlots, longitude, latitude); // all the checks will be in the cunstractur
            Data.AddNewStation(id, name, longitude, latitude, ChargeSlots);
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
            ParcelInDelivery parcel = new();
            //DroneBL NewDrone = new DroneBL(DroneId,model,weight,RandBatteryStatus(20,41),DroneStatus.Charging, parcel,)
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
