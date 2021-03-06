using DAL;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;


namespace DalObject
{
    internal class DalObject : IDal
    {
        internal static readonly DalObject instance = new();

        private DalObject()
        {
            DataSource.Initialize();

        }

        public static DalObject Instance { get => instance; }
        #region Add
        [MethodImpl(MethodImplOptions.Synchronized)]

        public void AddNewStation(Station station)
        {
            if (DataSource.stations.Any(s=>s.ID == station.ID))
                throw new StationExistException("The station ID exists already in the data!!");
            station.Name = "Station " + station.Name;
            DataSource.stations.Add(station);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]

        public void AddNewCustomer(Customer customer)
        {
            if(DataSource.customers.Any(c => c.ID == customer.ID))
                throw new CustomerExistException("The customer ID exists already in the data!!");
            DataSource.customers.Add(customer);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]

        public void AddNewParcel(Parcel parcel)
        {
            if (!DataSource.customers.Any(c => c.ID == parcel.SenderID))
                throw new CustomerExistException($"Customer {parcel.SenderID} doesn't exists in the data!!");
            if (!DataSource.customers.Any(c => c.ID == parcel.TargetID))
                throw new CustomerExistException($"Customer {parcel.TargetID} doesn't exists in the data!!");            
            parcel.ID = 344000 + ++DataSource.Config.ParcelsCounter;
            DataSource.parcels.Add(parcel);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]

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
        [MethodImpl(MethodImplOptions.Synchronized)]

        public void UpdateDroneName(Drone newDrone)
        {
            Drone oldDrone = DataSource.drones.Find(d => d.ID == newDrone.ID);
            if (oldDrone.ID == 0)
                throw new DroneExistException($"Drone {newDrone.ID} doesn't exists in the data!!");

            DataSource.drones.Remove(oldDrone);
            oldDrone.Model = newDrone.Model;
            DataSource.drones.Add(oldDrone);
            DataSource.drones = DataSource.drones.OrderBy(d => d.ID).ToList();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]

        public void UpdateStationName(Station newStation)
        {
            Station oldStation = DataSource.stations.Find(s => s.ID == newStation.ID);
            if (oldStation.ID == 0)
                throw new StationExistException($"Station {newStation.ID} doesn't exists in the data!!");
            DataSource.stations.Remove(oldStation);
            oldStation.Name = newStation.Name;
            DataSource.stations.Add(oldStation);
            DataSource.stations = DataSource.stations.OrderBy(s => s.ID).ToList();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]

        public void UpdateStationSlots(Station newStation)
        {
            Station oldStation = DataSource.stations.Find(s => s.ID == newStation.ID);
            if (oldStation.ID == 0)
                throw new StationExistException($"Station {newStation.ID} doesn't exists in the data!!");
            DataSource.stations.Remove(oldStation);
            oldStation.ChargeSlots = newStation.ChargeSlots;
            DataSource.stations.Add(oldStation);
            DataSource.stations = DataSource.stations.OrderBy(s => s.ID).ToList();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]

        public void UpdateCustomerName(Customer newCustomer)
        {
            Customer oldCustomer = DataSource.customers.Find(c => c.ID == newCustomer.ID);
            if (oldCustomer.ID == 0)
                throw new CustomerExistException($"Customer {newCustomer.ID} doesn't exists in the data!!");
            DataSource.customers.Remove(oldCustomer);
            oldCustomer.Name = newCustomer.Name;
            DataSource.customers.Add(oldCustomer);
            DataSource.customers = DataSource.customers.OrderBy(c => c.ID).ToList();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]

        public void UpdateCustomerPhone(Customer newCustomer)
        {
            Customer oldCustomer = DataSource.customers.Find(c => c.ID == newCustomer.ID);
            if (oldCustomer.ID == 0)
                throw new CustomerExistException($"Customer {newCustomer.ID} doesn't exists in the data!!");
            DataSource.customers.Remove(oldCustomer);
            oldCustomer.Phone = newCustomer.Phone;
            DataSource.customers.Add(oldCustomer);
            DataSource.customers = DataSource.customers.OrderBy(c => c.ID).ToList();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]

