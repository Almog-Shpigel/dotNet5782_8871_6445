using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DO;
using static BO.EnumsBL;

namespace BlApi
{
    partial class BL
    {
        public StationBL GetStation(int StationID)
        {
            Station station = Data.GetStation(StationID);
            Location location = new(station.Latitude, station.Longitude);
            StationBL StationToPrint = new(station.ID, station.Name, station.ChargeSlots, location);
            foreach (DroneCharge DroneCharge in Data.GetDroneCharge(droneCharge => true))
            {
                foreach (DroneToList DroneItem in DroneList)
                {
                    if (DroneItem.ID == DroneCharge.DroneID && DroneCharge.StationID == StationID)
                    {
                        DroneChargeBL drone = new(DroneCharge.DroneID, DroneItem.BatteryStatus);
                        StationToPrint.ChargingDrones.Add(drone);
                    }
                }
            }
            return StationToPrint;
        }
        
        public DroneBL GetDrone(int DroneID)
        {
            DroneBL DroneToDisplay;
            foreach (DroneToList drone in DroneList)
                if (drone.ID == DroneID)
                {
                    DroneToDisplay = new(drone.ID, drone.Model, drone.MaxWeight, drone.BatteryStatus, drone.Status); /// battery status needs to be updated every time
                    DroneToDisplay.CurrentLocation = drone.CurrentLocation;
                    if (drone.ParcelID == 0)
                        return DroneToDisplay;
                    if (DroneToDisplay.Status == DroneStatus.Delivery)
                    {
                        DroneToDisplay.Parcel = InitParcelInDelivery(Data.GetParcel(drone.ParcelID));
                    }
                    return DroneToDisplay;
                }
            throw new DroneExistException();
        }

        /// <summary>
        /// Function that receive a parcel and initialize ParcelInDelivery entity 
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns> ParcelInDelivery entity </returns>
        private ParcelInDelivery InitParcelInDelivery(Parcel parcel)
        {
            ParcelInDelivery UpdateParcelInDelivery = new ParcelInDelivery();
            UpdateParcelInDelivery.ID = parcel.ID;
            UpdateParcelInDelivery.Priority = parcel.Priority;
            UpdateParcelInDelivery.Weight = parcel.Weight;
            Customer sender = Data.GetCustomer(parcel.SenderID), target = Data.GetCustomer(parcel.TargetID);
            UpdateParcelInDelivery.Sender.ID = sender.ID;
            UpdateParcelInDelivery.Sender.Name = sender.Name;
            UpdateParcelInDelivery.Target.ID = target.ID;
            UpdateParcelInDelivery.Target.Name = target.Name;
            UpdateParcelInDelivery.PickUpLocation.Latitude = sender.Latitude;
            UpdateParcelInDelivery.PickUpLocation.Longitude = sender.Longitude;
            UpdateParcelInDelivery.TargetLocation.Latitude = target.Latitude;
            UpdateParcelInDelivery.TargetLocation.Longitude = target.Longitude;
            UpdateParcelInDelivery.DeliveryDistance = DistanceCustomerCustomer(sender.ID, target.ID);
            UpdateParcelInDelivery.Status = FindParcelStatus(parcel);
            return UpdateParcelInDelivery;
        }

        /// <summary>
        /// Return true if the parcel is in the middle of a delivery and false if it's not
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns></returns>
        private bool FindParcelStatus(Parcel parcel)
        {
            if (parcel.Scheduled == null || parcel.Delivered != null)
                return false;
            return true;
        }
        
