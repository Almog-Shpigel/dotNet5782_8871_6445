using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BO;
using DO;
using static BO.EnumsBL;
using DAL;

namespace BlApi
{
    public partial class BL : IBL
    {
        internal static readonly BL instance = new();

        static BL() { }

        /// <summary>
        /// The cunstrucator will initialize all the drones saved in the data layer, saving them as DroneToList objects and saving them in a list located in BL
        /// </summary>
        public static BL Instance { get => instance; }

        private DalApi.IDal Data;
        private List<DroneToList> DroneList;
        private Double[] BatteryUsed;

        private BL()
        {
            Data = DalFactory.GetDal("DalXml");
            DroneList = new();
            BatteryUsed = Data.GetBatteryUsed();

            foreach (Drone drone in Data.GetDrones(drone => true))
            {
                Parcel NewParcel = new();
                DroneToList NewDrone = new(drone.ID, drone.Model, drone.MaxWeight);

                Parcel parcel = Data.GetParcels(p => p.DroneID == drone.ID && p.Delivered == null).First();
                //foreach (Parcel parcel in Data.GetParcels(parcel => parcel.DroneID == drone.ID && parcel.Delivered == null))
                //{
                //    NewParcel = parcel;
                //}
                if (parcel.ID != 0)
                    NewDrone = InitDroneInDelivery(NewDrone, NewParcel);
                else
                    NewDrone = InitDroneNOTinDelivery(NewDrone);

                DroneList.Add(NewDrone);
            }
        }

        /// <summary>
        /// Initializing the drone to be in a delivery, giving it location and battery according to the status of the delivery.
        /// Location will be either at the station nearest to the sender or at the sender
        /// </summary>
        /// <param name="NewDrone"></param>
        /// <param name="parcel"></param>
        /// <returns>Drone to list object</returns>
        private DroneToList InitDroneInDelivery(DroneToList DroneInDelivery, Parcel parcel)   
        {
            DroneInDelivery.Status = DroneStatus.Delivery;         ///Updating drone's status
            DroneInDelivery.ParcelID = parcel.ID;
            Customer sender = Data.GetCustomer(parcel.SenderID), target = Data.GetCustomer(parcel.TargetID);
            Location SenderLocation = new(sender.Latitude, sender.Longitude), TargetLocation = new(target.Latitude, target.Longitude);
            Station NearestStationToTarget = GetNearestStation(TargetLocation, Data.GetStations(station => true));
            Station NearestStationTosender = GetNearestStation(SenderLocation, Data.GetStations(station => true));
            if (parcel.PickedUp == null)            ///Checking if the drone already picked up the parcel or not     
                DroneInDelivery.CurrentLocation = new(NearestStationTosender.Latitude, NearestStationTosender.Longitude);
            else    ///means it did get pickedup
                DroneInDelivery.CurrentLocation = SenderLocation;

            double total = BatteryUsageCurrStation(DroneInDelivery, parcel.SenderID, parcel.TargetID, NearestStationTosender, parcel.Weight); ///Returns the amount of battery needed to complete the delivery
            DroneInDelivery.BatteryStatus = GetRandBatteryStatus(total, 100);          ///Choosing random battery number between the minimum needed to complete the delivery and full battery
            return DroneInDelivery;
        }

        private DroneToList InitDroneNOTinDelivery(DroneToList NewDrone)
        {
            Station nearest;
            int RandomCustomer;
            Random rand = new();
            IEnumerable<Customer> AllPastCustomers = Data.GetParcels(parcel => parcel.Delivered != null).Select(parcel => Data.GetCustomer(parcel.TargetID));
            DroneCharge droneCharging = Data.GetDroneCharge(D => D.DroneID == NewDrone.ID).FirstOrDefault();
            if(droneCharging.DroneID != 0 || AllPastCustomers.Count() == 0)
            {
                NewDrone.Status = DroneStatus.Charging;
                NewDrone.CurrentLocation.Latitude = Data.GetStation(droneCharging.StationID).Latitude;
                NewDrone.CurrentLocation.Longitude = Data.GetStation(droneCharging.StationID).Longitude;
                Drone drone = new(NewDrone.ID);
                Data.UpdateDroneToBeCharge(drone, Data.GetStation(droneCharging.StationID), DateTime.Now);
                NewDrone.BatteryStatus = GetRandBatteryStatus(0, 21);
            }
            else
            {
                NewDrone.Status = DroneStatus.Available;
                RandomCustomer = rand.Next(0, AllPastCustomers.Count());
                NewDrone.CurrentLocation = new(AllPastCustomers.ElementAt(RandomCustomer).Latitude, AllPastCustomers.ElementAt(RandomCustomer).Longitude);
                nearest = GetNearestStation(NewDrone.CurrentLocation, Data.GetStations(station => true));
                NewDrone.BatteryStatus = RandBatteryToStation(NewDrone, new Location(nearest.Latitude, nearest.Longitude), BatteryUsed[0]);
            }
            return NewDrone;
        //    /**********************************************************************************************************************************/

        //    Station nearest;
        //    int RandomCustomer, AllPastCustomers;
        //    Random rand = new();
        //    DroneStatus option = (DroneStatus)rand.Next(0, 2);     ///Random status, Available or Charging
        //    IEnumerable<Station> AllAvailableStations = Data.GetStations(station => station.ChargeSlots > 0);
        //    IEnumerable<Customer> AllPastCustomers = Data.GetParcels(parcel => parcel.Delivered != null)
        //                                                 .Select(parcel => Data.GetCustomer(parcel.TargetID));
        //    if (AllPastCustomers.Count() == 0)                      ///We are assuming that the odds that there are no available stations are very unlikley
        //        option = DroneStatus.Charging;
        //    switch (option)
        //    {
        //        case DroneStatus.Available:
        //            NewDrone.Status = DroneStatus.Available;
        //            RandomCustomer = rand.Next(0, AllPastCustomers.Count());
        //            NewDrone.CurrentLocation = new(AllPastCustomers.ElementAt(RandomCustomer).Latitude, AllPastCustomers.ElementAt(RandomCustomer).Longitude);
        //            nearest = GetNearestStation(NewDrone.CurrentLocation, Data.GetStations(station => true));
        //            NewDrone.BatteryStatus = RandBatteryToStation(NewDrone, new Location(nearest.Latitude, nearest.Longitude), BatteryUsed[0]);
        //            break;
        //        case DroneStatus.Charging:
        //            NewDrone.Status = DroneStatus.Charging;
        //            RandomStation = rand.Next(0, AllAvailableStations.Count());
        //            Drone drone = new(NewDrone.ID);
        //            Data.UpdateDroneToBeCharge(drone, AllAvailableStations.ElementAt(RandomStation), DateTime.Now);
        //            NewDrone.CurrentLocation = new(AllAvailableStations.ElementAt(RandomStation).Latitude, AllAvailableStations.ElementAt(RandomStation).Longitude);
        //            NewDrone.BatteryStatus = GetRandBatteryStatus(0, 21);
        //            break;
        //    }
        //    return NewDrone;
        //}
    }
}

