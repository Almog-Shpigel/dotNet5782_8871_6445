using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DO;
using static BO.EnumsBL;

namespace BlApi
{
    public interface IBL
    {
        #region Add
        /// <summary>
        /// Adding new customer to the data, checking if the id is 9 digits,has a valid phone number that starts with "05" and contain 10 digits,and his location is within Jerusalme area
        /// </summary>
        /// <param name="customer"></param>
        public void AddNewCustomer(CustomerBL customer);

        /// <summary>
        /// Adding a new station and saving it in the data after checking the id is 6 digits, the charge slots are positive number or 0, and the location is inside Jerusalem area
        /// </summary>
        /// <param name="StationBO"></param>
        public void AddNewStation(StationBL station);


        /// <summary>
        /// Adding a new drone and saving it in the data and in BL after checking the id is 6 digits and station it was send to has free charge slots for it
        /// </summary>
        /// <param name="DroneBL"></param>
        /// <param name="StationID"></param>
        public void AddNewDrone(DroneBL drone, int stationID);

        /// <summary>
        /// Adding new parcel to the data, checking if the sender and the target IDs are valid ( contain 9 digits)
        /// </summary>
        /// <param name="parcel"></param>
        public void AddNewParcel(ParcelBL parcel);
        #endregion

        #region Update
        /// <summary>
        /// Updating a drone's model
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        public void UpdateDroneName(int droneID, string newName);

        /// <summary>
        /// Updating a station's name
        /// </summary>
        /// <param name="StationID"></param>
        /// <param name="NewStationName"></param>
        void UpdateStationName(int stationID, string newStationName);

        /// <summary>
        /// Updating a station's total charging slots
        /// </summary>
        /// <param name="StationID"></param>
        /// <param name="NewStationName"></param>
        void UpdateStationSlots(int stationID, int newNumberSlots , int currentlyCharging);
        /// <summary>
        /// Updating a customer's name
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="NewCustomerName"></param>

        public void UpdateCustomerName(int customerID, string newCustomerName);

        /// <summary>
        /// Updating a customer's phone number
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <param name="NewCustomerPhone"></param>
        public void UpdateCustomerPhone(int customerID, int newCustomerPhone);

        /// <summary>
        /// Pairing a parcel to the wanted drone, the parcel will be selected by the next algorithem:
        /// highest priority, then weight similler to the drone propertie, then the distance between the parcel and the drone.
        /// all in consider the parcel isn't paired already with a different drone
        /// and the drone's battery is able to collect the parcel,deliver it and return to a near by station.
        /// </summary>
        /// <param name="DroneID"></param>
        public void UpdateParcelAssignToDrone(int droneID);

        /// <summary>
        /// Updating that the parcel was delivered to the target by the drone carrying it, updating the drone's location 
        /// to be at the target customer, substracting the battery used from the last location and updating the drone's status to be
        /// available again
        /// </summary>
        /// <param name="DroneID"></param>
        public void UpdateParcelDeleiveredByDrone(int droneID);


        /// <summary>
        /// Sending the wanted drone to charge at the nearest station that has free charge slots
        /// </summary>
        /// <param name="DroneID"></param>
        public void UpdateDroneToBeCharged(int droneID);

        /// <summary>
        /// Updating a parcel status to be collected by the drone paired to her. also updating the drone's location
        /// to be at that target and the battery to substract according to the distance from it's last location
        /// </summary>
        /// <param name="DroneID"></param>
        public void UpdateParcelCollectedByDrone(int droneID);

        /// <summary>
        /// Relasing a drone that is currently charging, changing it's status to be available and deleting the "drone in charge" entity 
        /// saved in the data layer
        /// </summary>
        /// <param name="DroneID"></param>
        public void UpdateDroneToBeAvailable(int droneID);

        /// <summary>
        /// Deleting a parcel from the data
        /// </summary>
        /// <param name="id"></param>

        public void UpdateDeleteParcel(int id);

        /// <summary>
        /// Creating a thread that starts a simulator for the drone
        /// </summary>
        /// <param name="droneID"></param>
        /// <param name="updateView"></param>
        /// <param name="checkIfCanceled"></param>

