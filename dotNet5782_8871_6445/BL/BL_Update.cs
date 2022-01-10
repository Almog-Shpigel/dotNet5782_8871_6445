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
        public void UpdateDroneName(int DroneID, string NewModelName)
        {
            lock (Data)
            {
                if (DroneID is < 100000 or > 999999)
                    throw new InvalidInputException("Drone ID has to have 6 positive digits.");
                if (NewModelName == "")
                    return;                                     /// Do nothing
                Drone drone = new(DroneID, NewModelName);
                try
                {
                    Data.UpdateDroneName(drone);
                }
                catch (DroneExistException ex)
                {
                    throw new EntityExistException(ex.Message); // TO DO: maybe add to\change the message
                }
                DroneToList newDrone = DroneList.Find(d => d.ID == DroneID);
                DroneList.Remove(newDrone);
                newDrone.Model = NewModelName;
                DroneList.Add(newDrone);
                DroneList = DroneList.OrderBy(d => d.ID).ToList();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStationName(int StationID, string NewStationName)
        {
            lock (Data)
            {
                if (StationID is < 100000 or > 999999)
                    throw new InvalidInputException("Invalid station ID number. Must have 6 digits");
                if (NewStationName == "")
                    return;
                Station station = new(StationID, NewStationName);
                try
                {
                    Data.UpdateStationName(station);
                }
                catch (StationExistException ex)
                {
                    throw new EntityExistException(ex.Message); // TO DO: maybe add to\change the message
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStationSlots(int StationID, int NewNumberSlots, int CurrentlyCharging)
        {
            lock (Data)
            {
                if (StationID is < 100000 or > 999999)
                    throw new InvalidIDException("Invalid station ID number. Must have 6 digits");
                if (NewNumberSlots < 0)
                    throw new InvalidInputException("Slots can't be less than a 0");
                if (CurrentlyCharging > NewNumberSlots)
                    throw new InvalidSlotsException("Charge slots can't be less than the number of currently charging drones in the station");
                Station station = new(StationID, "", NewNumberSlots - CurrentlyCharging);
                try
                {
                    Data.UpdateStationSlots(station);
                }
                catch (StationExistException ex)
                {
                    throw new EntityExistException(ex.Message); // TO DO: maybe add to\change the message
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomerName(int CustomerID,string NewCustomerName)
        {
            lock (Data)
            {
                if (CustomerID is < 100000000 or > 999999999)
                    throw new InvalidIDException("Customer ID has to have 9 positive digits.");
                Customer customer = new(CustomerID, NewCustomerName);
                try
                {
                    Data.UpdateCustomerName(customer);
                }
                catch (CustomerExistException ex)
                {
                    throw new EntityExistException(ex.Message); // TO DO: maybe add to\change the message
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomerPhone(int CustomerID, int NewCustomerPhone)
        {
            lock (Data)
            {
                if (NewCustomerPhone is < 500000000 or > 599999999)
                    throw new InvalidInputException("Invalid phone number");
                string phoneNumber = NewCustomerPhone.ToString();
                Customer customer = new(CustomerID, "", phoneNumber);
                try
                {
                    Data.UpdateCustomerPhone(customer);
                }
                catch (CustomerExistException ex)
                {
                    throw new EntityExistException(ex.Message); // TO DO: maybe add to\change the message
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneToBeAvailable(int DroneID)
        {
            lock (Data)
            {
                DroneToList DroneToBeAvailable;
                if (DroneID is < 100000 or > 999999)
                    throw new InvalidIDException("Drone ID has to have 6 positive digits.");
                if (!DroneList.Any(d => d.ID == DroneID))
                    throw new EntityExistException($"Drone {DroneID} doesn't exist in data!");
                DroneToBeAvailable = DroneList.Find(d => d.ID == DroneID);
                if (DroneToBeAvailable.Status != DroneStatus.Charging)
                    throw new InvalidOperationException("Can't release a drone that isn't charging");

                DroneToBeAvailable.BatteryStatus = CalcBatteryCharged(DroneToBeAvailable);
                Drone drone = new(DroneID);
                try
                {
                    Data.UpdateDroneToBeAvailable(drone);
                }
                catch (DroneExistException ex)
                {
                    throw new EntityExistException(ex.Message); // TO DO: maybe add to\change the message
                }
                DroneToList droneToList = DroneList.Find(d => d.ID == DroneID);
                DroneList.Remove(droneToList);
                droneToList.BatteryStatus = DroneToBeAvailable.BatteryStatus;
                droneToList.Status = DroneStatus.Available;
                DroneList.Add(droneToList);
                DroneList = DroneList.OrderBy(d => d.ID).ToList();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneToBeCharged(int DroneID)
        {
            lock (Data)
            {
                if (DroneID is < 100000 or > 999999)
                    throw new InvalidIDException("Drone ID has to have 6 positive digits.");
                Drone drone;
                try
                {
                    drone = Data.GetDrone(DroneID);            ///Will throw an exception if the drone is not in the data
                }
                catch (DroneExistException ex)
                {
                    throw new EntityExistException(ex.Message); // TO DO: maybe add to\change the message
                }
                Station NearestStatation;
                Location NearestStataionLocation;
                DroneToList droneToList = DroneList.Find(d => d.ID == DroneID);
                if (droneToList.Status == DroneStatus.Delivery)
                    throw new DroneStatusExpetion("Drone is in the middle of a delivery");
                if (droneToList.Status == DroneStatus.Charging)
                    throw new DroneStatusExpetion("The drone is already charging");
                NearestStatation = GetNearestStation(droneToList.CurrentLocation, Data.GetStations(station => station.ChargeSlots > 0));
                NearestStataionLocation = new(NearestStatation.Latitude, NearestStatation.Longitude);
                if (droneToList.BatteryStatus < Distance(droneToList.CurrentLocation, NearestStataionLocation) * BatteryUsageEmpty)
                    throw new NotEnoughBatteryExpetion("There is not enough battery to reach the nearest station.");

                droneToList.BatteryStatus -= Distance(droneToList.CurrentLocation, NearestStataionLocation) * BatteryUsageEmpty;
                droneToList.CurrentLocation = new(NearestStataionLocation.Latitude, NearestStataionLocation.Longitude);
                droneToList.Status = DroneStatus.Charging;
                Data.UpdateDroneToBeCharge(drone, NearestStatation, DateTime.Now);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcelAssignToDrone(int DroneID)
        {
            lock (Data)
            {
                Parcel MaxParcel;
                DroneToList DroneToBeAssign;
                IEnumerable<Parcel> AllAvailableParcels;
                if (DroneID < 100000 || DroneID > 999999)
                    throw new InvalidIDException("Drone ID has to have 6 positive digits.");
                if (DroneList.Any(d => d.ID == DroneID))
                    DroneToBeAssign = DroneList.Find(d => d.ID == DroneID);
                else
                    throw new EntityExistException($"Drone {DroneID} doesn't exsits in the data!");
                if (DroneToBeAssign.Status != DroneStatus.Available)
                    throw new DroneStatusExpetion("Drone is unavailable for a delivery!");
                AllAvailableParcels = Data.GetParcels(p => p.DroneID == 0);
                if (!AllAvailableParcels.Any())
                    throw new NoAvailableParcelsException("There are no parcels to assign at this moment.");
                AllAvailableParcels = AllAvailableParcels.Where(p => PossibleDelivery(DroneToBeAssign, p));
                if (!AllAvailableParcels.Any())
                    throw new NoAvailableParcelsException("There is not enough battery to complete a delivery. Try charging the drone.");
                AllAvailableParcels = AllAvailableParcels.Where(p => DroneToBeAssign.MaxWeight >= p.Weight);
                if (!AllAvailableParcels.Any())
                    throw new NoAvailableParcelsException("The drone can't carry any parcel at this moment.");
                AllAvailableParcels = AllAvailableParcels.OrderBy(p => p.Priority).ThenBy(p => p.Weight);
                MaxParcel = AllAvailableParcels.First();
                foreach (var parcel in AllAvailableParcels)
                {
                    if (DistanceDroneCustomer(DroneToBeAssign, Data.GetCustomer(parcel.SenderID)) < DistanceDroneCustomer(DroneToBeAssign, Data.GetCustomer(MaxParcel.SenderID)))
                        MaxParcel = parcel;
                }
                Data.PairParcelToDrone(MaxParcel, new(DroneToBeAssign.ID));
                DroneList.Remove(DroneToBeAssign);
                DroneToBeAssign.Status = DroneStatus.Delivery;
                DroneToBeAssign.ParcelID = MaxParcel.ID;
                DroneList.Add(DroneToBeAssign);
                DroneList = DroneList.OrderBy(d => d.ID).ToList();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcelCollectedByDrone(int DroneID)
        {
            lock (Data)
            {
                Customer sender;
                Parcel ParcelToBeCollected;
                DroneToList droneInDelivery;
                if (DroneID < 100000 || DroneID > 999999)
                    throw new InvalidIDException("Drone ID has to have 6 positive digits.");
                droneInDelivery = DroneList.Find(d => d.ID == DroneID);
                if (droneInDelivery.Status != DroneStatus.Delivery)
                    throw new DroneNotInDeliveryException("This drone is not in delivery!");
                ParcelToBeCollected = Data.GetParcel(droneInDelivery.ParcelID);
                if (ParcelToBeCollected.PickedUp != null)
                    throw new ParcelTimesException("The parcel has been already collected!");

                sender = Data.GetCustomer(ParcelToBeCollected.SenderID);
                Data.UpdateParcelCollected(ParcelToBeCollected);
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDroneSimulatorStart(int droneID, Action updateView, Func<bool> checkIfCanceled)
        {
            new DroneSimulator(this, droneID, updateView, checkIfCanceled);
        }
        
    }
}
