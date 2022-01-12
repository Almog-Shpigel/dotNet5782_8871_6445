using System;
using System.Collections.Generic;
using System.Linq;
using BO;
using DO;
using static BO.EnumsBL;
using System.Runtime.CompilerServices;
using BL;

namespace BlApi
{
    partial class BL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneName(int droneID, string newModelName)
        {
            lock (Data)
            {
                if (droneID is < 100000 or > 999999)
                    throw new InvalidInputException("Drone ID has to have 6 positive digits.");
                if (newModelName == "")
                    return;                                     /// Do nothing
                Drone drone = new(droneID, newModelName);
                try
                {
                    Data.UpdateDroneName(drone);
                }
                catch (DroneExistException ex)
                {
                    throw new EntityExistException("Invalid operation. \n" + ex.Message); 
                }
                DroneToList newDrone = DroneList.Find(d => d.ID == droneID);
                DroneList.Remove(newDrone);
                newDrone.Model = newModelName;
                DroneList.Add(newDrone);
                DroneList = DroneList.OrderBy(d => d.ID).ToList();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStationName(int stationID, string newStationName)
        {
            lock (Data)
            {
                if (stationID is < 100000 or > 999999)
                    throw new InvalidInputException("Invalid station ID number. Must have 6 digits");
                if (newStationName == "")
                    return;
                Station station = new(stationID, newStationName);
                try
                {
                    Data.UpdateStationName(station);
                }
                catch (StationExistException ex)
                {
                    throw new EntityExistException("Invalid operation. \n" + ex.Message); 
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStationSlots(int stationID, int newNumberSlots, int currentlyCharging)
        {
            lock (Data)
            {
                if (stationID is < 100000 or > 999999)
                    throw new InvalidIDException("Invalid station ID number. Must have 6 digits");
                if (newNumberSlots < 0)
                    throw new InvalidInputException("Slots can't be less than a 0");
                if (currentlyCharging > newNumberSlots)
                    throw new InvalidSlotsException("Charge slots can't be less than the number of currently charging drones in the station");

                Station station = Data.GetStation(stationID);
                //if (station.ChargeSlots <= newNumberSlots)
                //    return;
                station = new(stationID, "", newNumberSlots - currentlyCharging);
                try
                {
                    Data.UpdateStationSlots(station);
                }
                catch (StationExistException ex)
                {
                    throw new EntityExistException("Invalid operation. \n" + ex.Message); 
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomerName(int customerID,string newCustomerName)
        {
            lock (Data)
            {
                if (customerID is < 100000000 or > 999999999)
                    throw new InvalidIDException("Customer ID has to have 9 positive digits.");
                if (newCustomerName == "")
                    return;
                Customer customer = new(customerID, newCustomerName);
                try
                {
                    Data.UpdateCustomerName(customer);
                }
                catch (CustomerExistException ex)
                {
                    throw new EntityExistException("Invalid operation. \n" + ex.Message); 
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomerPhone(int customerID, int newCustomerPhone)
        {
            lock (Data)
            {
                if (newCustomerPhone is < 500000000 or > 599999999)
                    throw new InvalidInputException("Invalid phone number");
                string phoneNumber ="0" + newCustomerPhone.ToString();
                Customer customer = new(customerID, "", phoneNumber);
                try
                {
                    Data.UpdateCustomerPhone(customer);
                }
                catch (CustomerExistException ex)
                {
                    throw new EntityExistException("Invalid operation. \n" + ex.Message); 
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneToBeAvailable(int droneID)
        {
            lock (Data)
            {
                DroneToList droneToBeAvailable;
                if (droneID is < 100000 or > 999999)
                    throw new InvalidIDException("Drone ID has to have 6 positive digits.");
                if (!DroneList.Any(d => d.ID == droneID))
                    throw new EntityExistException($"Drone {droneID} doesn't exist in data!");
                droneToBeAvailable = DroneList.Find(d => d.ID == droneID);
                if (droneToBeAvailable.Status != DroneStatus.Charging)
                    throw new InvalidOperationException("Can't release a drone that isn't charging");

                droneToBeAvailable.BatteryStatus = CalcBatteryCharged(droneToBeAvailable);
                Drone drone = new(droneID);
                try
                {
                    Data.UpdateDroneToBeAvailable(drone);
                }
                catch (DroneExistException ex)
                {
                    throw new EntityExistException("invalid operation. \n"+ ex.Message); 
                }
                DroneToList droneToList = DroneList.Find(d => d.ID == droneID);
                DroneList.Remove(droneToList);
                droneToList.BatteryStatus = droneToBeAvailable.BatteryStatus;
                droneToList.Status = DroneStatus.Available;
                DroneList.Add(droneToList);
                DroneList = DroneList.OrderBy(d => d.ID).ToList();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneToBeCharged(int droneID)
        {
            lock (Data)
            {
                if (droneID is < 100000 or > 999999)
                    throw new InvalidIDException("Drone ID has to have 6 positive digits.");
                Drone drone;
                try
                {
                    drone = Data.GetDrone(droneID);            ///Will throw an exception if the drone is not in the data
                }
                catch (DroneExistException ex)
                {
                    throw new EntityExistException("Invalid operation. \n " + ex.Message); // TO DO: maybe add to\change the message
                }
                Station nearestStatation;
                Location nearestStataionLocation;
                DroneToList droneToList = DroneList.Find(d => d.ID == droneID);
                if (droneToList.Status == DroneStatus.Delivery)
                    throw new DroneStatusExpetion("Drone is in the middle of a delivery");
                if (droneToList.Status == DroneStatus.Charging)
                    throw new DroneStatusExpetion("The drone is already charging");
                nearestStatation = GetNearestStation(droneToList.CurrentLocation, Data.GetStations(station => station.ChargeSlots > 0));
                nearestStataionLocation = new(nearestStatation.Latitude, nearestStatation.Longitude);
                if (droneToList.BatteryStatus < Distance(droneToList.CurrentLocation, nearestStataionLocation) * BatteryUsageEmpty)
                    throw new NotEnoughBatteryExpetion("There is not enough battery to reach the nearest station.");

                droneToList.BatteryStatus -= Distance(droneToList.CurrentLocation, nearestStataionLocation) * BatteryUsageEmpty;
                droneToList.CurrentLocation = new(nearestStataionLocation.Latitude, nearestStataionLocation.Longitude);
                droneToList.Status = DroneStatus.Charging;
                Data.UpdateDroneToBeCharge(drone, nearestStatation, DateTime.Now);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcelAssignToDrone(int droneID)
        {
            lock (Data)
            {
                Parcel maxParcel;
                DroneToList droneToBeAssign;
                IEnumerable<Parcel> allAvailableParcels;
                if (droneID < 100000 || droneID > 999999)
                    throw new InvalidIDException("Drone ID has to have 6 positive digits.");
                if (DroneList.Any(d => d.ID == droneID))
                    droneToBeAssign = DroneList.Find(d => d.ID == droneID);
                else
                    throw new EntityExistException($"Drone {droneID} doesn't exsits in the data!");
                if (droneToBeAssign.Status != DroneStatus.Available)
                    throw new DroneStatusExpetion("Drone is unavailable for a delivery!");
                allAvailableParcels = Data.GetParcels(p => p.DroneID == 0);
                if (!allAvailableParcels.Any())
                    throw new NoAvailableParcelsException("There are no parcels to assign at this moment.");
                allAvailableParcels = allAvailableParcels.Where(p => PossibleDelivery(droneToBeAssign, p));
                if (!allAvailableParcels.Any())
                    throw new NoAvailableParcelsException("There is not enough battery to complete a delivery. Try charging the drone.");
                allAvailableParcels = allAvailableParcels.Where(p => droneToBeAssign.MaxWeight >= p.Weight);
                if (!allAvailableParcels.Any())
                    throw new NoAvailableParcelsException("The drone can't carry any parcel at this moment.");
                allAvailableParcels = allAvailableParcels.OrderBy(p => p.Priority).ThenBy(p => p.Weight);
                maxParcel = allAvailableParcels.First();
                foreach (var parcel in allAvailableParcels)
                {
                    if (DistanceDroneCustomer(droneToBeAssign, Data.GetCustomer(parcel.SenderID)) < DistanceDroneCustomer(droneToBeAssign, Data.GetCustomer(maxParcel.SenderID)))
                        maxParcel = parcel;
                }
                Data.PairParcelToDrone(maxParcel, new(droneToBeAssign.ID));
                DroneList.Remove(droneToBeAssign);
                droneToBeAssign.Status = DroneStatus.Delivery;
                droneToBeAssign.ParcelID = maxParcel.ID;
                DroneList.Add(droneToBeAssign);
                DroneList = DroneList.OrderBy(d => d.ID).ToList();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcelCollectedByDrone(int droneID)
        {
            lock (Data)
            {
                Customer sender;
                Parcel parcelToBeCollected;
                DroneToList droneInDelivery;
                if (droneID < 100000 || droneID > 999999)
                    throw new InvalidIDException("Drone ID has to have 6 positive digits.");
                droneInDelivery = DroneList.Find(d => d.ID == droneID);
                if (droneInDelivery.Status != DroneStatus.Delivery)
                    throw new DroneNotInDeliveryException("This drone is not in delivery!");
                parcelToBeCollected = Data.GetParcel(droneInDelivery.ParcelID);
                if (parcelToBeCollected.PickedUp != null)
                    throw new ParcelTimesException("The parcel has been already collected!");

                sender = Data.GetCustomer(parcelToBeCollected.SenderID);
                Data.UpdateParcelCollected(parcelToBeCollected);
                DroneList.Remove(droneInDelivery);
                droneInDelivery.BatteryStatus -= DistanceDroneCustomer(droneInDelivery, sender) * BatteryUsageEmpty;
                droneInDelivery.CurrentLocation = new(sender.Latitude, sender.Longitude);
                DroneList.Add(droneInDelivery);
                DroneList = DroneList.OrderBy(d => d.ID).ToList();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcelDeleiveredByDrone(int droneID)
        {
            lock (Data)
            {
                if (droneID < 100000 || droneID > 999999)
                    throw new InvalidIDException("Drone ID has to have 6 positive digits.");
                DroneToList droneInDelivery = DroneList.Find(d => d.ID == droneID);
                if (droneInDelivery.Status != DroneStatus.Delivery)
                    throw new InvalidOperationException("This drone is not doing a delivery right now");
                Parcel parcelToBeDelivered = Data.GetParcel(droneInDelivery.ParcelID);
                if (parcelToBeDelivered.Delivered != null || parcelToBeDelivered.PickedUp == null)
                    throw new ParcelTimesException("The drone is not carrying the parcel right now");
                Data.UpdateParcelInDelivery(parcelToBeDelivered);
                Customer target = Data.GetCustomer(parcelToBeDelivered.TargetID);
                DroneList.Remove(droneInDelivery);
                droneInDelivery.BatteryStatus -= DistanceDroneCustomer(droneInDelivery, target) * GetWeightMultiplier(parcelToBeDelivered.Weight);
                droneInDelivery.CurrentLocation = new(target.Latitude, target.Longitude);
                droneInDelivery.Status = DroneStatus.Available;
                droneInDelivery.ParcelID = 0;
                DroneList.Add(droneInDelivery);
                DroneList = DroneList.OrderBy(d => d.ID).ToList();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDeleteParcel(int parcelID)
        {
            lock (Data)
            {
                Parcel parcelToBeDeleted;
                try
                {
                    parcelToBeDeleted = Data.GetParcel(parcelID);
                }
                catch (ParcelExistException ex)
                {
                    throw new EntityExistException($"Can't delete parcel {parcelID}.\n", ex);
                }
                if (parcelToBeDeleted.Scheduled != null)
                    throw new InvalidOperationException($"Can't delete parcel {parcelID}\n" +
                        $"Parcel {parcelID} has already being dispatched by one of our drones.");
                Data.UpdateDeleteParcel(parcelToBeDeleted);
            }
        }

        public void UpdateDroneSimulatorStart(int droneID, Action updateView, Func<bool> checkIfCanceled)
        {
            new DroneSimulator(this, droneID, updateView, checkIfCanceled);
        }
    }
}
