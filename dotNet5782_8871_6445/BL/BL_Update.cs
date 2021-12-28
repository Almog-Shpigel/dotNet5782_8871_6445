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
                throw new InvalidIDException("Drone ID has to have 6 positive digits.");
            if (NewModelName == "")
                return;                                     // Do nothing
            Data.UpdateDroneName(DroneID, NewModelName);
            foreach (DroneToList drone in DroneList)
            {
                if (drone.ID == DroneID)
                    drone.Model = NewModelName;
            }
        }
        

        public void UpdateStationName(int StationID, string NewStationName)
        {
            if (StationID is < 100000 or > 999999)
                throw new InvalidIDException("Invalid station ID number. Must have 6 digits");
            if(NewStationName != "")
                Data.UpdateStationName(StationID, NewStationName);
        }
        public void UpdateStationSlots(int StationID, int NewNumberSlots, int CurrentlyCharging)
        {
            if (StationID is < 100000 or > 999999)
                throw new InvalidIDException("Invalid station ID number. Must have 6 digits");
            if (NewNumberSlots < 0)
                throw new InvalidSlotsException("Slots can't be less than a 0");
            if (CurrentlyCharging > NewNumberSlots)
                throw new InvalidSlotsException("Charge slots can't be less than the number of currently charging drones in the station");
            Data.UpdateStationSlots(StationID, NewNumberSlots - CurrentlyCharging);
        }


        public void UpdateCustomer(int CustomerID, bool NameChanged, bool PhoneChanged, string NewCustomerName, int NewCustomerPhone)
        {
            if (CustomerID is < 100000000 or > 999999999)
                throw new InvalidIDException("Customer ID has to have 9 positive digits.");
            if (NewCustomerPhone is < 500000000 or > 599999999)
                throw new InvalidPhoneNumberException("Invalid phone number");
            if (PhoneChanged)
                Data.UpdateCustomerPhone(CustomerID, NewCustomerPhone);
            if (NameChanged)
                Data.UpdateCustomerName(CustomerID, NewCustomerName);
        }

        public void UpdateDroneToBeAvailable(int DroneID)
        {
            if (DroneID is < 100000 or > 999999)
                throw new InvalidIDException("Drone ID has to have 6 positive digits.");
            DroneToList DroneToBeAvailable = GetDroneToList(DroneID);
            if (DroneToBeAvailable.Status != DroneStatus.Charging)
                throw new DroneStatusExpetion("Can't release a drone that isn't charging");
            //if (!DroneList.Remove(DroneToBeAvailable))
            //    throw new Exception("Can't release drone from charging.");                  /// To Do: change Exception name
            
            double TimeDifference = (DateTime.Now - (DateTime)Data.GetDroneCharge(DroneID).Start).TotalHours;
            DroneToBeAvailable.BatteryStatus += BatteryUsed[4] * TimeDifference;
            DroneToBeAvailable.BatteryStatus *= 100;
            DroneToBeAvailable.BatteryStatus = (int)DroneToBeAvailable.BatteryStatus;
            DroneToBeAvailable.BatteryStatus /= 100;
            if (DroneToBeAvailable.BatteryStatus > 100)
                DroneToBeAvailable.BatteryStatus = 100;
            Data.UpdateDroneToBeAvailable(DroneID);
            foreach (var drone in DroneList)
                if (drone.ID == DroneToBeAvailable.ID)
                {
                    drone.BatteryStatus = DroneToBeAvailable.BatteryStatus;
                    drone.Status = DroneStatus.Available;
                }
            //DroneList.Add(DroneToBeAvailable);
        }

        public void UpdateDroneToBeCharged(int DroneID)
        {
            if (DroneID is < 100000 or > 999999)
                throw new InvalidIDException("Drone ID has to have 6 positive digits.");
            Drone test = Data.GetDrone(DroneID);            ///Will throw an exception if the drone is not in the data
            Station NearestStatation;
            Location NearestStataionLocation;
            foreach (DroneToList drone in DroneList)
            {
                if (drone.ID == DroneID)
                {
                    if (drone.Status != DroneStatus.Available)
                        throw new DroneStatusExpetion("Drone is not availbale");

                    NearestStatation = GetNearestStation(drone.CurrentLocation, Data.GetStations(station => station.ChargeSlots > 0));
                    NearestStataionLocation = new(NearestStatation.Latitude, NearestStatation.Longitude);

                    if (drone.BatteryStatus < Distance(drone.CurrentLocation, NearestStataionLocation) * BatteryUsed[0])
                        throw new NotEnoughBatteryExpetion("There is not enough battery to reach the nearest station.");

                    drone.BatteryStatus -= ((int)(Distance(drone.CurrentLocation, NearestStataionLocation) * BatteryUsed[0])*100)/100;
                    drone.CurrentLocation = new(NearestStataionLocation.Latitude, NearestStataionLocation.Longitude);
                    drone.Status = DroneStatus.Charging;
                    Data.UpdateDroneToBeCharge(DroneID, NearestStatation.ID, DateTime.Now);
                }
            }
        }
        
        public void UpdateParcelAssignToDrone(int DroneID)
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
            Data.PairParcelToDrone(MaxParcel.ID, DroneToBeAssign.ID);
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
            Data.UpdateParcelCollected(ParcelToBeCollected.ID);
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
            DroneToList DroneInDelivery = new();
            for (i = 0; i < DroneList.Count; i++)
            {
                if (DroneList[i].ID == DroneID)
                {
                    DroneInDelivery = DroneList[i];
                    break;
                }
            }
            if (DroneInDelivery.Status != DroneStatus.Delivery)
                throw new DroneStatusExpetion("This drone is not doing a delivery right now");
            Parcel ParcelToBeDelivered = Data.GetParcel(DroneInDelivery.ParcelID);
            if (ParcelToBeDelivered.Delivered != null || ParcelToBeDelivered.PickedUp == null)
                throw new ParcelTimesException("The drone is not carrying the parcel right now");
            Data.UpdateParcelInDelivery(ParcelToBeDelivered.ID);
            DroneInDelivery.BatteryStatus -= ((int)(DistanceDroneCustomer(DroneInDelivery, ParcelToBeDelivered.TargetID)*100))/100;
            DroneInDelivery.CurrentLocation.Latitude = Data.GetCustomer(ParcelToBeDelivered.TargetID).Latitude;
            DroneInDelivery.CurrentLocation.Longitude = Data.GetCustomer(ParcelToBeDelivered.TargetID).Longitude;
            DroneInDelivery.Status = DroneStatus.Available;
            DroneInDelivery.ParcelID = 0;
            DroneList[i] = DroneInDelivery;
        }
        public void DeleteStation(int StationID)
        {
            if (StationID is < 100000 or > 999999)
                throw new InvalidIDException("Invalid station ID number. Must have 6 digits");
            StationBL station = GetStation(StationID);
            if (station.ChargingDrones.Count() != 0)
                throw new InvalidDeleteException("Can't delete a station that have drones charging in it.");
            Data.DeleteStation(StationID);
        }
    }
}