        public void UpdateDroneToBeAvailable(Drone newDrone)
        {
            Drone oldDrone;
            DroneCharge droneCharge;
            Station station;

            if (DataSource.drones.Any(d => d.ID == newDrone.ID))
                oldDrone = DataSource.drones.Find(d => d.ID == newDrone.ID);
            else
                throw new DroneExistException($"Drone {newDrone.ID} doesn't exists in the data!!");

            droneCharge = DataSource.DroneCharges.Find(d => d.DroneID == newDrone.ID);     
            if(droneCharge.DroneID == 0)
                throw new DroneExistException($"Drone {newDrone.ID} isn't charging at this moment");
            if (DataSource.stations.Any(s => s.ID == droneCharge.StationID))
                station = DataSource.stations.Find(s => s.ID == droneCharge.StationID);
            else
                throw new StationExistException($"Station {droneCharge.StationID} doesn't exists in the data!!");

            DataSource.drones.Remove(oldDrone);
            oldDrone.Model = newDrone.Model;
            DataSource.drones.Add(oldDrone);
            DataSource.drones = DataSource.drones.OrderBy(d => d.ID).ToList();

            DataSource.stations.Remove(station);
            ++station.ChargeSlots;
            DataSource.stations.Add(station);
            DataSource.stations = DataSource.stations.OrderBy(s => s.ID).ToList();

            DataSource.DroneCharges.Remove(droneCharge);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]

        public void UpdateDroneToBeCharge(Drone drone, Station station, DateTime? start)
        {
            if (!DataSource.drones.Any(d => d.ID == drone.ID))
                throw new DroneExistException($"Drone {drone.ID} doesn't exists in the data!!");
            if (!DataSource.stations.Any(s => s.ID == station.ID))
                throw new StationExistException($"Station {station.ID} doesn't exists in the data!!");
            if (DataSource.DroneCharges.Any(d => d.DroneID == drone.ID))
                throw new DroneExistException($"Drone {drone.ID} is already being charged!!");
            
            DataSource.DroneCharges.Add(new(drone.ID, station.ID, start));
            station = DataSource.stations.Find(s => s.ID == station.ID);
            DataSource.stations.Remove(station);
            --station.ChargeSlots;
            DataSource.stations.Add(station);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]

