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
    internal class DalObject : IDal
    {
        internal static readonly DalObject Instance = new();

        private DalObject()
        {
            DataSource.Initialize();
        }

        public static DalObject GetDalObject() { return Instance; }

        #region Add
        public void AddNewStation(Station station)
        {
            if (DataSource.stations.Any(s=>s.ID == station.ID))
                throw new StationExistException("The station ID exists already in the data!!");
            station.Name = "Station " + station.Name;
            DataSource.stations.Add(station);
        }

        public void AddNewCustomer(Customer customer)
        {
            if(DataSource.customers.Any(c => c.ID == customer.ID))
                throw new CustomerExistException("The customer ID exists already in the data!!");
            DataSource.customers.Add(customer);
        }

        public void AddNewParcel(Parcel parcel)
        {
            if (DataSource.customers.Any(c => c.ID == parcel.SenderID))
                throw new CustomerExistException($"Customer {parcel.SenderID} dosen't exists in the data!!");
            if (DataSource.customers.Any(c => c.ID == parcel.TargetID))
                throw new CustomerExistException($"Customer {parcel.TargetID} dosen't exists in the data!!");
            
            parcel.ID = 344000 + ++DataSource.Config.ParcelsCounter;
            DataSource.parcels.Add(parcel);
        }

        public void AddNewDrone(Drone drone, Station station)
        {
            Drone newDrone = DataSource.drones.Find(d => d.ID == drone.ID);
            Station newStation = DataSource.stations.Find(s => s.ID == station.ID);
            if (newDrone.ID != 0)
                throw new DroneExistException($"Drone {drone.ID} exists already in the data!!");
            if (newStation.ID == 0)
                throw new StationExistException($"Station {station.ID} doesn't exists in the data!!");
            DataSource.drones.Add(drone);
            UpdateDroneToBeCharge(drone, station, DateTime.Now);
        }

        #endregion

        #region Update
        public void UpdateDroneName(Drone newDrone)
        {
            Drone oldDrone = DataSource.drones.Find(d => d.ID == newDrone.ID);
            if (oldDrone.ID != 0)
            {
                DataSource.drones.Remove(oldDrone);
                oldDrone.Model = newDrone.Model;
                DataSource.drones.Add(oldDrone);
                DataSource.drones = DataSource.drones.OrderBy(d => d.ID).ToList();
            }
            else
                throw new DroneExistException($"Drone {newDrone.ID} doesn't exists in the data!!");
        }

        public void UpdateStationName(Station newStation)
        {
            Station oldStation = DataSource.stations.Find(s => s.ID == newStation.ID);
            if (oldStation.ID != 0)
            {
                DataSource.stations.Remove(oldStation);
                oldStation.Name = newStation.Name;
                DataSource.stations.Add(oldStation);
                DataSource.stations = DataSource.stations.OrderBy(s => s.ID).ToList();
            }
            else
                throw new StationExistException($"Station {newStation.ID} doesn't exists in the data!!");
        }

        public void UpdateStationSlots(Station newStation)
        {
            Station oldStation = DataSource.stations.Find(s => s.ID == newStation.ID);
            if (oldStation.ID != 0)
            {
                DataSource.stations.Remove(oldStation);
                oldStation.ChargeSlots = newStation.ChargeSlots;
                DataSource.stations.Add(oldStation);
                DataSource.stations = DataSource.stations.OrderBy(s => s.ID).ToList();
            }
            else
                throw new StationExistException($"Station {newStation.ID} doesn't exists in the data!!");
        }

        public void UpdateCustomerName(Customer newCustomer)
        {
            Customer oldCustomer = DataSource.customers.Find(c => c.ID == newCustomer.ID);
            if (oldCustomer.ID != 0)
            {
                DataSource.customers.Remove(oldCustomer);
                oldCustomer.Name = newCustomer.Name;
                DataSource.customers.Add(oldCustomer);
                DataSource.customers = DataSource.customers.OrderBy(c => c.ID).ToList();
            }
            else
                throw new CustomerExistException($"Customer {newCustomer.ID} doesn't exists in the data!!");
        }

        public void UpdateCustomerPhone(Customer newCustomer)
        {
            Customer oldCustomer = DataSource.customers.Find(c => c.ID == newCustomer.ID);
            if (oldCustomer.ID != 0)
            {
                DataSource.customers.Remove(oldCustomer);
                oldCustomer.Phone = newCustomer.Phone;
                DataSource.customers.Add(oldCustomer);
                DataSource.customers = DataSource.customers.OrderBy(c => c.ID).ToList();
            }
            else
                throw new CustomerExistException($"Customer {newCustomer.ID} doesn't exists in the data!!");
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

        public void UpdateDroneToBeCharge(Drone drone, Station station, DateTime? start)
        {
            if (DataSource.drones.Any(d => d.ID == drone.ID))
                throw new DroneExistException($"Drone {drone.ID} doesn't exists in the data!!");
            if (DataSource.stations.Any(s => s.ID == station.ID))
                throw new StationExistException($"Station {station.ID} doesn't exists in the data!!");
            if (DataSource.DroneCharges.Any(d => d.DroneID == drone.ID))
                throw new DroneExistException($"Drone {drone.ID} is already being charged!!");
            
            DataSource.DroneCharges.Add(new(drone.ID, station.ID, start));

            station = DataSource.stations.Find(s => s.ID == station.ID);
            DataSource.stations.Remove(station);
            --station.ChargeSlots;
            DataSource.stations.Add(station);
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
        //###################################################################
        //# Not in use at the moment.. Will be used when implement full GUI #
        //###################################################################
        //internal double DistanceFromStation(double x1, double y1, int StationID)
        //{
        //    int i = 0;
        //    double x2, y2;

        //    while (StationID != DataSource.stations[i].ID)
        //        ++i;

        //    x1 = (x1 * Math.PI) / 180;
        //    y1 = (y1 * Math.PI) / 180;
        //    x2 = DataSource.stations[i].Longitude;
        //    x2 = (x2 * Math.PI) / 180;
        //    y2 = DataSource.stations[i].Latitude;
        //    y2 = (y2 * Math.PI) / 180;
        //    double result1 = Math.Pow(Math.Sin((x2 - x1) / 2), 2) + Math.Cos(y1) * Math.Cos(y2) * Math.Pow(Math.Sin((y2 - y1) / 2), 2);
        //    double result2 = 2 * Math.Asin(Math.Sqrt(result1));
        //    double radius = 3956;
        //    return (result2 * radius);
        //}

        //internal double DistanceFromCustomer(double x1, double y1, int CustomerID)
        //{
        //    int i = 0;
        //    double x2, y2;

        //    while (CustomerID != DataSource.customers[i].ID)
        //        ++i;

        //    x1 = (x1 * Math.PI) / 180;
        //    y1 = (y1 * Math.PI) / 180;
        //    x2 = DataSource.customers[i].Longitude;
        //    x2 = (x2 * Math.PI) / 180;
        //    y2 = DataSource.customers[i].Latitude;
        //    y2 = (y2 * Math.PI) / 180;
        //    double result1 = Math.Pow(Math.Sin((x2 - x1) / 2), 2) + Math.Cos(y1) * Math.Cos(y2) * Math.Pow(Math.Sin((y2 - y1) / 2), 2);
        //    double result2 = 2 * Math.Asin(Math.Sqrt(result1));
        //    double radius = 3956;
        //    return (result2 * radius);
        //}
        #endregion



        public void PairParcelToDrone(Parcel parcel, Drone drone)
        {
            throw new NotImplementedException();
        }
    }
}
