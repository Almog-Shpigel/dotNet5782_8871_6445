using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DalApi;
using DalObject;
using DO;
using static BO.EnumsBL;

namespace BlApi
{
    partial class BL
    {
        public void AddNewStation(StationBL StationBO)
        {
            if (StationBO.ID is < 100000 or > 999999)
                throw new InvalidInputException("Invalid station ID number. Must have 6 digits");
            if (StationBO.ChargeSlots < 0)
                throw new InvalidInputException("Charge slots can't be a negative number");
            if ((int)StationBO.Location.Latitude != 31 || (int)StationBO.Location.Longitude != 35)
                throw new InvalidInputException("The location is outside of Jerusalem");                    ///We assume for now that all the locations are inside Jerusalem
            Station StationDO = new(StationBO.ID, StationBO.Name, StationBO.ChargeSlots, StationBO.Location.Latitude, StationBO.Location.Longitude);
            try
            {
                Data.AddNewStation(StationDO);
            }
            catch (StationExistException ex)
            {
                throw new InvalidInputException("Invalid id. ", ex);
            }
        }

        public void AddNewDrone(DroneBL DroneBL, int StationID)         ///Reciving a drone with name, id and weight, and a staion id to sent it to charge there
        {
            if (DroneBL.ID is < 100000 or > 999999)
                throw new InvalidInputException("Drone ID has to have 6 positive digits.");
            Station station;
            try
            {
                station = Data.GetStation(StationID);
            }
            catch (StationExistException ex)
            {
                throw new InvalidInputException("Invalid id. ", ex);
            }
            if (station.ChargeSlots <= 0)
                throw new InvalidInputException("There are no slots available at this station.");
            DroneBL.CurrentLocation = new(station.Latitude, station.Longitude);
            DroneBL.BatteryStatus = GetRandBatteryStatus(20, 41);
            DroneBL.Status = DroneStatus.Charging;

            Drone NewDrone = new(DroneBL.ID, DroneBL.Model, DroneBL.MaxWeight);
            try
            {
                Data.AddNewDrone(NewDrone, station);              ///Sending the new drone to the data
            }
            catch (DroneExistException ex)
            {
                throw new InvalidInputException("Invalid id. ", ex);
            }
            catch (StationExistException ex)
            {
                throw new InvalidInputException("Invalid id. ", ex);
            }

            DroneToList NewDroneToList = new DroneToList(DroneBL.ID, DroneBL.Model, DroneBL.MaxWeight, DroneBL.BatteryStatus, DroneBL.Status, DroneBL.CurrentLocation, 0);
            DroneList.Add(NewDroneToList);      ///Saving a logic version of the new drone
        }

        public void AddNewCustomer(CustomerBL customer)
        {
            if (customer.ID is < 100000000 or > 999999999)
                throw new InvalidInputException("Invalid customer ID number");
            char str = customer.Phone[0];
            bool success = int.TryParse(customer.Phone, out int PhoneNumber);
            if (!success || str != '0' || PhoneNumber < 500000000 || PhoneNumber > 599999999) ///Checking if the number starts with a '05' and contain 10 numbers
                throw new InvalidInputException("Invalid phone number");
            if ((int)customer.Location.Latitude != 31 || (int)customer.Location.Longitude != 35)
                throw new InvalidInputException("The location is outside of Jerusalem"); ///We assume for now that all the locations
            Customer NewCustomer = new(customer.ID, customer.Name, customer.Phone, customer.Location.Latitude, customer.Location.Longitude);
            try
            {
                Data.AddNewCustomer(NewCustomer);
            }
            catch (CustomerExistException ex)
            {
                throw new InvalidInputException("Invalid id. ", ex);
            }
        }

        public void AddNewParcel(ParcelBL parcel)
        {
            if (parcel.Sender.ID < 100000000 || parcel.Sender.ID > 999999999)
                throw new InvalidInputException("Invalid sender ID number");
            if (parcel.Target.ID < 100000000 || parcel.Target.ID > 999999999)
                throw new InvalidInputException("Invalid receiver ID number");
            if (parcel.Target.ID == parcel.Sender.ID)
                throw new InvalidInputException("Customer can't send a parcel to himself");

            Parcel ParcelDO = new Parcel(parcel.ID, parcel.Sender.ID, parcel.Target.ID, 0, parcel.Weight, parcel.Priority, parcel.TimeRequested, parcel.Scheduled, parcel.PickedUp, parcel.Delivered);
            try
            {
                Data.AddNewParcel(ParcelDO);
            }
            catch (ParcelExistException ex)
            {
                throw new InvalidInputException("Invalid id. ", ex);
            }
        }
    }
}
