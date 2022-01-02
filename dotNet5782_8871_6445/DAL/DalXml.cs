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
        #endregion

        #region Get
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
        #endregion

        #region Calc

        #endregion

        #region Exists

        #endregion
    }
}