        public void UpdateParcelInDelivery(Parcel newParcel)
        {
            Parcel oldParcel = DataSource.parcels.Find(p => p.ID == newParcel.ID);
            if (oldParcel.ID == 0)
                throw new DroneExistException($"Parcel {newParcel.ID} doesn't exists in the data!!");

            DataSource.parcels.Remove(oldParcel);
            oldParcel.Delivered = DateTime.Now;         ///Changing the time of the parcel to update it's been delivered now
            DataSource.parcels.Add(oldParcel);
            DataSource.parcels = DataSource.parcels.OrderBy(p => p.ID).ToList();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcelCollected(Parcel newParcel)
        {
            Parcel oldParcel = DataSource.parcels.Find(p => p.ID == newParcel.ID);
            if (oldParcel.ID == 0)
                throw new DroneExistException($"Parcel {newParcel.ID} doesn't exists in the data!!");

            DataSource.parcels.Remove(oldParcel);
            oldParcel.PickedUp = DateTime.Now;         ///Updating the time of the pickup by the drone
            DataSource.parcels.Add(oldParcel);
            DataSource.parcels = DataSource.parcels.OrderBy(p => p.ID).ToList();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PairParcelToDrone(Parcel newParcel, Drone newDrone)
        {
            Parcel oldParcel = DataSource.parcels.Find(p => p.ID == newParcel.ID);
            Drone oldDrone = DataSource.drones.Find(d => d.ID == newDrone.ID);
            if (oldParcel.ID == 0)
                throw new DroneExistException($"Parcel {newParcel.ID} doesn't exists in the data!!");
            if (oldDrone.ID == 0)
                throw new DroneExistException($"Drone {newDrone.ID} doesn't exists in the data!!");

            DataSource.parcels.Remove(oldParcel);
            oldParcel.DroneID = newDrone.ID;            ///Pairing the parcel with the ID of the drone chose to take it
            oldParcel.Scheduled = DateTime.Now;         ///Updating the scheduled time for the parcel
            DataSource.parcels.Add(oldParcel);
            DataSource.parcels = DataSource.parcels.OrderBy(p => p.ID).ToList();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDeleteParcel(Parcel parcel)
        {
            Parcel oldParcel = DataSource.parcels.Find(p => p.ID == parcel.ID);
            if (oldParcel.ID == 0)
                throw new DroneExistException($"Parcel {parcel.ID} doesn't exists in the data!!");
            DataSource.parcels.Remove(oldParcel);
            DataSource.parcels = DataSource.parcels.OrderBy(p => p.ID).ToList();
        }
        #endregion

        #region Get

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double GetBatteryProperty(string property)
        {
            switch(property)
            {
                case "BatteryUsageEmpty":
                    return DataSource.Config.Empty;
                case "BatteryUsageLightWight":
                    return DataSource.Config.LightWight;
                case "BatteryUsageMediumWight":
                    return DataSource.Config.MediumWight;
                case "BatteryUsageHaevyWight":
                    return DataSource.Config.HaevyWight;
                case "BatteryChargeRate":
                    return DataSource.Config.ChargeRate;
                default:
                    throw new InvalidBatteryPropertyException("Battery property doesn't exsits");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int droneID)
        {
            Drone drone = DataSource.drones.Find(d => d.ID == droneID);
            if(drone.ID == 0)
                throw new DroneExistException($"Drone {droneID} doesn't exists in the data!!");
            return drone;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int parcelID)
        {
            Parcel parcel = DataSource.parcels.Find(p => p.ID == parcelID);
            if (parcel.ID == 0)
                throw new ParcelExistException($"Parcel {parcelID} doesn't exists in the data!!");
            return parcel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int stationID)
        {
            Station station = DataSource.stations.Find(s => s.ID == stationID);
            if (station.ID == 0)
                throw new StationExistException($"Station {stationID} doesn't exists in the data!!");
            return station;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int customerID)
        {
            Customer customer = DataSource.customers.Find(c => c.ID == customerID);
            if (customer.ID == 0)
                throw new CustomerExistException($"Customer {customerID} doesn't exists in the data!!");
            return customer;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int droneChargeID)
        {
            DroneCharge drone = DataSource.DroneCharges.Find(d => d.DroneID == droneChargeID);
            if (drone.DroneID == 0)
                throw new DroneExistException($"Drone in charge {droneChargeID} doesn't exists in the data!!");
            return drone;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDrones(Predicate<Drone> dronePredicate)
        {
            return DataSource.drones.Where(drone => dronePredicate(drone));
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> parcelPredicate)
        {
            IEnumerable<Parcel> selectedParcels = DataSource.parcels.Where(parcel => parcelPredicate(parcel));
            return selectedParcels;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations(Predicate<Station> stationPredicate)
        {
            IEnumerable<Station> selectedStations = DataSource.stations.Where(station => stationPredicate(station));
            return selectedStations;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> customerPredicate)
        {
            IEnumerable<Customer> selectedCustomers = DataSource.customers.Where(customer => customerPredicate(customer));
            return selectedCustomers;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetDroneCharge(Predicate<DroneCharge> droneChargePredicate)
        {
            IEnumerable<DroneCharge> selectedDroneCharge = DataSource.DroneCharges.Where(droneCharge => droneChargePredicate(droneCharge));
            return selectedDroneCharge;
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
    }
}
