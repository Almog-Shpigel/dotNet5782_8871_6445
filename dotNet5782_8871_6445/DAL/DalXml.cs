using DO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    sealed class DalXml : DalApi.IDal
    {
        internal static readonly DalXml instance = new();

        static DalXml() { }

        private DalXml() {
            //DataSource.Initialize();
            //XmlTools.SaveListToXmlSerializer(DataSource.customers, customersPath);
            //XmlTools.SaveListToXmlSerializer(DataSource.drones, dronesPath);
            //XmlTools.SaveListToXmlSerializer(DataSource.DroneCharges, droneChargePath);
            //XmlTools.SaveListToXmlSerializer(DataSource.parcels, parcelsPath);
            //XmlTools.SaveListToXmlSerializer(DataSource.stations, stationsPath);
        }

        public static DalXml Instance { get => instance; }

        #region DS XML Files
        readonly string customersPath = @"CustomersXml.xml";
        readonly string dronesPath = @"DronesXml.xml";
        readonly string droneChargePath = @"DroneChargeXml.xml";
        readonly string parcelsPath = @"ParcelsXml.xml";
        readonly string stationsPath = @"StationsXml.xml";
        readonly string configPath = @"ConfigXml.xml";
        #endregion

        #region Add
        public void AddNewStation(Station station)
        {
            List<Station> ListStations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            if (ListStations.Any(s => s.ID == station.ID))
                throw new StationExistException($"The station ID {station.ID} exists already in the data!!");
            station.Name = "Station " + station.Name;
            ListStations.Add(station);
            XmlTools.SaveListToXmlSerializer(ListStations, stationsPath);
        }

        public void AddNewCustomer(Customer customer)
        {
            List<Customer> ListCustomers = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            if (ListCustomers.Any(c => c.ID == customer.ID))
                throw new CustomerExistException($"The customer ID {customer.ID} exists already in the data!!");
            ListCustomers.Add(customer);
            XmlTools.SaveListToXmlSerializer(ListCustomers, customersPath);
        }

        public void AddNewParcel(Parcel parcel)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            List<Customer> ListCustomers = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            if (ListCustomers.Any(c => c.ID == parcel.SenderID))
                throw new CustomerExistException($"Customer {parcel.SenderID} dosen't exists in the data!!");
            if (ListCustomers.Any(c => c.ID == parcel.TargetID))
                throw new CustomerExistException($"Customer {parcel.TargetID} dosen't exists in the data!!");

            //parcel.ID = 344000 + ++DataSource.Config.ParcelsCounter;
            ListParcels.Add(parcel);
            XmlTools.SaveListToXmlSerializer(ListParcels, parcelsPath);
        }

        public void AddNewDrone(Drone drone, Station station)
        {
            List<Drone> ListDrones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            List<Station> ListStations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            if (ListDrones.Any(d => d.ID == drone.ID))
                throw new DroneExistException($"The drone ID {drone.ID} exists already in the data!!");
            if (!ListStations.Any(s => s.ID == station.ID))
                throw new StationExistException($"The station ID {station.ID} doesn't exists in the data!!");
            UpdateDroneToBeCharge(drone, station, DateTime.Now);
            ListDrones.Add(drone);
            XmlTools.SaveListToXmlSerializer(ListDrones, dronesPath);
        }
        #endregion

        #region Update
        public void UpdateDroneName(Drone newDrone)
        {
            List<Drone> ListDrones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            Drone oldDrone = GetDrone(newDrone.ID);
            ListDrones.Remove(oldDrone);
            oldDrone.Model = newDrone.Model;
            ListDrones.Add(oldDrone);
            ListDrones = ListDrones.OrderBy(d => d.ID).ToList();
            XmlTools.SaveListToXmlSerializer(ListDrones, dronesPath);
        }

        public void UpdateStationName(Station station)
        {
            List<Station> ListStations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            Station oldStation = GetStation(station.ID);
            ListStations.Remove(oldStation);
            oldStation.Name = station.Name;
            ListStations.Add(oldStation);
            ListStations = ListStations.OrderBy(s => s.ID).ToList();
            XmlTools.SaveListToXmlSerializer(ListStations, stationsPath);
        }

        public void UpdateStationSlots(Station station)
        {

            List<Station> ListStations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            Station oldStation = GetStation(station.ID);
            ListStations.Remove(oldStation);
            oldStation.ChargeSlots = station.ChargeSlots;
            ListStations.Add(oldStation);
            ListStations = ListStations.OrderBy(s => s.ID).ToList();
            XmlTools.SaveListToXmlSerializer(ListStations, stationsPath);
        }

        public void UpdateCustomerName(Customer newCustomer)
        {
            List<Customer> ListCustomers = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            Customer oldCustomer = GetCustomer(newCustomer.ID);
            ListCustomers.Remove(oldCustomer);
            oldCustomer.Name = newCustomer.Name;
            ListCustomers.Add(oldCustomer);
            ListCustomers = ListCustomers.OrderBy(c => c.ID).ToList();
            XmlTools.SaveListToXmlSerializer(ListCustomers, customersPath);
        }

        public void UpdateCustomerPhone(Customer customer)
        {
            List<Customer> ListCustomers = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            Customer oldCustomer = GetCustomer(customer.ID);
            ListCustomers.Remove(oldCustomer);
            oldCustomer.Phone = '0' + customer.Phone;
            ListCustomers.Add(oldCustomer);
            ListCustomers = ListCustomers.OrderBy(c => c.ID).ToList();
            XmlTools.SaveListToXmlSerializer(ListCustomers, customersPath);
        }
       
        public void UpdateDroneToBeAvailable(Drone drone)
        {
            Drone oldDrone = GetDrone(drone.ID);

            List<DroneCharge> ListDronesCharge = XmlTools.LoadListFromXmlSerializer<DroneCharge>(droneChargePath);
            DroneCharge droneCharge = GetDroneCharge(oldDrone.ID);
            ListDronesCharge.Remove(droneCharge);

            List<Station> ListStations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            Station station = GetStation(droneCharge.StationID);
            ListStations.Remove(station);
            station.ChargeSlots++;
            ListStations.Add(station);
            ListStations = ListStations.OrderBy(s => s.ID).ToList();
            XmlTools.SaveListToXmlSerializer(ListStations, stationsPath);
            XmlTools.SaveListToXmlSerializer(ListDronesCharge, droneChargePath);            
        }

        public void UpdateDroneToBeCharge(Drone drone, Station station, DateTime? start)
        {
            List<Station> ListStations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            List<DroneCharge> ListDronesCharge = XmlTools.LoadListFromXmlSerializer<DroneCharge>(droneChargePath);
            Station UpdatedStation = GetStation(station.ID);
            DroneCharge droneCharge = new DroneCharge(drone.ID, station.ID, start);
            ListDronesCharge.Add(droneCharge);
            ListDronesCharge = ListDronesCharge.OrderBy(d => d.DroneID).ToList();
            ListStations.Remove(UpdatedStation);
            UpdatedStation.ChargeSlots--;
            ListStations.Add(UpdatedStation);
            ListStations = ListStations.OrderBy(s => s.ID).ToList();
            XmlTools.SaveListToXmlSerializer(ListStations, stationsPath);
            XmlTools.SaveListToXmlSerializer(ListDronesCharge, droneChargePath);
        }

        public void UpdateParcelInDelivery(Parcel parcel)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            Parcel UpdatedParcel = GetParcel(parcel.ID);
            ListParcels.Remove(UpdatedParcel);
            UpdatedParcel.Delivered = DateTime.Now;
            ListParcels.Add(UpdatedParcel);
            ListParcels = ListParcels.OrderBy(p => p.ID).ToList();
            XmlTools.SaveListToXmlSerializer(ListParcels, parcelsPath);
        }

        public void UpdateParcelCollected(Parcel newParcel)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            Parcel oldParcel = ListParcels.Find(p => p.ID == newParcel.ID);
            if (oldParcel.ID == 0)
                throw new DroneExistException($"Parcel {newParcel.ID} dosen't exists in the data!!");

            ListParcels.Remove(oldParcel);
            oldParcel.PickedUp = DateTime.Now;         ///Updating the time of the pickup by the drone
            ListParcels.Add(oldParcel);
            XmlTools.SaveListToXmlSerializer(ListParcels, parcelsPath);
        }

        public void PairParcelToDrone(Parcel newParcel, Drone newDrone)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            List<Drone> DroneParcels = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            Parcel oldParcel = ListParcels.Find(p => p.ID == newParcel.ID);
            Drone oldDrone = DroneParcels.Find(d => d.ID == newDrone.ID);
            if (oldParcel.ID == 0)
                throw new DroneExistException($"Parcel {newParcel.ID} dosen't exists in the data!!");
            if (oldDrone.ID == 0)
                throw new DroneExistException($"Drone {newDrone.ID} doesn't exists in the data!!");

            ListParcels.Remove(oldParcel);
            oldParcel.DroneID = newDrone.ID;            ///Pairing the parcel with the ID of the drone chose to take it
            oldParcel.Scheduled = DateTime.Now;         ///Updating the scheduled time for the parcel
            ListParcels.Add(oldParcel);
            ListParcels = ListParcels.OrderBy(p => p.ID).ToList();
            XmlTools.SaveListToXmlSerializer(ListParcels, parcelsPath);
        }
        #endregion

        #region Get
        public double[] GetBatteryUsed()
        {
            double[] BatteryUsed = new double[5];
            BatteryUsed[0] = 3.3;
            BatteryUsed[1] = 4;
            BatteryUsed[2] = 5;
            BatteryUsed[3] = 6.6;
            BatteryUsed[4] = 60;
            return BatteryUsed;
        }

        public Drone GetDrone(int DroneID)
        {
            List<Drone> ListDrones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            Drone drone = ListDrones.Find(drone => DroneID == drone.ID);
            if (drone.ID != 0)
                return drone;
            else
                throw new CustomerExistException($"Drone {DroneID} dosen't exist in data!");
        }

        public Parcel GetParcel(int ParcelID)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            Parcel parcel = ListParcels.Find(parcel => ParcelID == parcel.ID);
            if (parcel.ID != 0)
                return parcel;
            else
                throw new CustomerExistException($"Parcel {ParcelID} dosen't exist in data!");
        }

        public Station GetStation(int StationID)
        {
            List<Station> ListStation = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            Station station = ListStation.Find(station => StationID == station.ID);
            if (station.ID != 0)
                return station;
            else
                throw new CustomerExistException($"Station {StationID} dosen't exist in data!");
        }

        public Customer GetCustomer(int CustomerID)
        {
            List<Customer> ListCustomer = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            Customer customer = ListCustomer.Find(customer => CustomerID == customer.ID);
            if (customer.ID != 0)
                return customer;
            else
                throw new CustomerExistException($"Customer {CustomerID} dosen't exist in data!");
        }

        public DroneCharge GetDroneCharge(int DroneChargeID)
        {
            List<DroneCharge> ListDroneCharge = XmlTools.LoadListFromXmlSerializer<DroneCharge>(droneChargePath);
            DroneCharge drone = ListDroneCharge.Find(drone => DroneChargeID == drone.DroneID);
            if (drone.DroneID != 0)
                return drone;
            else
                throw new CustomerExistException($"Customer {DroneChargeID} dosen't exist in data!");
        }

        public IEnumerable<Customer> GetCustomers(Predicate<Customer> CustomerPredicate)
        {
            List<Customer> ListCustomers = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            return from customer in ListCustomers
                   where CustomerPredicate(customer)
                   select customer;
        }

        public IEnumerable<Drone> GetDrones(Predicate<Drone> DronePredicate)
        {
            List<Drone> ListDrones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            return from drone in ListDrones
                   where DronePredicate(drone)
                   select drone;
        }

        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> ParcelPredicate)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            return from parcel in ListParcels
                   where ParcelPredicate(parcel)
                   select parcel;
        }

        public IEnumerable<Station> GetStations(Predicate<Station> StationPredicate)
        {
            List<Station> ListStations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            return from station in ListStations
                   where StationPredicate(station)
                   select station;
        }

        public IEnumerable<DroneCharge> GetDroneCharge(Predicate<DroneCharge> DroneChangePredicate)
        {
            List<DroneCharge> ListDroneCharge = XmlTools.LoadListFromXmlSerializer<DroneCharge>(droneChargePath);
            return from drone in ListDroneCharge
                   where DroneChangePredicate(drone)
                   select drone;
        }
        #endregion

        #region Calc

        #endregion
    }
}
