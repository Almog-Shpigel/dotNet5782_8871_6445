using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    sealed class DalXml : DalApi.IDal
    {
        static readonly DalXml instance = new();

        static DalXml() { }

        DalXml() { }

        public static DalXml Instance { get => instance; }

        #region DS XML Files
        readonly string customersPath = @"CustomersXml.xml";
        readonly string dronesPath = @"DronesXml.xml";
        readonly string droneChargePath = @"DroneChargeXml.xml";
        readonly string parcelsPath = @"ParcelsXml.xml";
        readonly string stationsPath = @"StationsXml.xml";
        #endregion

        #region Add

        #endregion

        #region Update
        public void UpdateDroneName(Drone newDrone)
        {
            List<Drone> ListDrones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            Drone oldDrone = ListDrones.Find(d => d.ID == newDrone.ID);

            if (oldDrone.ID != null)
            {
                ListDrones.Remove(oldDrone);
                oldDrone.Model = newDrone.Model;
                ListDrones.Add(oldDrone);
                //ListDrones.Sort();
            }
            else
                throw new DroneExistException($"Drone {newDrone.ID} dosen't exist in data!");

            XmlTools.SaveListToXmlSerializer(ListDrones, dronesPath);
        }

        public void UpdateStationName(Station newStation)
        {
            List<Station> ListStations = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            Station oldStation = ListStations.Find(s => s.ID == newStation.ID);

            if (oldStation.ID != null)
            {
                ListStations.Remove(oldStation);
                oldStation.Name = newStation.Name;
                ListStations.Add(oldStation);
            }
            else
                throw new StationExistException($"Station {newStation.ID} dosen't exist in data!");

            XmlTools.SaveListToXmlSerializer(ListStations, stationsPath);
        }

        public void UpdateCustomerName(Customer newCustomer)
        {
            List<Customer> ListCustomers = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            Customer oldCustomer = ListCustomers.Find(c => c.ID == newCustomer.ID);

            if (oldCustomer.ID != null)
            {
                ListCustomers.Remove(oldCustomer);
                oldCustomer.Name = newCustomer.Name;
                ListCustomers.Add(oldCustomer);
            }
            else
                throw new CustomerExistException($"Customer {newCustomer.ID} dosen't exist in data!");

            XmlTools.SaveListToXmlSerializer(ListCustomers, customersPath);
        }
        public void UpdateCustomerPhone(Customer customer)
        {
            List<Customer> ListCustomers = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            Customer oldCustomer = ListCustomers.Find(c => c.ID == customer.ID);

            if(oldCustomer.ID != null)
            {
                ListCustomers.Remove(oldCustomer);
                oldCustomer.Phone = '0' + customer.Phone;
                ListCustomers.Add(oldCustomer);
            }
            else
                throw new CustomerExistException($"Customer {customer.ID} dosen't exist in data!");
            XmlTools.SaveListToXmlSerializer(ListCustomers, customersPath);
        }
        public void UpdateStationSlots(Station station)
        {

        }
        public void UpdateDroneToBeAvailable(Drone drone)
        {

        }
        public void UpdateDroneToBeCharge(Drone drone, Station station, DateTime? start)
        {

        }
        public void UpdateParcelInDelivery(Drone drone)
        {

        }
        public void UpdateParcelCollected(Drone drone)
        {

        }
        public void PairParcelToDrone(Parcel parcel, Drone drone)
        {

        }
        #endregion

        #region Get
        //public double[] GetBatteryUsed()
        //{
        //    double[] BatteryUsed = new double[5];
        //    BatteryUsed[0] = DataSource.Config.Empty;
        //    BatteryUsed[1] = DataSource.Config.LightWight;
        //    BatteryUsed[2] = DataSource.Config.MediumWight;
        //    BatteryUsed[3] = DataSource.Config.HaevyWight;
        //    BatteryUsed[4] = DataSource.Config.ChargeRate;
        //    return BatteryUsed;
        //}

        public Drone GetDrone(int DroneID)
        {
            List<Drone> ListDrones = XmlTools.LoadListFromXmlSerializer<Drone>(dronesPath);
            Drone drone = ListDrones.Find(drone => DroneID == drone.ID);
            if (drone.ID != null)
                return drone;
            else
                throw new CustomerExistException($"Drone {DroneID} dosen't exist in data!");
        }

        public Parcel GetParcel(int ParcelID)
        {
            List<Parcel> ListParcels = XmlTools.LoadListFromXmlSerializer<Parcel>(parcelsPath);
            Parcel parcel = ListParcels.Find(parcel => ParcelID == parcel.ID);
            if (parcel.ID != null)
                return parcel;
            else
                throw new CustomerExistException($"Parcel {ParcelID} dosen't exist in data!");
        }

        public Station GetStation(int StationID)
        {
            List<Station> ListStation = XmlTools.LoadListFromXmlSerializer<Station>(stationsPath);
            Station station = ListStation.Find(station => StationID == station.ID);
            if (station.ID != null)
                return station;
            else
                throw new CustomerExistException($"Station {StationID} dosen't exist in data!");
        }

        public Customer GetCustomer(int CustomerID)
        {
            List<Customer> ListCustomer = XmlTools.LoadListFromXmlSerializer<Customer>(customersPath);
            Customer customer = ListCustomer.Find(customer => CustomerID == customer.ID);
            if (customer.ID != null)
                return customer;
            else
                throw new CustomerExistException($"Customer {CustomerID} dosen't exist in data!");
        }

        public DroneCharge GetDroneCharge(int DroneChargeID)
        {
            List<DroneCharge> ListDroneCharge = XmlTools.LoadListFromXmlSerializer<DroneCharge>(droneChargePath);
            DroneCharge drone = ListDroneCharge.Find(drone => DroneChargeID == drone.DroneID);
            if (drone.DroneID != null)
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

        #region Exists

        #endregion
    }
}