        public CustomerBL GetCustomer(int CustomerID)
        {
            Customer customer = Data.GetCustomer(CustomerID);
            Location location = new(customer.Latitude, customer.Longitude);
            CustomerBL CustomerToDisplay = new CustomerBL(customer.ID, customer.Name, customer.Phone, location);
            foreach (var parcel in Data.GetParcels(parcel => true))
            {
                if (parcel.SenderID == CustomerToDisplay.ID)
                    CustomerToDisplay.ParcelesSentByCustomer.Add(CreateParcelAtCustomer(parcel, parcel.TargetID));
                if (parcel.TargetID == CustomerToDisplay.ID)
                    CustomerToDisplay.ParcelesSentToCustomer.Add(CreateParcelAtCustomer(parcel, parcel.SenderID));
            }
            return CustomerToDisplay;

        }

        /// <summary>
        /// Receive a parcel and customer id and create a ParcelAtCustomer entity
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="CustomerID"></param>
        /// <returns>ParcelAtCustomer entity</returns>
        private ParcelAtCustomer CreateParcelAtCustomer(Parcel parcel, int CustomerID)
        {
            ParcelStatus status = GetParcelStatus(parcel);
            Customer cust = Data.GetCustomer(CustomerID);
            CustomerInParcel customer = new(cust.ID, cust.Name);
            ParcelAtCustomer NewParcel = new(parcel.ID, parcel.Weight, parcel.Priority, status, customer);
            return NewParcel;

        }

        /// <summary>
        /// Receiving the parcel's status according to it's updated times.
        /// Requested -> Schedualed -> PickedUp -> Delivered
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns>Parcel status</returns>
        private ParcelStatus GetParcelStatus(Parcel parcel)
        {
            if (parcel.Scheduled == null)
                return ParcelStatus.Requested;
            if (parcel.PickedUp == null)
                return ParcelStatus.Scheduled;
            if (parcel.Delivered == null)
                return ParcelStatus.PickedUp;
            return ParcelStatus.Delivered;
        }
       
        public ParcelBL GetParcel(int ParcelID)
        {
            Parcel parcel = Data.GetParcel(ParcelID);
            ParcelBL ParcelToDisplay = new(parcel.SenderID, parcel.TargetID, parcel.Weight, parcel.Priority);
            ParcelToDisplay.ID = parcel.ID;
            ParcelToDisplay.TimeRequested = parcel.TimeRequested;
            ParcelToDisplay.Scheduled = parcel.Scheduled;
            ParcelToDisplay.PickedUp = parcel.PickedUp;
            ParcelToDisplay.Delivered = parcel.Delivered;
            Customer sender = Data.GetCustomer(parcel.SenderID), target = Data.GetCustomer(parcel.TargetID);
            ParcelToDisplay.Sender.Name = sender.Name;
            ParcelToDisplay.Target.Name = target.Name;
            if (parcel.DroneID != 0)
                ParcelToDisplay.DroneInParcel = CreateDroneInParcel(parcel.DroneID);
            return ParcelToDisplay;
        }

        /// <summary>
        /// Receive drone id and create a DroneInParcel entity
        /// </summary>
        /// <param name="droneID"></param>
        /// <returns>DroneInParcel entity</returns>
        private DroneInParcel CreateDroneInParcel(int droneID)
        {
            DroneToList tempDrone = new DroneToList();
            foreach (var drone in DroneList)
            {
                if (drone.ID == droneID)
                    tempDrone = drone;
            }
            DroneInParcel Drone = new(tempDrone.ID, tempDrone.BatteryStatus, tempDrone.CurrentLocation);
            return Drone;
        }
        
        public string DisplayDistanceFromStation(double latitude, double longitude, int StationID)
        {
            Station station = Data.GetStation(StationID);
            Location StationLocation = new(station.Latitude, station.Longitude), location = new(latitude, longitude);
            return "The distance is: " + Distance(StationLocation, location) + " km";
        }
       
        public string DisplayDistanceFromCustomer(double longitude1, double latitude1, int CustomerID)
        {
            Customer customer = Data.GetCustomer(CustomerID);
            Location CustomerLocation = new(customer.Latitude, customer.Longitude), location = new(latitude1, longitude1);
            return "The distance is: " + Distance(CustomerLocation, location) + " km";
        }
    }
}
