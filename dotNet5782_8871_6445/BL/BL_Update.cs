using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL.DO;
using static IBL.BO.EnumsBL;

namespace IBL
{
    partial class BL
    {
        
        public void UpdateDroneName(int id, string model)
        {
            if (id < 100000 || id > 999999)
                throw new InvalidIDException("Drone ID has to have 6 positive digits.");
            Data.UpdateDroneName(id, model);
            foreach (var drone in DroneList)
            {
                if (drone.ID == id)
                    drone.Model = model;
            }
        }
        
        public void UpdateStation(int StationID, bool ChangeName, bool ChangeSlots, string name, int slots)
        {
            if (StationID < 100000 || StationID > 999999)
                throw new InvalidIDException("Invalid station ID number. Must have 6 digits");
            if (ChangeName)
                Data.UpdateStationName(StationID, name);
            if (ChangeSlots)
            {
                Station station = Data.GetStation(StationID);
                IEnumerable<DroneCharge> AllDroneCharge = Data.GetDroneCharge(droneCharge => droneCharge.StationID == StationID);
                int ChargeCounter = AllDroneCharge.Count();
                if (ChargeCounter > slots)
                    throw new InvalidSlotsException("Charge slots can't be less than the number of currently charging drones in the station");
                Data.UpdateStationSlots(StationID, slots);
            }
        }
       
        public void UpdateCustomer(int id, bool changeName, bool changePhone, string name, int phone)
        {
            if (id < 100000000 || id > 999999999)
                throw new InvalidIDException("Customer ID has to have 9 positive digits.");
            if (changePhone)
            {
                if (phone < 500000000 || phone > 599999999)
                    throw new InvalidPhoneNumberException("Invalid phone number");
                Data.UpdateCustomerPhone(id, phone);
            }
            if (changeName)
                Data.UpdateCustomerName(id, name);

        }
        
        public void UpdateDroneToBeCharged(int DroneID)
        {
            if (DroneID < 100000 || DroneID > 999999)
                throw new InvalidIDException("Drone ID has to have 6 positive digits.");
            Drone test = Data.GetDrone(DroneID);    ///Will throw an exception if the drone is not in the data
            Station NearestStat = new();
            DroneToList DroneToBeCharged = new();
            foreach (DroneToList drone in DroneList)
            {
                if (drone.ID == DroneID)
                {
                    if (drone.Status != DroneStatus.Available)
                        throw new DroneStatusExpetion("Drone is not availbale");
                    NearestStat = GetNearestStation(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, GetAllAvailableStations());

                    if (drone.BatteryStatus < Distance(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, NearestStat.Latitude, NearestStat.Longitude) * BatteryUsed[0])
                        throw new NotEnoughBatteryExpetion("There is not enough battery to reach the nearest station.");
                    drone.BatteryStatus -= Distance(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, NearestStat.Latitude, NearestStat.Longitude) * BatteryUsed[0];
                    drone.CurrentLocation = new(NearestStat.Latitude, NearestStat.Longitude);
                    drone.Status = DroneStatus.Charging;
                    Data.DroneToBeCharge(DroneID, NearestStat.ID, DateTime.Now);

                }
            }
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
            Data.ParcelDelivery(ParcelToBeDelivered.ID);
            DroneInDelivery.BatteryStatus -= DistanceDroneCustomer(DroneInDelivery, ParcelToBeDelivered.TargetID);
            DroneInDelivery.CurrentLocation.Latitude = Data.GetCustomer(ParcelToBeDelivered.TargetID).Latitude;
            DroneInDelivery.CurrentLocation.Longitude = Data.GetCustomer(ParcelToBeDelivered.TargetID).Longitude;
            DroneInDelivery.Status = DroneStatus.Available;
            DroneList[i] = DroneInDelivery;
        }
        
        public void UpdateDroneAvailable(int DroneID)
        {
            int i = 0;
            if (DroneID < 100000 || DroneID > 999999)
                throw new InvalidIDException("Drone ID has to have 6 positive digits.");
            DroneToList DroneToBeAvailable = new();
            for (i = 0; i < DroneList.Count; i++)
            {
                if (DroneList[i].ID == DroneID)
                {
                    DroneToBeAvailable = DroneList[i];
                    break;
                }
            }
            if (DroneToBeAvailable.Status != DroneStatus.Charging)
                throw new DroneStatusExpetion("Can't release a drone that isn't charging");
            DroneToBeAvailable.Status = DroneStatus.Available;
            DroneCharge DroneInCharge = Data.GetDroneCharge(DroneID);
            double TimeCharged = DateTime.Now.Subtract((DateTime)DroneInCharge.Start).TotalHours;
            DroneToBeAvailable.BatteryStatus += BatteryUsed[4] * TimeCharged;
            if (DroneToBeAvailable.BatteryStatus > 100)
                DroneToBeAvailable.BatteryStatus = 100;
            Data.DroneAvailable(DroneID);
            DroneList[i] = DroneToBeAvailable;
        }
        
        public void UpdateParcelToDrone(int DroneID)
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
                throw new NoAvailableParcelsException("There are no parcels to assign at this moment");
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
                throw new NoAvailableParcelsException("There are no parcels that can be assign to this drone at this moment");
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
            Data.ParcelCollected(ParcelToBeCollected.ID);
            DroneInDelivery.BatteryStatus -= DistanceDroneCustomer(DroneInDelivery, ParcelToBeCollected.SenderID);
            DroneInDelivery.CurrentLocation.Latitude = Data.GetCustomer(ParcelToBeCollected.SenderID).Latitude;
            DroneInDelivery.CurrentLocation.Longitude = Data.GetCustomer(ParcelToBeCollected.SenderID).Longitude;
            DroneList[i] = DroneInDelivery;
        }
    }
}
