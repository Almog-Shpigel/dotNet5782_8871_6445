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
        /// <summary>
        /// Updating a drone's model
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
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
        /// <summary>
        /// Updating a station's charge slots or name or both, if an empty input recived for either, will not change it
        /// </summary>
        /// <param name="StationID"></param>
        /// <param name="ChangeName"></param>
        /// <param name="ChangeSlots"></param>
        /// <param name="name"></param>
        /// <param name="slots"></param>
        public void UpdateStation(int StationID, bool ChangeName, bool ChangeSlots, string name, int slots)
        {
            if (StationID < 100000 || StationID > 999999)
                throw new InvalidIDException("Invalid station ID number. Must have 6 digits");
            if (ChangeName)
                Data.UpdateStationName(StationID, name);
            if (ChangeSlots)
            {
                Station station = Data.GetStation(StationID);
                IEnumerable<DroneCharge> AllDroneCharge = Data.GetAllDronesCharge();
                int ChargeCounter = 0;
                foreach (var drone in AllDroneCharge)
                {
                    if (drone.StationID == StationID)
                        ChargeCounter++;
                }
                if (ChargeCounter > slots)
                    throw new InvalidSlotsException("Charge slots can't be less than the number of currently charging drones in the station");
                Data.UpdateStationSlots(StationID, slots);
            }
        }
        /// <summary>
        /// Updating a customer's phone number or name or both, if an empty input recived for either, will not change it
        /// </summary>
        /// <param name="id"></param>
        /// <param name="changeName"></param>
        /// <param name="changePhone"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
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
        /// <summary>
        /// Sending the wanted drone to charge at the nearest station that has free charge slots
        /// </summary>
        /// <param name="DroneID"></param>
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
        /// <summary>
        /// Updating that the parcel was delivered to the target by the drone carrying it, updating the drone's location 
        /// to be at the target customer, substracting the battery used from the last location and updating the drone's status to be
        /// available again
        /// </summary>
        /// <param name="DroneID"></param>
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
            if (ParcelToBeDelivered.Delivered != DateTime.MinValue || ParcelToBeDelivered.PickedUp == DateTime.MinValue)
                throw new ParcelTimesException("The drone is not carrying the parcel right now");
            Data.ParcelDelivery(ParcelToBeDelivered.ID);
            DroneInDelivery.BatteryStatus -= DistanceDroneCustomer(DroneInDelivery, ParcelToBeDelivered.TargetID);
            DroneInDelivery.CurrentLocation.Latitude = Data.GetCustomer(ParcelToBeDelivered.TargetID).Latitude;
            DroneInDelivery.CurrentLocation.Longitude = Data.GetCustomer(ParcelToBeDelivered.TargetID).Longitude;
            DroneInDelivery.Status = DroneStatus.Available;
            DroneList[i] = DroneInDelivery;
        }
        /// <summary>
        /// Relasing a drone that is currently charging, changing it's status to be available and deleting the "drone in charge" entity 
        /// saved in the data layer
        /// </summary>
        /// <param name="DroneID"></param>
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
            double TimeCharged = DateTime.Now.Subtract(DroneInCharge.Start).TotalHours;
            DroneToBeAvailable.BatteryStatus += BatteryUsed[4] * TimeCharged;
            if (DroneToBeAvailable.BatteryStatus > 100)
                DroneToBeAvailable.BatteryStatus = 100;
            Data.DroneAvailable(DroneID);
            DroneList[i] = DroneToBeAvailable;
        }
        /// <summary>
        /// Pairing a parcel to the wanted drone, the parcel will be selected by the next algorithem:
        /// highest priority, then weight similler to the drone propertie, then the distance between the parcel and the drone.
        /// all in consider the parcel isn't paired already with a different drone
        /// and the drone's battery is able to collect the parcel,deliver it and return to a near by station.
        /// </summary>
        /// <param name="DroneID"></param>
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

            IEnumerable<Parcel> AllAvailableParcels = Data.GetAllAvailableParcels();
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
        /// <summary>
        /// Updating a parcel status to be collected by the drone paired to her. also updating the drone's location
        /// to be at that target and the battery to substract according to the distance from it's last location
        /// </summary>
        /// <param name="DroneID"></param>
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
            if (ParcelToBeCollected.PickedUp != DateTime.MinValue)
                throw new ParcelTimesException("The parcel has been already collected!");
            Data.ParcelCollected(ParcelToBeCollected.ID);
            DroneInDelivery.BatteryStatus -= DistanceDroneCustomer(DroneInDelivery, ParcelToBeCollected.SenderID);
            DroneInDelivery.CurrentLocation.Latitude = Data.GetCustomer(ParcelToBeCollected.SenderID).Latitude;
            DroneInDelivery.CurrentLocation.Longitude = Data.GetCustomer(ParcelToBeCollected.SenderID).Longitude;
            DroneList[i] = DroneInDelivery;
        }
    }
}
