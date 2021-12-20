using DalApi;
using DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    internal class DalObject : DalApi.IDal
    {
        internal static readonly DalObject Instance = new();

        private DalObject()
        {
            DataSource.Initialize();
        }

        public static DalObject GetDalObject() { return Instance; }

        private bool DroneIsCharging(int DroneID)
        {
            foreach (DroneCharge drone in GetDroneCharge(droneCharge => true))
                if (drone.DroneID == DroneID)
                    return true;
            return false;
        }

        #region Add
        public void AddNewStation(Station station)
        {
            if (StationExist(station.ID))
                throw new StationExistException("The station ID exists already in the data!!");
            station.Name = "Station " + station.Name;
            DataSource.stations.Add(station);
        }

        public void AddNewCustomer(Customer customer)
        {
            if(CustomerExist(customer.ID))
                throw new CustomerExistException("The customer ID exists already in the data!!");
            DataSource.customers.Add(customer);
        }

        public void AddNewParcel(Parcel parcel)
        {
            if (!CustomerExist(parcel.SenderID))
                throw new CustomerExistException("The sender ID dosen't exists in the data!!");
            if (!CustomerExist(parcel.TargetID))
                throw new CustomerExistException("The target ID dosen't exists in the data!!");
            parcel.ID = 344000 + ++DataSource.Config.ParcelsCounter;
            DataSource.parcels.Add(parcel);
        }

        public void AddNewDrone(Drone drone, int StationID)
        {
            if (DroneExist(drone.ID))
                throw new DroneExistException("The drone ID exists already in the data!!");
            if (!StationExist(StationID))
                throw new StationExistException("The station doesn't exists in the data!!");
            DataSource.drones.Add(drone);
            UpdateDroneToBeCharge(drone.ID, StationID, DateTime.Now);
        }
        #endregion

        #region Update
        public void UpdateDroneName(int DroneID, string model)
        {
            Drone drone = GetDrone(DroneID);
            drone.Model = model;
            for (int i = 0; i < DataSource.drones.Count; i++)
            {
                if (DataSource.drones[i].ID == drone.ID)
                {
                    DataSource.drones[i] = drone;
                    break;
                }
            }
        }

        public void UpdateStationName(int stationID, string name)
        {
            Station station = GetStation(stationID);
            station.Name = name;
            for (int i = 0; i < DataSource.stations.Count; i++)
            {
                if (DataSource.stations[i].ID == station.ID)
                    DataSource.stations[i] = station;
            }
        }

        public void UpdateCustomerName(int CustomerID, string name)
        {
            Customer customer = GetCustomer(CustomerID);
            customer.Name = name;
            for (int i = 0; i < DataSource.customers.Count; i++)
            {
                if (DataSource.customers[i].ID == customer.ID)
                    DataSource.customers[i] = customer;
            }
        }

        public void UpdateCustomerPhone(int CustomerID, int phone)
        {
            Customer customer = GetCustomer(CustomerID);
            customer.Phone = '0' + phone.ToString();
            for (int i = 0; i < DataSource.customers.Count; i++)
            {
                if (DataSource.customers[i].ID == customer.ID)
                    DataSource.customers[i] = customer;
            }
        }

        public void UpdateStationSlots(int stationID, int slots)
        {
            Station station = GetStation(stationID);
            station.ChargeSlots = slots;
            for (int i = 0; i < DataSource.stations.Count; i++)
            {
                if (DataSource.stations[i].ID == station.ID)
                    DataSource.stations[i] = station;
            }
        }

        public void UpdateDroneToBeAvailable(int DroneID)
        {
            if (!DroneExist(DroneID))
                throw new DroneExistException("The drone dosen't exists in the data!!");

            DroneCharge droneCharge = new();
            for (int j = 0; j < DataSource.DroneCharges.Count; ++j)         ///Going through the array to find the wanted DroneCharged object
                if (DataSource.DroneCharges[j].DroneID == DroneID)
                {
                    droneCharge = DataSource.DroneCharges[j];
                    break;
                }

            Station NewStation;
            DataSource.DroneCharges.Remove(droneCharge);
            for (int i = 0; i < DataSource.stations.Count; ++i)             ///Going through the array to find the wanted station the drone was charged in
            {

                if (DataSource.stations[i].ID == droneCharge.StationID)
                {
                    NewStation = DataSource.stations[i];
                    NewStation.ChargeSlots++;
                    DataSource.stations[i] = NewStation;                    ///Freeing a space for other drones
                    break;
                }
            }
        }

        public void UpdateDroneToBeCharge(int DroneID, int StationID, DateTime? start)
        {
            if (!DroneExist(DroneID))
                throw new DroneExistException("The drone dosen't exists in the data!!");

            if (!StationExist(StationID))
                throw new StationExistException("The station dosen't exists in the data!!");

            if (DroneIsCharging(DroneID))
                throw new DroneExistException("The drone is already being charged!!");

            
            DroneCharge NewDroneToBeCharge = new(DroneID, StationID, start);
            DataSource.DroneCharges.Add(NewDroneToBeCharge);

            for (int i = 0; i < DataSource.stations.Count; ++i)         ///Finding the wanted station
            {
                if (DataSource.stations[i].ID == StationID)
                {
                    Station NewStation = DataSource.stations[i];
                    NewStation.ChargeSlots--;
                    DataSource.stations[i] = NewStation;                ///one slot was taken by the drone we chose
                }
            }
        }

        public void UpdateParcelInDelivery(int ParcelID)
        {
            if (!ParcelExist(ParcelID))
                throw new DroneExistException("The parcel dosen't exists in the data!!");
            int i = 0;
            while (DataSource.parcels[i].ID != ParcelID) ///Finding the wanted parcel
                ++i;
            Parcel NewParcel = DataSource.parcels[i];
            NewParcel.Delivered = DateTime.Now; ///Changing the time of the parcel to update it's been delivered now
            DataSource.parcels[i] = NewParcel;
        }

        public void UpdateParcelCollected(int ParcelID)
        {
            if (!ParcelExist(ParcelID))
                throw new DroneExistException("The parcel dosen't exists in the data!!");

            int i = 0;
            while (DataSource.parcels[i].ID != ParcelID) ///Searching for the wanted parcel
                ++i;
            Parcel NewParcel = DataSource.parcels[i];
            NewParcel.PickedUp = DateTime.Now;
            DataSource.parcels[i] = NewParcel; ///Updating the time of the pickup by the drone
        }

        public void PairParcelToDrone(int ParcelID,int DroneID)
        {
            if (!ParcelExist(ParcelID))
                throw new DroneExistException("The parcel dosen't exists in the data!!");

            if (!DroneExist(DroneID))
                throw new DroneExistException("The drone dosen't exists in the data!!");

            int i = 0;
            while (DataSource.parcels[i].ID != ParcelID) ///Searching for the wanted parcel
                ++i;
            Parcel NewParcel = DataSource.parcels[i];
            NewParcel.DroneID = DroneID;                ///Pairing the parcel with the ID of the drone chose to take it
            NewParcel.Scheduled = DateTime.Now;         ///Updating the scheduled time for the parcel
            DataSource.parcels[i] = NewParcel;
        }
        #endregion

        #region Get
        public double[] GetBatteryUsed()
        {
            double[] BatteryUsed = new double[5];
            BatteryUsed[0] = DataSource.Config.Empty;
            BatteryUsed[1] = DataSource.Config.LightWight;
            BatteryUsed[2] = DataSource.Config.MediumWight;
            BatteryUsed[3] = DataSource.Config.HaevyWight;
            BatteryUsed[4] = DataSource.Config.ChargeRate;
            return BatteryUsed;
        }

        public Drone GetDrone(int DroneID)
        {
            foreach (Drone drone in DataSource.drones)
            {
                if (drone.ID == DroneID)
                    return drone;
            }
            throw new DroneExistException("The drone dosen't exists in the data!!");
        }

        public Parcel GetParcel(int ParcelID)
        {
            foreach (Parcel parcel in DataSource.parcels)
            {
                if (parcel.ID == ParcelID)
                    return parcel;
            }
            throw new ParcelExistException("Parcel not exist!");
        }

        public Station GetStation(int StationID)
        {
            foreach (Station station in DataSource.stations)
            {
                if (station.ID == StationID)
                    return station;
            }
            throw new StationExistException("The station dosen't exists in the data!!");

        }

        public Customer GetCustomer(int CustomerID)
        {
            foreach (Customer customer in DataSource.customers)
            {
                if (customer.ID == CustomerID)
                    return customer;
            }
            throw new CustomerExistException("The customer doesn't exists in the data!!");
        }

        public DroneCharge GetDroneCharge(int DroneChargeID)
        {
            foreach (DroneCharge drone in DataSource.DroneCharges)
            {
                if (drone.DroneID == DroneChargeID)
                    return drone;
            }
            throw new DroneExistException("Drone in charge not exist!");
        }

        public IEnumerable<Drone> GetDrones(Predicate<Drone> DronePredicate)
        {
            IEnumerable<Drone> SelectedDrones = DataSource.drones.Where(drone => DronePredicate(drone));
            return SelectedDrones;
        }

        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> ParcelPredicate)
        {
            IEnumerable<Parcel> SelectedParcels = DataSource.parcels.Where(parcel => ParcelPredicate(parcel));
            return SelectedParcels;
        }

        public IEnumerable<Station> GetStations(Predicate<Station> StationPredicate)
        {
            IEnumerable<Station> SelectedStations = DataSource.stations.Where(station => StationPredicate(station));
            return SelectedStations;
        }

        public IEnumerable<Customer> GetCustomers(Predicate<Customer> CustomerPredicate)
        {
            IEnumerable<Customer> SelectedCustomers = DataSource.customers.Where(customer => CustomerPredicate(customer));
            return SelectedCustomers;
        }

        public IEnumerable<DroneCharge> GetDroneCharge(Predicate<DroneCharge> DroneChargePredicate)
        {
            IEnumerable<DroneCharge> SelectedDroneCharge = DataSource.DroneCharges.Where(droneCharge => DroneChargePredicate(droneCharge));
            return SelectedDroneCharge;
        }
        #endregion

        #region Calc
        //Not in use at the moment.. Will be used when implement full GUI
        internal double DistanceFromStation(double x1, double y1, int StationID)
        {
            int i = 0;
            double x2, y2;

            while (StationID != DataSource.stations[i].ID)
                ++i;

            x1 = (x1 * Math.PI) / 180;
            y1 = (y1 * Math.PI) / 180;
            x2 = DataSource.stations[i].Longitude;
            x2 = (x2 * Math.PI) / 180;
            y2 = DataSource.stations[i].Latitude;
            y2 = (y2 * Math.PI) / 180;
            double result1 = Math.Pow(Math.Sin((x2 - x1) / 2), 2) + Math.Cos(y1) * Math.Cos(y2) * Math.Pow(Math.Sin((y2 - y1) / 2), 2);
            double result2 = 2 * Math.Asin(Math.Sqrt(result1));
            double radius = 3956;
            return (result2 * radius);
        }

        internal double DistanceFromCustomer(double x1, double y1, int CustomerID)
        {
            int i = 0;
            double x2, y2;

            while (CustomerID != DataSource.customers[i].ID)
                ++i;

            x1 = (x1 * Math.PI) / 180;
            y1 = (y1 * Math.PI) / 180;
            x2 = DataSource.customers[i].Longitude;
            x2 = (x2 * Math.PI) / 180;
            y2 = DataSource.customers[i].Latitude;
            y2 = (y2 * Math.PI) / 180;
            double result1 = Math.Pow(Math.Sin((x2 - x1) / 2), 2) + Math.Cos(y1) * Math.Cos(y2) * Math.Pow(Math.Sin((y2 - y1) / 2), 2);
            double result2 = 2 * Math.Asin(Math.Sqrt(result1));
            double radius = 3956;
            return (result2 * radius);
        }
        #endregion

        #region Exist
        internal bool DroneExist(int id)
        {
            foreach (Drone drone in DataSource.drones)
                if (drone.ID == id)
                    return true;
            return false;
        }

        internal bool StationExist(int id)
        {
            foreach (Station station in DataSource.stations)
                if (station.ID == id)
                    return true;
            return false;
        }

        internal bool CustomerExist(int id)
        {
            foreach (Customer customer in DataSource.customers)
                if (customer.ID == id)
                    return true;
            return false;
        }

        internal bool ParcelExist(int id)
        {
            foreach (Parcel parcel in DataSource.parcels)
                if (parcel.ID == id)
                    return true;
            return false;
        }
        #endregion
    }
}
