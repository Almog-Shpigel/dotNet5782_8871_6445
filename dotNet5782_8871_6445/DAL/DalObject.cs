using IDAL;
using IDAL.DO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DalObject
{
    public class DalObject :IDal
    {
        public DalObject()
        {
            DataSource.Initialize();
        }
        #region Add
        public void AddNewStation(Station station)
        {
            if (StationExist(station.ID))
                throw new StationExistException("The station ID exists already in the data!!");
            station.Name = "Station " + station.Name;
            DataSource.stations.Add(station);
        }

        public void AddNewCustomer(Customer customer)
        {
            if(CustomerExist(customer.ID))
                throw new CustomerExistException("The customer ID exists already in the data!!");
            DataSource.customers.Add(customer);
        }

        public void AddNewParcel(Parcel parcel)
        {
            if (!CustomerExist(parcel.SenderID))
                throw new CustomerExistException("The sender ID dosen't exists in the data!!");
            if (!CustomerExist(parcel.TargetID))
                throw new CustomerExistException("The target ID dosen't exists in the data!!");
            parcel.ID = ++DataSource.Config.ParcelsCounter;
            DataSource.parcels.Add(parcel);
        }

        public void AddNewDrone(Drone drone,int StationID)
        {
            if (DroneExist(drone.ID))
                throw new DroneExistException("The drone ID exists already in the data!!");
            if (!StationExist(StationID))
                throw new StationExistException("The station doesn't exists in the data!!");
            DataSource.drones.Add(drone);
            DroneToBeCharge(drone.ID, StationID, DateTime.Now);
        }
        #endregion

        #region Update
        public void UpdateDroneName(int id, string model)
        {
            Drone drone = GetDrone(id);
            drone.Model = model;
            for (int i = 0; i < DataSource.drones.Count; i++)
            {
                if (DataSource.drones[i].ID == drone.ID)
                {
                    DataSource.drones[i] = drone;
                    break;
                } 
            }
        }
        public void UpdateStationSlots(int stationID,int slots)
        {
            Station station = GetStation(stationID);
            station.ChargeSlots = slots;
            for (int i = 0; i < DataSource.stations.Count; i++)
            {
                if (DataSource.stations[i].ID == station.ID)
                    DataSource.stations[i] = station;
            }
        }
        public void UpdateStationName(int stationID, string name)
        {
            Station station = GetStation(stationID);
            station.Name = name;
            for (int i = 0; i < DataSource.stations.Count; i++)
            {
                if (DataSource.stations[i].ID == station.ID)
                    DataSource.stations[i] = station;
            }
        }
        public void UpdateCustomerName(int id, string name)
        {
            Customer customer = GetCustomer(id);
            customer.Name = name;
            for (int i = 0; i < DataSource.customers.Count; i++)
            {
                if (DataSource.customers[i].ID == customer.ID)
                    DataSource.customers[i] = customer;
            }
        }
        public void UpdateCustomerPhone(int id, int phone)
        {
            Customer customer = GetCustomer(id);
            customer.Phone ='0'+ phone.ToString();
            for (int i = 0; i < DataSource.customers.Count; i++)
            {
                if (DataSource.customers[i].ID == customer.ID)
                    DataSource.customers[i] = customer;
            }
        }
        public void DroneAvailable(int DroneID)
        {
            if(!DroneExist(DroneID))
                    throw new DroneExistException("The drone dosen't exists in the data!!");
            DroneCharge droneCharge = new();
            for (int j = 0; j < DataSource.DroneCharges.Count; ++j)        ///Going through the array to find the wanted DroneCharged object
                if(DataSource.DroneCharges[j].DroneID == DroneID)
                    droneCharge = DataSource.DroneCharges[j];
            Station NewStation;
            DataSource.DroneCharges.Remove(droneCharge);
            for (int i = 0; i < DataSource.stations.Count; ++i)              ///Going through the array to find the wanted station the drone was charged in
                if(DataSource.stations[i].ID != droneCharge.StationID)
                {
                    NewStation = DataSource.stations[i];
                    NewStation.ChargeSlots++;
                    DataSource.stations[i] = NewStation;                        ///Freeing a space for other drones
                }
        }


        public void DroneToBeCharge(int DroneID, int StationID, DateTime start)
        {
            if (!DroneExist(DroneID))
                throw new DroneExistException("The drone dosen't exists in the data!!");

            if (!StationExist(StationID))
                throw new StationExistException("The station dosen't exists in the data!!");

            if (DroneIsCharging(DroneID))
                throw new DroneExistException("The drone is already being charged ");
            int i = 0;
            DroneCharge NewCharge = new(DroneID, StationID, start);
            DataSource.DroneCharges.Add(NewCharge);
            while (DataSource.stations[i].ID != StationID)      ///Finding the wanted station
                ++i;
            Station NewStation = DataSource.stations[i];
            NewStation.ChargeSlots--;
            DataSource.stations[i] = NewStation;                ///one slot was taken by the drone we chose
        }

        private bool DroneIsCharging(int droneID)
        {
            foreach (DroneCharge drone in GetAllDronesCharge())
                if (drone.DroneID == droneID)
                    return true;
            return false;
        }

        public void ParcelDelivery(int ParcelID)
        {
            if (!ParcelExist(ParcelID))
                throw new DroneExistException("The parcel dosen't exists in the data!!");
            int i = 0;
            while (DataSource.parcels[i].ID != ParcelID) ///Finding the wanted parcel
                ++i;
            Parcel NewParcel = DataSource.parcels[i];
            NewParcel.Delivered = DateTime.Now; ///Changing the time of the parcel to update it's been delivered now
            DataSource.parcels[i] = NewParcel;
        }

        public void ParcelCollected(int ParcelID)
        {
            if (!ParcelExist(ParcelID))
                throw new DroneExistException("The parcel dosen't exists in the data!!");

            int i = 0;
            while (DataSource.parcels[i].ID != ParcelID) ///Searching for the wanted parcel
                ++i;
            Parcel NewParcel = DataSource.parcels[i];
            NewParcel.PickedUp = DateTime.Now; 
            DataSource.parcels[i] = NewParcel; ///Updating the time of the pickup by the drone
        }

        public void PairParcelToDrone(int ParcelID,int DroneID)
        {
            if (!ParcelExist(ParcelID))
                throw new DroneExistException("The parcel dosen't exists in the data!!");

            if (!DroneExist(DroneID))
                throw new DroneExistException("The drone dosen't exists in the data!!");

            int i = 0;
            while (DataSource.parcels[i].ID != ParcelID) ///Searching for the wanted parcel
                ++i;
            Parcel NewParcel = DataSource.parcels[i];
            NewParcel.PickedUp = DateTime.Now;
            NewParcel.DroneID = DroneID;                ///Pairing the parcel with the ID of the drone chose to take it
            NewParcel.Scheduled = DateTime.Now;         ///Updating the scheduled time for the parcel
            DataSource.parcels[i] = NewParcel;
        }

        public double DistanceFromStation(double x1, double y1, int StationID)
        {
            int i = 0;
            double x2, y2;

            while (StationID != DataSource.stations[i].ID)
                ++i;

            x1 = (x1 * Math.PI) / 180;
            y1 = (y1 * Math.PI) / 180;
            x2 = DataSource.stations[i].Longitude;
            x2 = (x2 * Math.PI) / 180;
            y2 = DataSource.stations[i].Latitude;
            y2 = (y2 * Math.PI) / 180;
            double result1 =  Math.Pow(Math.Sin((x2 - x1) / 2), 2) + Math.Cos(y1) * Math.Cos(y2) * Math.Pow(Math.Sin((y2 - y1) / 2), 2);
            double result2 = 2 * Math.Asin(Math.Sqrt(result1));
            double radius = 3956;
            return (result2 * radius);
        }

        public double DistanceFromCustomer(double x1, double y1, int CustomerID)
        {
            int i = 0;
            double x2, y2;

            while (CustomerID != DataSource.customers[i].ID)
                ++i;

            x1 = (x1 * Math.PI) / 180;
            y1 = (y1 * Math.PI) / 180;
            x2 = DataSource.customers[i].Longitude;
            x2 = (x2 * Math.PI) / 180;
            y2 = DataSource.customers[i].Latitude;
            y2 = (y2 * Math.PI) / 180;
            double result1 = Math.Pow(Math.Sin((x2 - x1) / 2), 2) + Math.Cos(y1) * Math.Cos(y2) * Math.Pow(Math.Sin((y2 - y1) / 2), 2);
            double result2 = 2 * Math.Asin(Math.Sqrt(result1));
            double radius = 3956;
            return (result2 * radius);
        }
        #endregion

        #region Display
        //public string DisplayDrone(int DroneID)
        //{
        //    int i = 0;
        //    while (DataSource.drones[i].ID != DroneID)  ///Going through the array to find the wanted drone
        //        ++i;
        //    return DataSource.drones[i].ToString();
        //}

        //public string DisplayCustomer(int CustomerID)
        //{
        //    int j = 0;
        //    while (DataSource.customers[j].ID != CustomerID) ///Going through the array to find the wanted customer
        //        ++j;
        //    return DataSource.customers[j].ToString();
        //}

        //public string DisplayParcel(int ParcelID)
        //{
        //    int j = 0;
        //    while (DataSource.parcels[j].ID != ParcelID) ///Going through the array to find the wanted parcel
        //        ++j;
        //    return DataSource.parcels[j].ToString();
        //}

        //public string DisplayStation(int StationID)
        //{
        //    int j = 0;
        //    while (DataSource.stations[j].ID != StationID) ///Going through the array to find the wanted station
        //        ++j;
        //    return DataSource.stations[j].ToString();
        //}
        #endregion

        #region Print
        //public IEnumerable<string> PrintAllStations()
        //{
        //    List <string> StationsList = new List<string>();
        //    for (int i = 0; i < DataSource.stations.Count(); i++)
        //        StationsList.Add(DataSource.stations[i].ToString());
        //    return StationsList;
        //}
        //public IEnumerable<string> PrintAllDrones()
        //{
        //    List<string> DronesList = new List<string>();
        //    for (int i = 0; i < DataSource.drones.Count(); i++)
        //        DronesList.Add(DataSource.drones[i].ToString());
        //    return DronesList;
        //}
        //public IEnumerable<string> PrintAllCustomers()
        //{
        //    List<string> CustomerList = new List<string>();
        //    for (int i = 0; i < DataSource.customers.Count(); i++)
        //        CustomerList.Add(DataSource.customers[i].ToString());
        //    return CustomerList;
        //}
        //public IEnumerable<string> PrintAllParcels()
        //{
        //    List<string> ParcelsList = new List<string>();
        //    for (int i = 0; i < DataSource.parcels.Count(); i++)
        //        ParcelsList.Add(DataSource.parcels[i].ToString());
        //    return ParcelsList;
        //}
        //
        //}

        //public IEnumerable<string> PrintAllAvailableStations()
        //{
        //    List<string> AvailableStationsList = new List<string>();
        //    for (int i = 0; i < DataSource.stations.Count(); i++) 
        //        if (DataSource.stations[i].ChargeSlots > 0)
        //            AvailableStationsList.Add(DataSource.stations[i].ToString());                
        //    return AvailableStationsList;
        //}
        #endregion Print

        #region Get
        public IEnumerable<Parcel> GetAllParcels()
        {
            return DataSource.parcels;
        }

        public IEnumerable<Station> GetAllStations()
        {
            return DataSource.stations;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return DataSource.customers;
        }

        public IEnumerable<Drone> GetAllDrones()
        {
            return DataSource.drones;
        }

        public IEnumerable<DroneCharge> GetAllDronesCharge()
        {
            return DataSource.DroneCharges;
        }
        public IEnumerable<Parcel> GetAllAvailableParcels()
        {
            List<Parcel> AvailabledParcelsList = new List<Parcel>();
            for (int i = 0; i < DataSource.parcels.Count(); i++)
                if (DataSource.parcels[i].DroneID == 0) ///If DroneID is 0 it means the parcel hasn't been assigned yet
                    AvailabledParcelsList.Add(DataSource.parcels[i]);
            return AvailabledParcelsList;
        }
        public Drone GetDrone(int id)
        {
            foreach (Drone drone in DataSource.drones)
            {
                if (drone.ID == id)
                    return drone;
            }
            throw new DroneExistException("The drone dosen't exists in the data!!");
        }

        public Station GetStation(int id)
        {
            foreach (Station station in DataSource.stations)
            {
                if (station.ID == id)
                    return station;
            }
            throw new StationExistException("The station dosen't exists in the data!!");
         
        }

        public Customer GetCustomer(int id)
        {
            foreach (Customer customer in DataSource.customers)
            {
                if (customer.ID == id)
                    return customer;
            }
            throw new CustomerExistException("The customer doesn't exists in the data!!");
        }
        public DroneCharge GetDroneCharge(int id)
        {
            foreach (DroneCharge drone in DataSource.DroneCharges)
            {
                if (drone.DroneID == id)
                    return drone;
            }
            throw new DroneExistException("Drone in charge not exist!");
        }
        public Parcel GetParcel(int id)
        {
            foreach (Parcel parcel in DataSource.parcels)
            {
                if (parcel.ID == id)
                    return parcel;
            }
            throw new ParcelExistException("Parcel not exist!");
        }

        public double[] GetBatteryUsed()
        {
            double[] BatteryUsed = new double[5];
            BatteryUsed[0] = DataSource.Config.Empty;
            BatteryUsed[1] = DataSource.Config.LightWight;
            BatteryUsed[2] = DataSource.Config.MediumWight;
            BatteryUsed[3] = DataSource.Config.HaevyWight;
            BatteryUsed[4] = DataSource.Config.ChargeRate;
            return BatteryUsed;
        }
        
        internal bool DroneExist(int id)
        {
            foreach (Drone drone in DataSource.drones)
                if (drone.ID == id)
                    return true;
            return false;
        }

        internal bool StationExist(int id)
        {
            foreach (Station station in DataSource.stations)
                if (station.ID == id)
                    return true;
            return false;
        }

        internal bool CustomerExist(int id)
        {
            foreach (Customer customer in DataSource.customers)
                if (customer.ID == id)
                    return true;
            return false;
        }

        internal bool ParcelExist(int id)
        {
            foreach (Parcel parcel in DataSource.parcels)
                if (parcel.ID == id)
                    return true;
            return false;
        }
        #endregion
    }
}