        public void UpdateDroneSimulatorStart(int droneID, Action updateView, Func<bool> checkIfCanceled);

        #endregion

        #region Get
        #region Get All
        /// <summary>
        /// Function that recive all the drones from the data and convert them into "DroneToLIst" entity and returns the new IEnumarble
        /// </summary>
        /// <returns>IEnumerable<DroneToList></returns>
        public IEnumerable<DroneToList> GetAllDrones();

        /// <summary>
        /// Function that recive all the parcels from the data and convert them into "ParcelToLIst" entity and returns the new IEnumarble
        /// </summary>
        /// <returns>IEnumerable<ParcelToList></returns>
        public IEnumerable<ParcelToList> GetAllParcels();

        /// <summary>
        /// Function that recive all the stations from data and create a list of StationToList and send it to print in console
        /// </summary>
        /// <returns>IEnumerable<StationToList></returns>
        public IEnumerable<StationToList> GetAllStations();

        /// <summary>
        /// Function that recive all the customers from data and create a list of CustomerToList and send it to print in console
        /// </summary>
        /// <returns>IEnumerable<CustomerToList></returns>
        public IEnumerable<CustomerToList> GetAllCustomers();

        /// <summary>
        /// Returns all the stations from the data
        /// </summary>
        /// <returns>IEnumerable<Station></returns>
        public IEnumerable<Station> GatAllStationsDO();

        /// <summary>
        /// Recive a ParcelAtCustomer entity and returns the matching ParcelToList entity
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns>ParcelToList</returns>
        ParcelToList GetParcelToList(ParcelAtCustomer parcel);
        #endregion

        #region Get some

        /// <summary>
        /// Returns all the CustomerInParcel entity
        /// </summary>
        /// <returns>IEnumerable<CustomerInParcel></returns>
        public IEnumerable<CustomerInParcel> GetAllCustomerInParcels();
        /// <summary>
        /// Function that return all the station who has more than 0 charge slots available
        /// </summary>
        /// <returns>list of available stations</returns>
        public IEnumerable<StationToList> GetAvailableStations();
        
        /// <summary>
        /// Returns all the parcels grouped according to the paremeters sent
        /// </summary>
        /// <param name="groupByString"></param>
        /// <returns>Grouped parcels by sender\recevier</returns>
        public IEnumerable<ParcelToList> GetParcelsGroupBy(string groupByString);

        /// <summary>
        /// Returns a list of all the droneToList entities filtered by the wanted status and\or weight
        /// </summary>
        /// <param name="status"></param>
        /// <param name="weight"></param>
        /// <returns>Filtered list of DroneToList</returns>
        public IEnumerable<DroneToList> GetDrones(DroneStatus status, WeightCategories weight);

        /// <summary>
        /// Returns all the parcels filtered by priority/wight/status and time created
        /// </summary>
        /// <param name="priorities"></param>
        /// <param name="weight"></param>
        /// <param name="status"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns>Filtered list of ParcelToList</returns>

        public IEnumerable<ParcelToList> GetParcels(Priorities priorities, WeightCategories weight,ParcelStatus status, DateTime? from, DateTime? to);
        #endregion

        #region Get one
        /// <summary>
        /// Receiving a drone id and converting the drone to a DroneBL and print it
        /// </summary>
        /// <param name="DroneID"></param>
        /// <returns>DroneBL to print</returns>
        public DroneBL GetDrone(int droneID);

        /// <summary>
        /// Receive parcel id and convert it to ParcelBL entity and send it to print
        /// </summary>
        /// <param name="ParcelID"></param>
        /// <returns>ParcelBL entity</returns>
        public ParcelBL GetParcel(int parcelID);

        /// <summary>
        /// Receiving a station id and converting the station to a StationBL and print it
        /// </summary>
        /// <param name="StationID"></param>
        /// <returns>StationBL to print</returns>
        public StationBL GetStation(int stationID);

        /// <summary>
        /// Receiving a customer id and converting it to CustomerBL and sends it to print
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns>CustomerBL to print</returns>
        public CustomerBL GetCustomer(int customerID);
        #endregion
        #endregion
    }
}
