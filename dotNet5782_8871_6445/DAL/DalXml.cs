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

        //static DalXml() { }
        ////struct config
        ////{
        ////    internal static int ParcelsCounter = 0;
        ////    /// <summary>
        ////    /// According to our reasearch, a drone with full battery and empty cargo can fly 30 km
        ////    /// A drone with full battery and light cargo can fly 25 km
        ////    /// A drone with full battery and medium cargo can fly 20 km
        ////    /// A drone with full battery and heavy cargo can fly 15 km
        ////    /// </summary>
            
        ////    internal static double Empty = 3.3;
        ////    internal static double LightWight = 4;
        ////    internal static double MediumWight = 5;
        ////    internal static double HaevyWight = 6.6;
        ////    internal static double ChargeRate = 60;     ///1% per minute
        ////}
        private DalXml()
        {
            //DataSource.Initialize();
            //XmlTools.SaveListToXmlSerializer(DataSource.customers, customersPath);
            //XmlTools.SaveListToXmlSerializer(DataSource.drones, dronesPath);
            //XmlTools.SaveListToXmlSerializer(DataSource.DroneCharges, droneChargePath);
            //XmlTools.SaveListToXmlSerializer(DataSource.parcels, parcelsPath);
            //XmlTools.SaveListToXmlSerializer(DataSource.stations, stationsPath);
            //XmlTools.CreateFiles(configPath);
            //XElement BatteryUsageEmpty = new("BatteryUsageEmpty", 3.3);
            //XElement BatteryUsageLightWight = new("BatteryUsageLightWight", 4);
            //XElement BatteryUsageMediumWight = new("BatteryUsageMediumWight", 5);
            //XElement BatteryUsageHaevyWight = new("BatteryUsageHaevyWight", 6.6);
            //XElement BatteryChargeRate = new("BatteryChargeRate", 60);
            //XElement BatteryUsage = new("BatteryUsage", BatteryUsageEmpty, BatteryUsageLightWight, BatteryUsageMediumWight, BatteryUsageHaevyWight, BatteryChargeRate);
            //XElement ParcelIDCounter = new("ParcelIDCounter", 344011);
            //XElement Counters = new("Counters", ParcelIDCounter);
            //XElement configItems = new XElement("Config-Data", BatteryUsage, Counters);
            //XmlTools.SaveListToXElement(configItems, configPath);

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
            XElement ListStations = XmlTools.LoadListFromXElement(stationsPath);
            var x = (from s in ListStations.Elements()
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

            ListStations.Add(new XElement("Station", id, name, latitude, longitude, slots));
            XmlTools.SaveListToXElement(ListStations, stationsPath);
            
            
            //x = x.FirstOrDefault(s => s.Element("ID")?.Value == station.ID.ToString())
            //x = ListStations.Descendants("Station").FirstOrDefault(s => s.Element("ID")?.Value == station.ID.ToString());
            //if (x != null)
            //    throw new StationExistException($"The station ID {station.ID} exists already in the data!!");


/***********************************************************************************************************************************/
            //List<Station> ListStations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            //if (ListStations.Any(s => s.ID == station.ID))
            //    throw new StationExistException($"The station ID {station.ID} exists already in the data!!");
            //station.Name = "Station " + station.Name;
            //ListStations.Add(station);
            //XmlTools.SaveListToXmlSerializer(ListStations, stationsPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewCustomer(Customer customer)
        {
            List<Customer> ListCustomers = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            if (ListCustomers.Any(c => c.ID == customer.ID))
                throw new CustomerExistException($"The customer ID {customer.ID} exists already in the data!!");
            ListCustomers.Add(customer);
            XmlTools.SaveListToXmlSerializer(ListCustomers, customersPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewParcel(Parcel parcel)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            List<Customer> ListCustomers = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            if (!ListCustomers.Any(c => c.ID == parcel.SenderID))
                throw new CustomerExistException($"Customer {parcel.SenderID} doesn't exists in the data!!");
            if (!ListCustomers.Any(c => c.ID == parcel.TargetID))
                throw new CustomerExistException($"Customer {parcel.TargetID} dosen't exists in the data!!");
            XElement config = XmlTools.LoadData(configPath);
            int parcelCounter = Convert.ToInt32(config.Element("Counters").Element("ParcelIDCounter").Value);
            parcel.ID = parcelCounter;
            config.Element("Counters").Element("ParcelIDCounter").Remove();
            parcelCounter++;
            config.Element("Counters").Add(new XElement("ParcelIDCounter", parcelCounter));
            ListParcels.Add(parcel);
            XmlTools.SaveListToXmlSerializer(ListParcels, parcelsPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStationName(Station station)
        {
            XElement ListStations = XmlTools.LoadListFromXElement(stationsPath);
            XElement oldStation = (from s in ListStations.Elements()
                                   where Convert.ToInt32(s.Element("ID").Value) == station.ID
                                   select s).FirstOrDefault();
            if (oldStation is null)
                throw new StationExistException($"Station {station.ID} doesn't exists in the data!!");
            oldStation.Element("Name").Value = station.Name;
            XmlTools.SaveListToXElement(ListStations, stationsPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStationSlots(Station station)
        {
            XElement ListStations = XmlTools.LoadListFromXElement(stationsPath);
            XElement oldStation = (from s in ListStations.Elements()
                                   where Convert.ToInt32(s.Element("ID").Value) == station.ID
                                   select s).FirstOrDefault();
            if (oldStation is null)
                throw new StationExistException($"Station {station.ID} doesn't exists in the data!!");
            oldStation.Element("ChargeSlots").Value = station.ChargeSlots.ToString();
            XmlTools.SaveListToXElement(ListStations, stationsPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcelCollected(Parcel newParcel)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            Parcel oldParcel = ListParcels.Find(p => p.ID == newParcel.ID);
            if (oldParcel.ID == 0)
                throw new DroneExistException($"Parcel {newParcel.ID} doesn't exists in the data!!");

            ListParcels.Remove(oldParcel);
            oldParcel.PickedUp = DateTime.Now;         ///Updating the time of the pickup by the drone
            ListParcels.Add(oldParcel);
            XmlTools.SaveListToXmlSerializer(ListParcels, parcelsPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PairParcelToDrone(Parcel newParcel, Drone newDrone)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            List<Drone> DroneParcels = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            Parcel oldParcel = ListParcels.Find(p => p.ID == newParcel.ID);
            Drone oldDrone = DroneParcels.Find(d => d.ID == newDrone.ID);
            if (oldParcel.ID == 0)
                throw new DroneExistException($"Parcel {newParcel.ID} doesn't exists in the data!!");
            if (oldDrone.ID == 0)
                throw new DroneExistException($"Drone {newDrone.ID} doesn't exists in the data!!");

            ListParcels.Remove(oldParcel);
            oldParcel.DroneID = newDrone.ID;            ///Pairing the parcel with the ID of the drone chose to take it
            oldParcel.Scheduled = DateTime.Now;         ///Updating the scheduled time for the parcel
            ListParcels.Add(oldParcel);
            ListParcels = ListParcels.OrderBy(p => p.ID).ToList();
            XmlTools.SaveListToXmlSerializer(ListParcels, parcelsPath);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDeleteParcel(Parcel parcel)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            Parcel UpdatedParcel = GetParcel(parcel.ID);
            ListParcels.Remove(UpdatedParcel);
            ListParcels = ListParcels.OrderBy(p => p.ID).ToList();
            XmlTools.SaveListToXmlSerializer(ListParcels, parcelsPath);
        }
        #endregion

        #region Get
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double GetBatteryProperty(string elementString)
        {
            XElement BatteryPropertyList = XmlTools.LoadData(configPath);
            double batteryProperty = 0;
            switch (elementString)
            {
                case "BatteryUsageEmpty":
                    batteryProperty =Convert.ToDouble( BatteryPropertyList.Element("BatteryUsage").Element("BatteryUsageEmpty").Value);
                    break;
                case "BatteryUsageLightWight":
                    batteryProperty = Convert.ToDouble(BatteryPropertyList.Element("BatteryUsage").Element("BatteryUsageLightWight").Value);
                    break;
                case "BatteryUsageMediumWight":
                    batteryProperty = Convert.ToDouble(BatteryPropertyList.Element("BatteryUsage").Element("BatteryUsageMediumWight").Value);
                    break;
                case "BatteryUsageHaevyWight":
                    batteryProperty = Convert.ToDouble(BatteryPropertyList.Element("BatteryUsage").Element("BatteryUsageHaevyWight").Value);
                    break;
                case "BatteryChargeRate":
                    batteryProperty = Convert.ToDouble(BatteryPropertyList.Element("BatteryUsage").Element("BatteryChargeRate").Value);
                    break;
                default:
                    break;
            }            
            return batteryProperty;
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int DroneID)
        {
            List<Drone> ListDrones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            Drone drone = ListDrones.Find(drone => DroneID == drone.ID);
            if (drone.ID != 0)
                return drone;
            else
                throw new CustomerExistException($"Drone {DroneID} doesn't exist in data!");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int ParcelID)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            Parcel parcel = ListParcels.Find(parcel => ParcelID == parcel.ID);
            if (parcel.ID != 0)
                return parcel;
            else
                throw new CustomerExistException($"Parcel {ParcelID} doesn't exist in data!");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int StationID)
        {
            XElement ListStations = XmlTools.LoadListFromXElement(stationsPath);
            Station station = (from s in ListStations.Elements()
                               where Convert.ToInt32(s.Element("ID").Value) == StationID
                               select new Station()
                               {
                                   ID = Convert.ToInt32(s.Element("ID").Value),
                                   ChargeSlots = Convert.ToInt32(s.Element("ChargeSlots").Value),
                                   Latitude = Convert.ToDouble(s.Element("Latitude").Value),
                                   Longitude = Convert.ToDouble(s.Element("Longitude").Value),
                                   Name = s.Element("Name").Value
                               }).FirstOrDefault();
            //try
            //{
            //    station = (from s in ListStations.Elements()
            //                   where Convert.ToInt32(s.Element("ID").Value) == StationID
            //                   select new Station()
            //                   {
            //                       ID = Convert.ToInt32(s.Element("ID").Value),
            //                       ChargeSlots = Convert.ToInt32(s.Element("ChargeSlots").Value),
            //                       Latitude = Convert.ToDouble(s.Element("Latitude").Value),
            //                       Longitude = Convert.ToDouble(s.Element("Longitude").Value),
            //                       Name = s.Element("Name").Value
            //                   }).FirstOrDefault();
            //}
            //catch (Exception)
            //{
            //    station = new();
            //}
            return station;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int CustomerID)
        {
            List<Customer> ListCustomer = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            Customer customer = ListCustomer.Find(customer => CustomerID == customer.ID);
            if (customer.ID != 0)
                return customer;
            else
                throw new CustomerExistException($"Customer {CustomerID} doesn't exist in data!");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int DroneChargeID)
        {
            List<DroneCharge> ListDroneCharge = XmlTools.LoadListFromXmlSerializer<DroneCharge>(droneChargePath);
            DroneCharge drone = ListDroneCharge.Find(drone => DroneChargeID == drone.DroneID);
            if (drone.DroneID != 0)
                return drone;
            else
                throw new CustomerExistException($"Customer {DroneChargeID} doesn't exist in data!");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> CustomerPredicate)
        {
            List<Customer> ListCustomers = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            return from customer in ListCustomers
                   where CustomerPredicate(customer)
                   select customer;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDrones(Predicate<Drone> DronePredicate)
        {
            List<Drone> ListDrones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            return from drone in ListDrones
                   where DronePredicate(drone)
                   select drone;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> ParcelPredicate)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            return from parcel in ListParcels
                   where ParcelPredicate(parcel)
                   select parcel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations(Predicate<Station> StationPredicate)
        {
            XElement ListStations = XmlTools.LoadListFromXElement(stationsPath);
            List<Station> stations = (from s in ListStations.Elements()
                                      select new Station()
                                      {
                                          ID = Convert.ToInt32(s.Element("ID").Value),
                                          ChargeSlots = Convert.ToInt32(s.Element("ChargeSlots").Value),
                                          Latitude = Convert.ToDouble(s.Element("Latitude").Value),
                                          Longitude = Convert.ToDouble(s.Element("Longitude").Value),
                                          Name = s.Element("Name").Value
                                      }).Where(s => StationPredicate(s)).ToList();
            //try
            //{
            //    stations = (from s in ListStations.Elements()
            //                select new Station()
            //                {
            //                    ID = Convert.ToInt32(s.Element("ID").Value),
            //                    ChargeSlots = Convert.ToInt32(s.Element("ChargeSlots").Value),
            //                    Latitude = Convert.ToDouble(s.Element("Latitude").Value),
            //                    Longitude = Convert.ToDouble(s.Element("Longitude").Value),
            //                    Name = s.Element("Name").Value
            //                }).Where(s => StationPredicate(s)).ToList();
            //}
            //catch (Exception)
            //{
            //    stations = null;
            //}
            return stations;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
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
