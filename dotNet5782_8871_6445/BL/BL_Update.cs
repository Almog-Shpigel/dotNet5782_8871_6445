using System;
using System.Collections.Generic;
using System.Linq;
using BO;
using DO;
using static BO.EnumsBL;

namespace BlApi
{
    partial class BL
    {
        public void UpdateDroneName(int DroneID, string NewModelName)
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
            DroneToList newDrone = DroneList.Find(d =>d.ID == DroneID);
            DroneList.Remove(newDrone);
            newDrone.Model = NewModelName;
            DroneList.Add(newDrone);
            DroneList = DroneList.OrderBy(d => d.ID).ToList();
        }

        public void UpdateStationName(int StationID, string NewStationName)
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

        public void UpdateStationSlots(int StationID, int NewNumberSlots, int CurrentlyCharging)
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


        public void UpdateCustomerName(int CustomerID,string NewCustomerName)
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
        public void UpdateCustomerPhone(int CustomerID, int NewCustomerPhone)
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

        public void UpdateDroneToBeAvailable(int DroneID)
        {
            if (DroneID is < 100000 or > 999999)
                throw new InvalidIDException("Drone ID has to have 6 positive digits.");
            DroneToList DroneToBeAvailable = GetDroneToList(DroneID);
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

        public void UpdateDroneToBeCharged(int DroneID)
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
            if (droneToList.BatteryStatus < Distance(droneToList.CurrentLocation, NearestStataionLocation) * BatteryUsed[0])
                throw new NotEnoughBatteryExpetion("There is not enough battery to reach the nearest station.");

            droneToList.BatteryStatus -= ((int)(Distance(droneToList.CurrentLocation, NearestStataionLocation) * BatteryUsed[0])*100)/100;
            droneToList.CurrentLocation = new(NearestStataionLocation.Latitude, NearestStataionLocation.Longitude);
            droneToList.Status = DroneStatus.Charging;
            Data.UpdateDroneToBeCharge(drone, NearestStatation, DateTime.Now);

        }
        
        public void UpdateParcelAssignToDrone(int DroneID)      // TO DO: rewrite this function
        {
            int i = 0;
            if (DroneID < 100000 || DroneID > 999999)
                throw new InvalidIDException("Drone ID has to have 6 positive digits.");
            DroneToList DroneToBeAssign = new();
            for (; i < DroneList.Count; i++)
            {
                if (DroneList[i].ID == DroneID)
                {
                    DroneToBeAssign = DroneList[i];
                    break;
                }
            }
            if (DroneToBeAssign.Status != DroneStatus.Available)
                throw new DroneStatusExpetion("Drone is unavailable for a delivery!");

            IEnumerable<Parcel> AllAvailableParcels = Data.GetParcels(parcel => parcel.DroneID == 0);
            if (AllAvailableParcels.Count() == 0)
                throw new NoAvailableParcelsException("There are no parcels to assign at this moment.");
            Parcel MaxParcel = AllAvailableParcels.First();
            foreach (var parcel in AllAvailableParcels)
            {
                if (PossibleDelivery(DroneToBeAssign, parcel))
                {
                    if (parcel.Priority > MaxParcel.Priority)
                        MaxParcel = parcel;
                    if (parcel.Priority == MaxParcel.Priority)
                    {
                        if (parcel.Weight > MaxParcel.Weight && DroneToBeAssign.MaxWeight >= parcel.Weight)
                            MaxParcel = parcel;
                        if (parcel.Weight == MaxParcel.Weight && DroneToBeAssign.MaxWeight >= parcel.Weight)
                        {
                            if (DistanceDroneCustomer(DroneToBeAssign, parcel.SenderID) < DistanceDroneCustomer(DroneToBeAssign, MaxParcel.SenderID))
                                MaxParcel = parcel;
                        }
                    }
                }
            }
            if (!PossibleDelivery(DroneToBeAssign, MaxParcel))
                throw new NoAvailableParcelsException("There is not enough battery to complete a delivery. Try charging the drone.");
            Drone drone = new(DroneToBeAssign.ID);
            Data.PairParcelToDrone(MaxParcel, drone);
            DroneToBeAssign.Status = DroneStatus.Delivery;
            DroneToBeAssign.ParcelID = MaxParcel.ID;
            DroneList[i] = DroneToBeAssign;
        }
        
        public void UpdateParcelCollectedByDrone(int DroneID)
        {
            int i = 0;
            if (DroneID < 100000 || DroneID > 999999)
                throw new InvalidIDException("Drone ID has to have 6 positive digits.");
            DroneToList DroneInDelivery = new();
            for (; i < DroneList.Count; i++)
            {
                if (DroneList[i].ID == DroneID)
                {
                    DroneInDelivery = DroneList[i];
                    break;
                }
            }
            if (DroneInDelivery.Status != DroneStatus.Delivery)
                throw new DroneNotInDeliveryException("This drone is not in delivery!");
            Parcel ParcelToBeCollected = Data.GetParcel(DroneInDelivery.ParcelID);
            if (ParcelToBeCollected.PickedUp != null)
                throw new ParcelTimesException("The parcel has been already collected!");
            Data.UpdateParcelCollected(ParcelToBeCollected);
            DroneInDelivery.BatteryStatus -= ((int)(DistanceDroneCustomer(DroneInDelivery, ParcelToBeCollected.SenderID)*100))/100;
            DroneInDelivery.CurrentLocation.Latitude = Data.GetCustomer(ParcelToBeCollected.SenderID).Latitude;
            DroneInDelivery.CurrentLocation.Longitude = Data.GetCustomer(ParcelToBeCollected.SenderID).Longitude;
            DroneList[i] = DroneInDelivery;
        }

        public void UpdateParcelDeleiveredByDrone(int DroneID)
        {
            int i = 0;
            if (DroneID < 100000 || DroneID > 999999)
                throw new InvalidIDException("Drone ID has to have 6 positive digits.");
            DroneToList DroneInDelivery = DroneList.Find(d => d.ID == DroneID);
            if (DroneInDelivery.Status != DroneStatus.Delivery)
                throw new InvalidOperationException("This drone is not doing a delivery right now");
            Parcel ParcelToBeDelivered = Data.GetParcel(DroneInDelivery.ParcelID);
            if (ParcelToBeDelivered.Delivered != null || ParcelToBeDelivered.PickedUp == null)
                throw new ParcelTimesException("The drone is not carrying the parcel right now");
            Data.UpdateParcelInDelivery(ParcelToBeDelivered);
            DroneInDelivery.BatteryStatus -= ((int)(DistanceDroneCustomer(DroneInDelivery, ParcelToBeDelivered.TargetID)*100))/100;
            DroneInDelivery.CurrentLocation.Latitude = Data.GetCustomer(ParcelToBeDelivered.TargetID).Latitude;
            DroneInDelivery.CurrentLocation.Longitude = Data.GetCustomer(ParcelToBeDelivered.TargetID).Longitude;
            DroneInDelivery.Status = DroneStatus.Available;
            DroneInDelivery.ParcelID = 0;
            DroneList[i] = DroneInDelivery;
        }
    }
}
