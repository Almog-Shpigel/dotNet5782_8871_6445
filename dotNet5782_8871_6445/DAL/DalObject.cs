using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DalObject
    {
        public DalObject()
        {
            DataSource.Initialize();
        }
        #region Add
        public void AddNewStation(double longitude, double latitude,int ChargeSlots)
        {
            int id = 122000 + ++DataSource.config.StationCounter;        ///Every ID number will be 6 digits starting with 12200 (changeable)
            string name = "Station " + DataSource.config.StationCounter; ///Marking the stations according to the counter, also changable
            ///Adding new object to the array
            DataSource.stations[DataSource.config.StationCounter - 1] = new IDAL.DO.Station(id, name, ChargeSlots, longitude, latitude);
        }
        public void AddNewCustomer(int id, string name,string phone,double longitude,double latitude)
        {
            ///Adding new object to the array
            DataSource.customers[DataSource.config.CustomerCounter++] = new IDAL.DO.Customer(id, name, phone, longitude, latitude);
        }
        public void AddNewParcel(int sender,int target,IDAL.DO.WeightCategories weight, IDAL.DO.Priorities priority)
        {
            int id = 344000 + ++DataSource.config.ParcelsCounter;    ///Every parcel id will begin with 34400(changeable) and will be 6 digits
            int DroneID = 0;
            ///Setting all the times to be dufault untill they will recive any updates
            DateTime TimeRequested = DateTime.Now,
                        Scheduled = DateTime.MaxValue,
                        PickedUp = DateTime.MaxValue,
                        Delivered = DateTime.MaxValue;
            ///Adding new object to the array
            DataSource.parcels[DataSource.config.ParcelsCounter - 1] = new IDAL.DO.Parcel(id, sender, target, DroneID, weight, priority, TimeRequested, Scheduled, PickedUp, Delivered);
        }
        public void AddNewDrone(string model,IDAL.DO.WeightCategories weight)
        {
            int id = 669000 + ++DataSource.config.DroneCounter;      ///Every drone id will begin with 66900(changeable) and will be 6 digits
            model += " " + DataSource.config.DroneCounter;
            ///Adding new object to the array
            DataSource.drones[DataSource.config.DroneCounter - 1] = new IDAL.DO.Drone(id, model, weight, DroneStatuses.Available, 100);
        }
        #endregion
        #region Update
        public void DroneAvailable(int DroneID)
        {
            int j = 0;
            while (DataSource.drones[j].ID != DroneID) ///Going through the array to find the wanted drone
                ++j;
            DataSource.drones[j].Status = DroneStatuses.Available;           ///Changing the status of the drone 
        }
        public void DroneToBeCharge(int DroneID, int station)
        {
            int i = 0, j = 0;
            while (DataSource.drones[j].ID != DroneID && j <= DataSource.config.DroneCounter)    ///Finding the wanted drone
                ++j;
            DroneCharge charge = new DroneCharge(DataSource.drones[j].ID, station);
            station %= 1000;                                                                    /// Assuming we have less than 1000 stations because our ID is 122xxx
            --DataSource.stations[station].ChargeSlots;
            ///Checking to see if our drone was in the middle of a delivery
            if (DataSource.drones[j].Status == DroneStatuses.Delivery)                          ///Checking to see if our drone was in the middle of a delivery
            {
                ///If so, searching the parcel that paired with this drone so we can unassign them
                while (DataSource.parcels[i].DroneID != DroneID && i <= DataSource.config.ParcelsCounter)
                    ++i;
                DataSource.parcels[i].DroneID = 0;
            }
            DataSource.drones[j].Status = DroneStatuses.Charging;       ///Changing the drone status
        }
        public void ParcelDeleivery(int idNum)
        {
            int i = 0;
            while (DataSource.parcels[i].ID != idNum && i <= DataSource.config.ParcelsCounter) ///Finding the wanted parcel
                ++i;
            DataSource.parcels[i].Delivered = DateTime.Now;         ///Changing the time of the parcel to update it's been delivered now
        }
        public void ParcelCollected(int id)
        {
  
            int i = 0, j = 0;
            while (DataSource.parcels[i].ID != id && i <= DataSource.config.ParcelsCounter) ///Searching for the wanted parcel
                ++i;        
            int DroneID = DataSource.parcels[i].DroneID;
            while (DataSource.drones[j].ID != DroneID && j <= DataSource.config.DroneCounter)
                ++j;
            DataSource.parcels[i].PickedUp = DateTime.Now; ///Updating the time of the pickup by the drone
        }
        public void PairParcelToDrone(int ParcelID)
        {
            int i = 0, j = 0;
            while (DataSource.parcels[i].ID != ParcelID && i <= DataSource.config.ParcelsCounter) ///Searching for the wanted parcel
                ++i;
            while (j <= DataSource.config.DroneCounter &&
                    (DataSource.drones[j].Status != DroneStatuses.Available ||
                    (int)DataSource.parcels[i].Weight > (int)DataSource.drones[j].MaxWeight))
            { ///Searching a drone that is available and also can carry the parcel according to his max wight category
                ++j;
            }
             DataSource.parcels[i].DroneID = DataSource.drones[j].ID;    ///Pairing the parcel with the ID of the drone chose to take it
             DataSource.drones[j].Status = DroneStatuses.Delivery;       ///Changing the status of the drone
             DataSource.parcels[i].Scheduled = DateTime.Now;             ///Updating the scheduled time for the parcel
             
        }
        #endregion
        #region Display
        public string DisplayDrone(int DroneID)
        {
            int i = 0;
            while (DataSource.drones[i].ID != DroneID)  ///Going through the array to find the wanted drone
                ++i;
            return DataSource.drones[i].ToString();
        }

        public string DisplayCustomer(int CustomerID)
        {
            int j = 0;
            while (DataSource.customers[j].ID != CustomerID) ///Going through the array to find the wanted customer
                ++j;
            return DataSource.customers[j].ToString();
        }

        public string DisplayParcel(int ParcelID)
        {
            int j = 0;
            while (DataSource.parcels[j].ID != ParcelID) ///Going through the array to find the wanted parcel
                ++j;
            return DataSource.parcels[j].ToString();
        }

        public string DisplayStation(int StationID)
        {
            int j = 0;
            while (DataSource.stations[j].ID != StationID) ///Going through the array to find the wanted station
                ++j;
            return DataSource.stations[j].ToString();
        }
        #endregion
        #region Print
        public string[] PrintAllStations()
        {
            string[] StationsStrings = new string[DataSource.config.StationCounter];
            for (int i = 0; i < DataSource.config.StationCounter; i++)
                StationsStrings[i] = DataSource.stations[i].ToString();

            return StationsStrings;
        }
        public string[] PrintAllDrones()
        {
            string[] DronesStrings = new string[DataSource.config.DroneCounter];
            for (int i = 0; i < DataSource.config.DroneCounter; i++)
                DronesStrings[i] = DataSource.drones[i].ToString();

            return DronesStrings;
        }
        public string[] PrintAllCustomers()
        {
            string[] CustomersStrings = new string[DataSource.config.CustomerCounter];
            for (int i = 0; i < DataSource.config.CustomerCounter; i++)
                CustomersStrings[i] = DataSource.customers[i].ToString();

            return CustomersStrings;
        }
        public string[] PrintAllParcels()
        {
            string[] ParcelsStrings = new string[DataSource.config.ParcelsCounter];
            for (int i = 0; i < DataSource.config.ParcelsCounter; i++)
                ParcelsStrings[i] = DataSource.parcels[i].ToString();

            return ParcelsStrings;
        }
        public string[] PrintAllUnassignedParcels()
        {
            string[] parcels = new string[DataSource.config.ParcelsCounter];
            for (int i = 0, j = 0; i < DataSource.config.ParcelsCounter; i++)
                if (DataSource.parcels[i].DroneID == 0) ///If DroneID is 0 it means the parcel hasn't been assigned yet
                {
                    parcels[j] = DataSource.parcels[i].ToString();
                    ++j;
                }
            return parcels;
        }
        public string[] PrintAllAvailableStations()
        {
            string[] stations = new string[DataSource.config.StationCounter];
            for (int i = 0, j = 0; i < DataSource.config.StationCounter; i++) 
                if (DataSource.stations[i].ChargeSlots > 0)
                {
                    stations[j] = DataSource.stations[i].ToString();
                    ++j;
                }

            return stations;
        }
        #endregion Print
    }
}
