using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using BO;
using DalApi;
using DO;
using static BO.EnumsBL;
using DAL;

namespace BlApi
{
    public partial class BL : IBL
    {
        private DalApi.IDal Data;
        private List<DroneToList> DroneList;
        private Double[] BatteryUsed;
        private static Random rand = new();

        /// <summary>
        /// The cunstrucator will initialize all the drones saved in the data layer, saving them as DroneToList objects and saving them in a list located in BL
        /// </summary>
        public BL()
        {
            Data = DalFactory.GetDal("DalObject");
            DroneList = new List<DroneToList>();
            BatteryUsed = Data.GetBatteryUsed();

            foreach (Drone drone in Data.GetDrones(drone => true))
            {
                DroneToList NewDrone = new();
                Parcel NewParcel = new();
                NewDrone.ID = drone.ID;
                NewDrone.Model = drone.Model;
                NewDrone.MaxWeight = drone.MaxWeight;
                foreach (Parcel parcel in Data.GetParcels(parcel => true))
                {
                    if (parcel.DroneID == drone.ID && parcel.Delivered == null)
                        NewParcel = parcel;
                }
                if (NewParcel.DroneID == drone.ID && NewParcel.Delivered == null)
                    NewDrone = InitDroneInDelivery(NewDrone, NewParcel);
                else
                    NewDrone = InitDroneNOTinDelivery(NewDrone);

                DroneList.Add(NewDrone);
            }
        }

        public List<DroneToList> GetDrones()
        {
            return DroneList;
        }

        public IEnumerable GetAllAvailableStationsID()
        {
            return GetAllAvailableStationsDO().Select(station => station.ID);
        }

        /// <summary>
        /// Initializing the drone to be in a delivery, giving it location and battery according to the status of the delivery.
        /// Location will be either at the station nearest to the sender or at the sender
        /// </summary>
        /// <param name="NewDrone"></param>
        /// <param name="parcel"></param>
        /// <returns>Drone to list object</returns>
        private DroneToList InitDroneInDelivery(DroneToList NewDrone, Parcel parcel)   
        {
            double total;
            Random rand = new();
            NewDrone.Status = DroneStatus.Delivery;    ///Updating drone's status
            NewDrone.ParcelID = parcel.ID;
            Customer sender = Data.GetCustomer(parcel.SenderID), target = Data.GetCustomer(parcel.TargetID);
            Station NearestStatTarget = GetNearestStation(target.Latitude, target.Longitude, Data.GetStations(station => true));
            Station NearestStat = GetNearestStation(sender.Latitude, sender.Longitude, Data.GetStations(station => true));
            if (parcel.PickedUp == null)      ///Checking if the drone already picked up the parcel or not     
                NewDrone.CurrentLocation = new(NearestStat.Latitude, NearestStat.Longitude);
            else    ///means it did get pickedup
                NewDrone.CurrentLocation = new(sender.Latitude, sender.Longitude);

            total = BatteryUsageCurrStation(NewDrone, parcel.SenderID, parcel.TargetID, NearestStat, parcel.Weight); ///Returns the amount of battery needed to complete the delivery
            NewDrone.BatteryStatus = GetRandBatteryStatus(total, 100);  ///Choosing random battery number between the minimum needed to complete the delivery and full battery
            return NewDrone;
        }

        private DroneToList InitDroneNOTinDelivery(DroneToList NewDrone)
        {
            Random rand = new Random();
            Station nearest;
            int RandomStation, RandomCustomer;
            DroneStatus option = (DroneStatus)rand.Next(0, 2);     ///Random status, Available or Charging
            List<Station> AllAvailableStations = GetAllAvailableStationsDO();
            List<Customer> AllPastCustomers = GetPastCustomers().ToList();
            if (AllPastCustomers.Count() == 0)                      ///We are assuming that the odds that there are no available stations are very unlikley
                option = DroneStatus.Charging;
            switch (option)
            {
                case DroneStatus.Available:
                    NewDrone.Status = DroneStatus.Available;
                    RandomCustomer = rand.Next(0, AllPastCustomers.Count());
                    NewDrone.CurrentLocation.Latitude = AllPastCustomers[RandomCustomer].Latitude;
                    NewDrone.CurrentLocation.Longitude = AllPastCustomers[RandomCustomer].Longitude;
                    nearest = GetNearestStation(NewDrone.CurrentLocation.Latitude, NewDrone.CurrentLocation.Longitude, Data.GetStations(station => true));
                    NewDrone.BatteryStatus = RandBatteryToStation(NewDrone, new Location(nearest.Latitude, nearest.Longitude), BatteryUsed[0]);
                    break;
                case DroneStatus.Charging:
                    NewDrone.Status = DroneStatus.Charging;
                    RandomStation = rand.Next(0, AllAvailableStations.Count());
                    Data.DroneToBeCharge(NewDrone.ID, AllAvailableStations[RandomStation].ID, DateTime.Now);
                    NewDrone.CurrentLocation.Latitude = AllAvailableStations[RandomStation].Latitude;
                    NewDrone.CurrentLocation.Longitude = AllAvailableStations[RandomStation].Longitude;
                    NewDrone.BatteryStatus = GetRandBatteryStatus(0, 21);
                    break;
            }
            return NewDrone;
        }

    }
}

