using DalObject;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Runtime.CompilerServices;


namespace DAL
{
    sealed class DalXml : DalApi.IDal
    {
        internal static readonly DalXml instance = new();
        private DalXml()
        {
            /*****XML Initializtion, after the first run should put lines 18-34 on comments out *****/
            #region XML Initialize
            DataSource.Initialize();
            XmlTools.SaveListToXmlSerializer(DataSource.customers, customersPath);
            XmlTools.SaveListToXmlSerializer(DataSource.drones, dronesPath);
            XmlTools.SaveListToXmlSerializer(DataSource.DroneCharges, droneChargePath);
            XmlTools.SaveListToXmlSerializer(DataSource.parcels, parcelsPath);
            XmlTools.SaveListToXmlSerializer(DataSource.stations, stationsPath);
            XmlTools.CreateFiles(configPath);
            XElement BatteryUsageEmpty = new("BatteryUsageEmpty", 3.3);
            XElement BatteryUsageLightWight = new("BatteryUsageLightWight", 4);
            XElement BatteryUsageMediumWight = new("BatteryUsageMediumWight", 5);
            XElement BatteryUsageHaevyWight = new("BatteryUsageHaevyWight", 6.6);
            XElement BatteryChargeRate = new("BatteryChargeRate", 60);
            XElement BatteryUsage = new("BatteryUsage", BatteryUsageEmpty, BatteryUsageLightWight, BatteryUsageMediumWight, BatteryUsageHaevyWight, BatteryChargeRate);
            XElement ParcelIDCounter = new("ParcelIDCounter", 344011);
            XElement Counters = new("Counters", ParcelIDCounter);
            XElement configItems = new XElement("Config-Data", BatteryUsage, Counters);
            XmlTools.SaveListToXElement(configItems, configPath);
            #endregion

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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewStation(Station station)
        {
            XElement listStations = XmlTools.LoadListFromXElement(stationsPath);
            var x = (from s in listStations.Elements()
                     where Convert.ToInt32(s.Element("ID").Value) == station.ID
                     select s
                   ).FirstOrDefault();
            if(x is not null)
                throw new StationExistException($"Station {station.ID} exists already in the data!!");
            XElement id = new("ID", station.ID);
            XElement name = new("Name", station.Name);
            XElement latitude = new("Latitude", station.Latitude);
            XElement longitude = new("Longitude", station.Longitude);
            XElement slots = new("ChargeSlots",station.ChargeSlots);

            listStations.Add(new XElement("Station", id, name, latitude, longitude, slots));
            XmlTools.SaveListToXElement(listStations, stationsPath);
            
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewCustomer(Customer customer)
        {
            List<Customer> listCustomers = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            if (listCustomers.Any(c => c.ID == customer.ID))
                throw new CustomerExistException($"The customer ID {customer.ID} exists already in the data!!");
            listCustomers.Add(customer);
            XmlTools.SaveListToXmlSerializer(listCustomers, customersPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewParcel(Parcel parcel)
        {
            List<Parcel> listParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            List<Customer> listCustomers = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            if (!listCustomers.Any(c => c.ID == parcel.SenderID))
                throw new CustomerExistException($"Customer {parcel.SenderID} doesn't exists in the data!!");
            if (!listCustomers.Any(c => c.ID == parcel.TargetID))
                throw new CustomerExistException($"Customer {parcel.TargetID} dosen't exists in the data!!");
            XElement config = XmlTools.LoadData(configPath);
            int parcelCounter = Convert.ToInt32(config.Element("Counters").Element("ParcelIDCounter").Value);
            parcel.ID = parcelCounter;
            config.Element("Counters").Element("ParcelIDCounter").Remove();
            parcelCounter++;
            config.Element("Counters").Add(new XElement("ParcelIDCounter", parcelCounter));
            listParcels.Add(parcel);
            XmlTools.SaveListToXElement(config, configPath);
            XmlTools.SaveListToXmlSerializer(listParcels, parcelsPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewDrone(Drone drone, Station station)
        {
            List<Drone> listDrones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            List<Station> listStations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            if (listDrones.Any(d => d.ID == drone.ID))
                throw new DroneExistException($"The drone ID {drone.ID} exists already in the data!!");
            if (!listStations.Any(s => s.ID == station.ID))
                throw new StationExistException($"The station ID {station.ID} doesn't exists in the data!!");
            UpdateDroneToBeCharge(drone, station, DateTime.Now);
            listDrones.Add(drone);
            XmlTools.SaveListToXmlSerializer(listDrones, dronesPath);
        }
        #endregion

        #region Update
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneName(Drone newDrone)
        {
            List<Drone> listDrones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            Drone oldDrone = GetDrone(newDrone.ID);
            listDrones.Remove(oldDrone);
            oldDrone.Model = newDrone.Model;
            listDrones.Add(oldDrone);
            listDrones = listDrones.OrderBy(d => d.ID).ToList();
            XmlTools.SaveListToXmlSerializer(listDrones, dronesPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStationName(Station station)
        {
            XElement listStations = XmlTools.LoadListFromXElement(stationsPath);
            XElement oldStation = (from s in listStations.Elements()
                                   where Convert.ToInt32(s.Element("ID").Value) == station.ID
                                   select s).FirstOrDefault();
            if (oldStation is null)
                throw new StationExistException($"Station {station.ID} doesn't exists in the data!!");
            oldStation.Element("Name").Value = station.Name;
            XmlTools.SaveListToXElement(listStations, stationsPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStationSlots(Station station)
        {
            XElement listStations = XmlTools.LoadListFromXElement(stationsPath);
            XElement oldStation = (from s in listStations.Elements()
                                   where Convert.ToInt32(s.Element("ID").Value) == station.ID
                                   select s).FirstOrDefault();
            if (oldStation is null)
                throw new StationExistException($"Station {station.ID} doesn't exists in the data!!");
            oldStation.Element("ChargeSlots").Value = station.ChargeSlots.ToString();
            XmlTools.SaveListToXElement(listStations, stationsPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomerName(Customer newCustomer)
        {
            List<Customer> listCustomers = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            Customer oldCustomer = GetCustomer(newCustomer.ID);
            listCustomers.Remove(oldCustomer);
            oldCustomer.Name = newCustomer.Name;
            listCustomers.Add(oldCustomer);
            listCustomers = listCustomers.OrderBy(c => c.ID).ToList();
            XmlTools.SaveListToXmlSerializer(listCustomers, customersPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomerPhone(Customer customer)
        {
            List<Customer> listCustomers = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            Customer oldCustomer = GetCustomer(customer.ID);
            listCustomers.Remove(oldCustomer);
            oldCustomer.Phone = '0' + customer.Phone;
            listCustomers.Add(oldCustomer);
            listCustomers = listCustomers.OrderBy(c => c.ID).ToList();
            XmlTools.SaveListToXmlSerializer(listCustomers, customersPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneToBeAvailable(Drone drone)
        {
            Drone oldDrone = GetDrone(drone.ID);

            List<DroneCharge> listDronesCharge = XmlTools.LoadListFromXmlSerializer<DroneCharge>(droneChargePath);
            DroneCharge droneCharge = GetDroneCharge(oldDrone.ID);
            listDronesCharge.Remove(droneCharge);
            List<Station> listStations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            Station station = GetStation(droneCharge.StationID);
            listStations.Remove(station);
            station.ChargeSlots++;
            listStations.Add(station);
            listStations = listStations.OrderBy(s => s.ID).ToList();
            XmlTools.SaveListToXmlSerializer(listStations, stationsPath);
            XmlTools.SaveListToXmlSerializer(listDronesCharge, droneChargePath);            
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneToBeCharge(Drone drone, Station station, DateTime? start)
        {
            List<Station> listStations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            List<DroneCharge> listDronesCharge = XmlTools.LoadListFromXmlSerializer<DroneCharge>(droneChargePath);
            Station updatedStation = GetStation(station.ID);
            Drone droneToBeCharged = GetDrone(drone.ID);
            DroneCharge droneCharge = new DroneCharge(droneToBeCharged.ID, station.ID, start);
            listDronesCharge.Add(droneCharge);
            listDronesCharge = listDronesCharge.OrderBy(d => d.DroneID).ToList();
            listStations.Remove(updatedStation);
            updatedStation.ChargeSlots--;
            listStations.Add(updatedStation);
            listStations = listStations.OrderBy(s => s.ID).ToList();
            XmlTools.SaveListToXmlSerializer(listStations, stationsPath);
            XmlTools.SaveListToXmlSerializer(listDronesCharge, droneChargePath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcelInDelivery(Parcel parcel)
        {
            List<Parcel> listParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            Parcel updatedParcel = GetParcel(parcel.ID);
            listParcels.Remove(updatedParcel);
            updatedParcel.Delivered = DateTime.Now;
            listParcels.Add(updatedParcel);
            listParcels = listParcels.OrderBy(p => p.ID).ToList();
            XmlTools.SaveListToXmlSerializer(listParcels, parcelsPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcelCollected(Parcel newParcel)
        {
            List<Parcel> listParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            Parcel oldParcel = listParcels.Find(p => p.ID == newParcel.ID);
            if (oldParcel.ID == 0)
                throw new DroneExistException($"Parcel {newParcel.ID} doesn't exists in the data!!");

            listParcels.Remove(oldParcel);
            oldParcel.PickedUp = DateTime.Now;         ///Updating the time of the pickup by the drone
            listParcels.Add(oldParcel);
            XmlTools.SaveListToXmlSerializer(listParcels, parcelsPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PairParcelToDrone(Parcel newParcel, Drone newDrone)
        {
            List<Parcel> listParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            List<Drone> DroneParcels = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            Parcel oldParcel = listParcels.Find(p => p.ID == newParcel.ID);
            Drone oldDrone = DroneParcels.Find(d => d.ID == newDrone.ID);
            if (oldParcel.ID == 0)
                throw new DroneExistException($"Parcel {newParcel.ID} doesn't exists in the data!!");
            if (oldDrone.ID == 0)
                throw new DroneExistException($"Drone {newDrone.ID} doesn't exists in the data!!");

            listParcels.Remove(oldParcel);
            oldParcel.DroneID = newDrone.ID;            ///Pairing the parcel with the ID of the drone chose to take it
            oldParcel.Scheduled = DateTime.Now;         ///Updating the scheduled time for the parcel
            listParcels.Add(oldParcel);
            listParcels = listParcels.OrderBy(p => p.ID).ToList();
            XmlTools.SaveListToXmlSerializer(listParcels, parcelsPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDeleteParcel(Parcel parcel)
        {
            List<Parcel> listParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            Parcel updatedParcel = GetParcel(parcel.ID);
            listParcels.Remove(updatedParcel);
            listParcels = listParcels.OrderBy(p => p.ID).ToList();
            XmlTools.SaveListToXmlSerializer(listParcels, parcelsPath);
        }
        #endregion

        #region Get
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double GetBatteryProperty(string elementString)
        {
            XElement batteryPropertyList = XmlTools.LoadData(configPath);
            double batteryProperty = 0;
            switch (elementString)
            {
                case "BatteryUsageEmpty":
                    batteryProperty =Convert.ToDouble( batteryPropertyList.Element("BatteryUsage").Element("BatteryUsageEmpty").Value);
                    break;
                case "BatteryUsageLightWight":
                    batteryProperty = Convert.ToDouble(batteryPropertyList.Element("BatteryUsage").Element("BatteryUsageLightWight").Value);
                    break;
                case "BatteryUsageMediumWight":
                    batteryProperty = Convert.ToDouble(batteryPropertyList.Element("BatteryUsage").Element("BatteryUsageMediumWight").Value);
                    break;
                case "BatteryUsageHaevyWight":
                    batteryProperty = Convert.ToDouble(batteryPropertyList.Element("BatteryUsage").Element("BatteryUsageHaevyWight").Value);
                    break;
                case "BatteryChargeRate":
                    batteryProperty = Convert.ToDouble(batteryPropertyList.Element("BatteryUsage").Element("BatteryChargeRate").Value);
                    break;
                default:
                    break;
            }            
            return batteryProperty;
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int droneID)
        {
            List<Drone> listDrones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            Drone drone = listDrones.Find(drone => droneID == drone.ID);
            if (drone.ID != 0)
                return drone;
            else
                throw new CustomerExistException($"Drone {droneID} doesn't exist in data!");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int parcelID)
        {
            List<Parcel> listParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            Parcel parcel = listParcels.Find(parcel => parcelID == parcel.ID);
            if (parcel.ID != 0)
                return parcel;
            else
                throw new CustomerExistException($"Parcel {parcelID} doesn't exist in data!");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int stationID)
        {
            XElement listStations = XmlTools.LoadListFromXElement(stationsPath);
            Station station = (from s in listStations.Elements()
                               where Convert.ToInt32(s.Element("ID").Value) == stationID
                               select new Station()
                               {
                                   ID = Convert.ToInt32(s.Element("ID").Value),
                                   ChargeSlots = Convert.ToInt32(s.Element("ChargeSlots").Value),
                                   Latitude = Convert.ToDouble(s.Element("Latitude").Value),
                                   Longitude = Convert.ToDouble(s.Element("Longitude").Value),
                                   Name = s.Element("Name").Value
                               }).FirstOrDefault();
            if (station.ID != 0)
                return station;
            else
                throw new StationExistException($"Station {stationID} doesn't exist in data!");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int customerID)
        {
            List<Customer> ListCustomer = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            Customer customer = ListCustomer.Find(customer => customerID == customer.ID);
            if (customer.ID != 0)
                return customer;
            else
                throw new CustomerExistException($"Customer {customerID} doesn't exist in data!");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int droneChargeID)
        {
            List<DroneCharge> listDroneCharge = XmlTools.LoadListFromXmlSerializer<DroneCharge>(droneChargePath);
            DroneCharge drone = listDroneCharge.Find(drone => droneChargeID == drone.DroneID);
            if (drone.DroneID != 0)
                return drone;
            else
                throw new DroneExistException($"Drone charge {droneChargeID} doesn't exist in data!");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> customerPredicate)
        {
            List<Customer> listCustomers = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            return from customer in listCustomers
                   where customerPredicate(customer)
                   select customer;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDrones(Predicate<Drone> dronePredicate)
        {
            List<Drone> listDrones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            return from drone in listDrones
                   where dronePredicate(drone)
                   select drone;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> parcelPredicate)
        {
            List<Parcel> listParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            return from parcel in listParcels
                   where parcelPredicate(parcel)
                   select parcel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations(Predicate<Station> stationPredicate)
        {
            XElement listStations = XmlTools.LoadListFromXElement(stationsPath);
            var stations = (from s in listStations.Elements()
                                      select new Station()
                                      {
                                          ID = Convert.ToInt32(s.Element("ID").Value),
                                          ChargeSlots = Convert.ToInt32(s.Element("ChargeSlots").Value),
                                          Latitude = Convert.ToDouble(s.Element("Latitude").Value),
                                          Longitude = Convert.ToDouble(s.Element("Longitude").Value),
                                          Name = s.Element("Name").Value
                                      }).Where(s => stationPredicate(s));
            return stations;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetDroneCharge(Predicate<DroneCharge> droneChangePredicate)
        {
            List<DroneCharge> listDroneCharge = XmlTools.LoadListFromXmlSerializer<DroneCharge>(droneChargePath);
            return from drone in listDroneCharge
                   where droneChangePredicate(drone)
                   select drone;
        }
        #endregion
    }
}
