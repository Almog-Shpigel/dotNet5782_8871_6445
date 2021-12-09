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
        public void AddNewStation(StationBL StationBO)
        {
            if (StationBO.ID is < 100000 or > 999999)
                throw new InvalidIDException("Invalid station ID number. Must have 6 digits");
            if (StationBO.ChargeSlots < 0)
                throw new InvalidSlotsException("Charge slots can't be a negative number");
            if ((int)StationBO.Location.Latitude != 31 || (int)StationBO.Location.Longitude != 35)
                throw new OutOfRangeLocationException("The location is outside of Jerusalem"); ///We assume for now that all the locations are inside Jerusalem
            Station StationDO = new(StationBO.ID, StationBO.Name, StationBO.ChargeSlots, StationBO.Location.Latitude, StationBO.Location.Longitude);
            Data.AddNewStation(StationDO);
        }
       
        public void AddNewDrone(DroneBL DroneBL, int StationID) ///Reciving a drone with name,id and weight, and a staion id to sent it to charge there
        {
            if (DroneBL.ID is < 100000 or > 999999)
                throw new InvalidIDException("Drone ID has to have 6 positive digits.");
            DroneBL.BatteryStatus = GetRandBatteryStatus(20, 41);
            DroneBL.Status = DroneStatus.Charging;
            IEnumerable<Station> stations = Data.GetStations(station => true);
            foreach (Station station in stations)
                if (station.ID == StationID)
                {
                    if (station.ChargeSlots <= 0)
                        throw new InvalidSlotsException("There are no slots available at this station.");
                    DroneBL.CurrentLocation = new(station.Latitude, station.Longitude);
                }
            Drone NewDrone = new Drone(DroneBL.ID, DroneBL.Model, DroneBL.MaxWeight);
            try
            {
                Data.AddNewDrone(NewDrone, StationID);              ///Sending the new drone to the data
            }
            catch (DroneExistException exp)
            {
                throw new DroneExistExceptionBL(exp.Message);
            }
            catch(StationExistException exp)
            {
                throw new StationExistExceptionBL(exp.Message);
            }
            
            DroneToList NewDroneToList = new DroneToList(DroneBL.ID, DroneBL.Model, DroneBL.MaxWeight, DroneBL.BatteryStatus, DroneBL.Status, DroneBL.CurrentLocation, 0);
            DroneList.Add(NewDroneToList);      ///Saving a logic version of the new drone
            
        }
        
        public void AddNewCustomer(CustomerBL customer)
        {
            if (customer.ID < 100000000 || customer.ID > 999999999)
                throw new InvalidIDException("Invalid customer ID number");
            char str = customer.Phone[0];
            bool success = int.TryParse(customer.Phone, out int PhoneNumber);
            if (!success || str != '0' || PhoneNumber < 500000000 || PhoneNumber > 599999999) ///Checking if the number starts with a '05' and contain 10 numbers
                throw new InvalidPhoneNumberException("Invalid phone number");
            if ((int)customer.Location.Latitude != 31 || (int)customer.Location.Longitude != 35)
                throw new OutOfRangeLocationException("The location is outside of Jerusalem"); ///We assume for now that all the locations
            Customer NewCustomer = new Customer(customer.ID, customer.Name, customer.Phone, customer.Location.Latitude, customer.Location.Longitude);
            Data.AddNewCustomer(NewCustomer);
        }
       
        public void AddNewParcel(ParcelBL parcel)
        {
            if (parcel.Sender.ID < 100000000 || parcel.Sender.ID > 999999999)
                throw new InvalidIDException("Invalid sender ID number");
            if (parcel.Target.ID < 100000000 || parcel.Target.ID > 999999999)
                throw new InvalidIDException("Invalid receiver ID number");
            Parcel ParcelDO = new Parcel(parcel.ID, parcel.Sender.ID, parcel.Target.ID, 0, parcel.Weight, parcel.Priority, parcel.TimeRequested, parcel.Scheduled, parcel.PickedUp, parcel.Delivered);
            Data.AddNewParcel(ParcelDO);
        }
    }
}
