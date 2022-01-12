using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{ 
    public interface IDal
    {
        #region Add
        /// <summary>
        /// Adding the new station to the data
        /// </summary>
        /// <param name="station"></param>
        public void AddNewStation(Station station);

        /// <summary>
        /// Adding the new customer to the data
        /// </summary>
        /// <param name="customer"></param>
        public void AddNewCustomer(Customer customer);

        /// <summary>
        /// Adding the new parcel to the data
        /// </summary>
        /// <param name="parcel"></param>
        public void AddNewParcel(Parcel parcel);

        /// <summary>
        /// Adding the new drone to the data, and initiallizing him as charging in the given station
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="station"></param>
        public void AddNewDrone(Drone drone, Station station);
        #endregion

        #region Update
        /// <summary>
        /// Updating a drone's name
        /// </summary>
        /// <param name="drone"></param>
        public void UpdateDroneName(Drone drone);

        /// <summary>
        /// Updating a station's name
        /// </summary>
        /// <param name="station"></param>
        public void UpdateStationName(Station station);

        /// <summary>
        /// Updating a customer's name
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomerName(Customer customer);

        /// <summary>
        /// Updating a customer's phone number
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomerPhone(Customer customer);

        /// <summary>
        /// Updating a station's total charging slots
        /// </summary>
        /// <param name="station"></param>
        public void UpdateStationSlots(Station station);

        /// <summary>
        /// Releasing a drone from charging by deleting the specific entity "DroneCharge" saved in the data with the wanted drone's ID 
        /// </summary>
        /// <param name="drone"></param>
        public void UpdateDroneToBeAvailable(Drone drone);

        /// <summary>
        /// Sending a drone to charge at the wanted station by creating a new "DroneCharge" entity
        /// containing the drone's and the station's IDs and the time it started to charge,
        /// also decreasing the available spots in the stations
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="station"></param>
        /// <param name="start"></param>
        public void UpdateDroneToBeCharge(Drone drone, Station station, DateTime? start);

        /// <summary>
        /// Updating a parcel to be delivered and recived by the target customer by updating the field "Delivery" time to be the current time
        /// </summary>
        /// <param name="parcel"></param>
        public void UpdateParcelInDelivery(Parcel parcel);

        /// <summary>
        /// Updating that the parcel has been collected by the drone assigned to preform the delivery
        /// by updating the field "PickedUp" time to be the current time
        /// </summary>
        /// <param name="parcel"></param>
        public void UpdateParcelCollected(Parcel parcel);

        /// <summary>
        /// Assigning the drone to preform the delivery of the given parcel, updating the fields in the parcel of "Schedualed" to be current time
        /// and the field of "DroneID" to be the drone's ID
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="drone"></param>
        public void PairParcelToDrone(Parcel parcel, Drone drone);

        /// <summary>
        /// Deleting a parcel from the data
        /// </summary>
        /// <param name="parcel"></param>
        public void UpdateDeleteParcel(Parcel parcel);
        #endregion

        #region Get
        /// <summary>
        /// Returns an IEnumerable contains the parcels saved in the data, filtered by the given predicate
        /// </summary>
        /// <param name="ParcelPredicate"></param>
        /// <returns></returns>
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> ParcelPredicate);

        /// <summary>
        /// Returns an IEnumerable contains the stations saved in the data, filtered by the given predicate
        /// </summary>
        /// <param name="StationPredicate"></param>
        /// <returns>IEnumerable<Parcel></returns>
        public IEnumerable<Station> GetStations(Predicate<Station> StationPredicate);

        /// <summary>
        /// Returns an IEnumerable contains the customers saved in the data, filtered by the given predicate
        /// </summary>
        /// <param name="CustomerPredicate"></param>
        /// <returns>IEnumerable<Station></returns>
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> CustomerPredicate);

        /// <summary>
        /// Returns an IEnumerable contains the drones saved in the data, filtered by the given predicate
        /// </summary>
        /// <param name="DronePredicate"></param>
        /// <returns>IEnumerable<Customer></returns>
        public IEnumerable<Drone> GetDrones(Predicate<Drone> DronePredicate);

        /// <summary>
        /// Returns an IEnumerable contains the charging drones entities saved in the data, filtered by the given predicate
        /// </summary>
        /// <param name="DroneChargePredicate"></param>
        /// <returns>IEnumerable<Drone> </returns>
        public IEnumerable<DroneCharge> GetDroneCharge(Predicate<DroneCharge> DroneChargePredicate);

        /// <summary>
        /// Returns a specific drone from the data based on the given ID number, will throw an exception if the drone doesn't exsits
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IEnumerable<DroneCharge></returns>
        public Drone GetDrone(int id);

        /// <summary>
        /// Returns a specific station from the data based on the given ID number, will throw an exception if the station doesn't exsits
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Drone</returns>
        public Station GetStation(int id);

        /// <summary>
        /// Returns a specific customer from the data based on the given ID number, will throw an exception if the customer doesn't exsits
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Station</returns>
        public Customer GetCustomer(int id);

        /// <summary>
        /// Returns a specific parcel from the data based on the given ID number, will throw an exception if the parcel doesn't exsits
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Customer</returns>
        public Parcel GetParcel(int id);

        /// <summary>
        /// Returns a specific charging drone entity from the data based on the given ID number, will throw an exception if the drone doesn't exsits
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Parcel</returns>
        public DroneCharge GetDroneCharge(int id);
        /// <summary>
        /// Returns a specific battery's property from the data based on the given string value, will throw an exception if the property doesn't exsits
        /// </summary>
        /// <param name="value"></param>
        /// <returns>double battery property</returns>
        public double GetBatteryProperty(string value);

        #endregion
    }
}
