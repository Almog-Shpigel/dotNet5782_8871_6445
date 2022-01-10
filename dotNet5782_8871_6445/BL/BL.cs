using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BO;
using DO;
using static BO.EnumsBL;
using DAL;
using System.Runtime.CompilerServices;


namespace BlApi
{
    internal partial class BL : IBL
    {
        internal static readonly BL instance = new();

        static BL() { }

        /// <summary>
        /// The cunstrucator will initialize all the drones saved in the data layer, saving them as DroneToList objects and saving them in a list located in BL
        /// </summary>
        public static BL Instance { get => instance; }

        internal DalApi.IDal Data;
        private List<DroneToList> DroneList;
        //private Double[] BatteryUsed;
        private double BatteryUsageEmpty;
        private double BatteryUsageLightWight;
        private double BatteryUsageMediumWight;
        private double BatteryUsageHaevyWight;
        private double BatteryChargeRate;
        private BL()
        {
            Data = DalFactory.GetDal("DalXml");
            DroneList = new();
            //BatteryUsed = Data.GetBatteryUsed();
            BatteryUsageEmpty = Data.GetBatteryProperty("BatteryUsageEmpty");
            BatteryUsageLightWight = Data.GetBatteryProperty("BatteryUsageLightWight");
            BatteryUsageMediumWight = Data.GetBatteryProperty("BatteryUsageMediumWight");
            BatteryUsageHaevyWight = Data.GetBatteryProperty("BatteryUsageHaevyWight");
            BatteryChargeRate = Data.GetBatteryProperty("BatteryChargeRate");
            foreach (Drone drone in Data.GetDrones(drone => true))
            {
                DroneToList newDrone = new(drone.ID, drone.Model, drone.MaxWeight);
                Parcel newParcel = Data.GetParcels(parcel => parcel.DroneID == drone.ID).FirstOrDefault();
                if (newParcel.ID != 0 && newParcel.Delivered == null)
                    newDrone = InitDroneInDelivery(newDrone, newParcel);
                else
                    newDrone = InitDroneNOTinDelivery(newDrone);
                DroneList.Add(newDrone);
            }
        }

        /// <summary>
        /// Initializing the drone to be in a delivery, giving it location and battery according to the status of the delivery.
        /// Location will be either at the station nearest to the sender or at the sender
        /// </summary>
        /// <param name="NewDrone"></param>
        /// <param name="parcel"></param>
        /// <returns>Drone to list object</returns>
        private DroneToList InitDroneInDelivery(DroneToList droneInDelivery, Parcel parcel)
        {
            droneInDelivery.Status = DroneStatus.Delivery;         ///Updating drone's status
            droneInDelivery.ParcelID = parcel.ID;
            Customer sender = Data.GetCustomer(parcel.SenderID), target = Data.GetCustomer(parcel.TargetID);
            Location SenderLocation = new(sender.Latitude, sender.Longitude), TargetLocation = new(target.Latitude, target.Longitude);
            Station nearestStationToSender = GetNearestStation(SenderLocation, Data.GetStations(station => true));
            if (parcel.PickedUp == null)            ///Checking if the drone already picked up the parcel or not     
                droneInDelivery.CurrentLocation = new(nearestStationToSender.Latitude, nearestStationToSender.Longitude);
            else                                    ///means it did get pickedup
                droneInDelivery.CurrentLocation = SenderLocation;

            double total = BatteryUsageCurrStation(droneInDelivery, sender, target, nearestStationToSender, parcel.Weight); ///Returns the amount of battery needed to complete the delivery
            droneInDelivery.BatteryStatus = GetRandBatteryStatus(total, 100);          ///Choosing random battery number between the minimum needed to complete the delivery and full battery
            return droneInDelivery;
        }

        private DroneToList InitDroneNOTinDelivery(DroneToList newDrone)
        {
            Station nearest;
            Customer customer;
            int RandomCustomer, RandomStation;
            double battery;
            Random rand = new();
            IEnumerable<Station> AllAvailableStations = Data.GetStations(station => station.ChargeSlots > 0);
            IEnumerable<Customer> AllPastCustomers = Data.GetParcels(parcel => parcel.Delivered != null).Select(parcel => Data.GetCustomer(parcel.TargetID));
            DroneCharge droneCharging = Data.GetDroneCharge(D => D.DroneID == newDrone.ID).FirstOrDefault();
            if (droneCharging.DroneID != 0 || !AllPastCustomers.Any())
            {
                newDrone.Status = DroneStatus.Charging;
                if(droneCharging.StationID == 0)
                {
                    RandomStation = rand.Next(0, GetAvailableStations().Count());
                    nearest = AllAvailableStations.ElementAt(RandomStation);
                }
                else
                    nearest = Data.GetStation(droneCharging.StationID);
                newDrone.CurrentLocation = new(nearest.Latitude, nearest.Longitude);               
                Drone drone = new(newDrone.ID);
                if (droneCharging.DroneID == 0)
                    Data.UpdateDroneToBeCharge(drone, nearest, DateTime.Now);
                newDrone.BatteryStatus = GetRandBatteryStatus(0, 21);
            }
            else
            {
                newDrone.Status = DroneStatus.Available;
                RandomCustomer = rand.Next(0, AllPastCustomers.Count());
                customer = AllPastCustomers.ElementAt(RandomCustomer);
                newDrone.CurrentLocation = new(customer.Latitude, customer.Longitude);
                nearest = GetNearestStation(newDrone.CurrentLocation, Data.GetStations(station => true));
                battery = Distance(newDrone.CurrentLocation, new Location(nearest.Latitude, nearest.Longitude)) * BatteryUsageEmpty;
                newDrone.BatteryStatus = GetRandBatteryStatus(battery, 100);
            }
            return newDrone;
        }
    }
}

