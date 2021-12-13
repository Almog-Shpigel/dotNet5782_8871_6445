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
        /// <summary>
        /// Function that recive all the stations from data and create a list of StationToList and send it to print in console
        /// </summary>
        /// <returns></returns>
        public List<StationToList> DispalyAllStations();
        public List<DroneToList> GetDrones();
        public List<DroneToList> GetDrones(DroneStatus status, WeightCategories weight);

        /// <summary>
        /// Returns the list of drones saved in BL
        /// </summary>
        /// <returns></returns>
        //public List<DroneToList> GetDrones(drone => true);;
        /// <summary>
        /// Function that recive all the customers from data and create a list of CustomerToList and send it to print in console
        /// </summary>
        /// <returns></returns>
        public List<CustomerToList> DispalyAllCustomers();
        /// <summary>
        /// Function that recive all the parcels from data and create a list of ParcelToList and send it to print in console
        /// </summary>
        /// <returns></returns>
        public List<ParcelToList> DispalyAllParcels();
        /// <summary>
        /// Function that returns all the unassigned parcels by going through the parcels and creating a new list out of the ones that doesn't have a drone paired to them
        /// </summary>
        /// <returns></returns>
        public List<ParcelToList> DispalyAllUnassignedParcels();
        /// <summary>
        /// Function that return all the station who has more than 0 charge slots available
        /// </summary>
        /// <returns>list of available stations</returns>
        public List<StationToList> GetAllAvailableStationsToList();
        /// <summary>
        /// Receiving a station id and converting the station to a StationBL and print it
        /// </summary>
        /// <param name="StationID"></param>
        /// <returns>StationBL to print</returns>
        public StationBL DisplayStation(int StationID);
        /// <summary>
        /// Receiving a drone id and converting the drone to a DroneBL and print it
        /// </summary>
        /// <param name="DroneID"></param>
        /// <returns>DroneBL to print</returns>
        public DroneBL DisplayDrone(int DroneID);
        /// <summary>
        /// Receiving a customer id and converting it to CustomerBL and sends it to print
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns>CustomerBL to print</returns>
        public CustomerBL DisplayCustomer(int CustomerID);
        /// <summary>
        /// Receive parcel id and convert it to ParcelBL entity and send it to print
        /// </summary>
        /// <param name="ParcelID"></param>
        /// <returns>ParcelBL entity</returns>
        public ParcelBL DisplayParcel(int ParcelID);
        /// <summary>
        /// Returns the distance between a point and a specific station
        /// </summary>
        /// <param name="longitude1"></param>
        /// <param name="latitude1"></param>
        /// <param name="StationID"></param>
        /// <returns>distance (km)</returns>
        public string DisplayDistanceFromStation(double longitude1, double latitude1, int StationID);
        /// <summary>
        /// Returns the distance between a point and a specific customer
        /// </summary>
        /// <param name="longitude1"></param>
        /// <param name="latitude1"></param>
        /// <param name="CustomerID"></param>
        /// <returns>distance (km)</returns>
        public string DisplayDistanceFromCustomer(double longitude1, double latitude1, int CustomerID);
        /// <summary>
        /// Updating a drone's model
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        public void UpdateDroneName(int DroneID, string NewName);
        /// <summary>
        /// Updating a station's charge slots or name or both, if an empty input recived for either, will not change it
        /// </summary>
        /// <param name="StationID"></param>
        /// <param name="ChangeName"></param>
        /// <param name="ChangeSlots"></param>
        /// <param name="name"></param>
        /// <param name="slots"></param>
        public void UpdateStation(int StationID, bool ChangeName, bool ChangeSlots, string name, int slots);
        /// <summary>
        /// Updating a customer's phone number or name or both, if an empty input recived for either, will not change it
        /// </summary>
        /// <param name="id"></param>
        /// <param name="changeName"></param>
        /// <param name="changePhone"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        public void UpdateCustomer(int id, bool changeName, bool changePhone, string name, int phone);
        /// <summary>
        /// Pairing a parcel to the wanted drone, the parcel will be selected by the next algorithem:
        /// highest priority, then weight similler to the drone propertie, then the distance between the parcel and the drone.
        /// all in consider the parcel isn't paired already with a different drone
        /// and the drone's battery is able to collect the parcel,deliver it and return to a near by station.
        /// </summary>
        /// <param name="DroneID"></param>
        public void UpdateParcelAssignToDrone(int DroneID);
        /// <summary>
        /// Updating that the parcel was delivered to the target by the drone carrying it, updating the drone's location 
        /// to be at the target customer, substracting the battery used from the last location and updating the drone's status to be
        /// available again
        /// </summary>
        /// <param name="DroneID"></param>
        public void UpdateParcelDeleiveredByDrone(int DroneID);
        /// <summary>
        /// Sending the wanted drone to charge at the nearest station that has free charge slots
        /// </summary>
        /// <param name="DroneID"></param>
        public void UpdateDroneToBeCharged(int DroneID);
        /// <summary>
        /// Updating a parcel status to be collected by the drone paired to her. also updating the drone's location
        /// to be at that target and the battery to substract according to the distance from it's last location
        /// </summary>
        /// <param name="DroneID"></param>
        public void UpdateParcelCollectedByDrone(int DroneID);
        /// <summary>
        /// Relasing a drone that is currently charging, changing it's status to be available and deleting the "drone in charge" entity 
        /// saved in the data layer
        /// </summary>
        /// <param name="DroneID"></param>
        public void UpdateDroneAvailable(int DroneID);
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
        public void AddNewDrone(DroneBL drone, int StationID);
        /// <summary>
        /// Adding new parcel to the data, checking if the sender and the target IDs are valid ( contain 9 digits)
        /// </summary>
        /// <param name="parcel"></param>
        public void AddNewParcel(ParcelBL parcel);
        public IEnumerable GetAllAvailableStationsID();
    }
}
