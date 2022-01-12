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
using System.Runtime.CompilerServices;


namespace BlApi
{
    partial class BL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewStation(StationBL stationBO)
        {
            lock (Data)
            {
                if (stationBO.ID is < 100000 or > 999999)
                    throw new InvalidInputException("Invalid station ID number. Must have 6 digits");
                if (stationBO.ChargeSlots < 0)
                    throw new InvalidInputException("Charge slots can't be a negative number");
                if ((int)stationBO.Location.Latitude != 31 || (int)stationBO.Location.Longitude != 35)
                    throw new InvalidInputException("The location is outside of Jerusalem");                    ///We assume for now that all the locations are inside Jerusalem
                Station stationDO = new(stationBO.ID, stationBO.Name, stationBO.ChargeSlots, stationBO.Location.Latitude, stationBO.Location.Longitude);
                try
                {
                    Data.AddNewStation(stationDO);
                }
                catch (StationExistException ex)
                {
                    throw new InvalidInputException("Invalid id. \n" + ex.Message);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewDrone(DroneBL droneBL, int stationID)         ///Reciving a drone with name, id and weight, and a staion id to sent it to charge there
        {
            lock(Data)
            {
                if (droneBL.ID is < 100000 or > 999999)
                    throw new InvalidInputException("Drone ID has to have 6 positive digits.");
                Station station;
                try
                {
                    station = Data.GetStation(stationID);
                }
                catch (StationExistException ex)
                {
                    throw new InvalidInputException("Invalid id. \n" + ex.Message);
                }
                if (station.ChargeSlots <= 0)
                    throw new InvalidInputException("There are no slots available at this station.");
                droneBL.CurrentLocation = new(station.Latitude, station.Longitude);
                droneBL.BatteryStatus = GetRandBatteryStatus(20, 41);
                droneBL.Status = DroneStatus.Charging;

                Drone newDrone = new(droneBL.ID, droneBL.Model, droneBL.MaxWeight);
                try
                {
                    Data.AddNewDrone(newDrone, station);              ///Sending the new drone to the data
                }
                catch (DroneExistException ex)
                {
                    throw new InvalidInputException("Invalid id. \n" + ex.Message);
                }
                catch (StationExistException ex)
                {
                    throw new InvalidInputException("Invalid id. \n" + ex.Message);
                }

                DroneToList newDroneToList = new DroneToList(droneBL.ID, droneBL.Model, droneBL.MaxWeight, droneBL.BatteryStatus, droneBL.Status, droneBL.CurrentLocation, 0);
                DroneList.Add(newDroneToList);      ///Saving a logic version of the new drone
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewCustomer(CustomerBL customer)
        {
            lock (Data)
            {
                if (customer.ID is < 100000000 or > 999999999)
                    throw new InvalidInputException("Invalid customer ID number");
                char str = customer.Phone[0];
                bool success = int.TryParse(customer.Phone, out int PhoneNumber);
                if (!success || str != '0' || PhoneNumber < 500000000 || PhoneNumber > 599999999) ///Checking if the number starts with a '05' and contain 10 numbers
                    throw new InvalidInputException("Invalid phone number");
                if ((int)customer.Location.Latitude != 31 || (int)customer.Location.Longitude != 35)
                    throw new InvalidInputException("The location is outside of Jerusalem"); ///We assume for now that all the locations
                Customer newCustomer = new(customer.ID, customer.Name, customer.Phone, customer.Location.Latitude, customer.Location.Longitude);
                try
                {
                    Data.AddNewCustomer(newCustomer);
                }
                catch (CustomerExistException ex)
                {
                    throw new InvalidInputException("Invalid id. \n" + ex.Message);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddNewParcel(ParcelBL parcel)
        {
            lock (Data)
            {
                if (parcel.Sender.ID < 100000000 || parcel.Sender.ID > 999999999)
                    throw new InvalidInputException("Invalid sender ID number");
                if (parcel.Target.ID < 100000000 || parcel.Target.ID > 999999999)
                    throw new InvalidInputException("Invalid receiver ID number");
                if (parcel.Target.ID == parcel.Sender.ID)
                    throw new InvalidInputException("Customer can't send a parcel to himself");

                Parcel parcelDO = new Parcel(parcel.ID, parcel.Sender.ID, parcel.Target.ID, 0, parcel.Weight, parcel.Priority, parcel.TimeRequested, parcel.Scheduled, parcel.PickedUp, parcel.Delivered);
                try
                {
                    Data.AddNewParcel(parcelDO);
                }
                catch (ParcelExistException ex)
                {
                    throw new InvalidInputException("Invalid id. \n"  + ex.Message);
                }
            }
        }
    }
}
