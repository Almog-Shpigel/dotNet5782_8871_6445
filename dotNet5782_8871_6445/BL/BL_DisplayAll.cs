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
        
        public List<StationToList> DispalyAllStations()
        {
            List<StationToList> stations = new();
            foreach (Station station in Data.GetAllStations())
            {
                StationToList NewStation = new();
                NewStation.ID = station.ID;
                NewStation.Name = station.Name;
                NewStation.AvailableChargeSlots = station.ChargeSlots;
                foreach (IDAL.DO.DroneCharge drone in Data.GetAllDronesCharge())
                {
                    if (drone.StationID == NewStation.ID)
                        NewStation.UsedChargeSlots++;
                }
                stations.Add(NewStation);
            }
            return stations;
        }

        public List<DroneToList> DispalyAllDrones()
        {
            return DroneList;
        }
        
        public List<CustomerToList> DispalyAllCustomers()
        {
            List<CustomerToList> customers = new();
            foreach (Customer customer in Data.GetAllCustomers())
            {
                CustomerToList NewCustomer = new();
                NewCustomer.ID = customer.ID;
                NewCustomer.Name = customer.Name;
                NewCustomer.Phone = customer.Phone;
                foreach (Parcel parcel in Data.GetAllParcels())
                {
                    if (parcel.TargetID == customer.ID)
                    {
                        if (parcel.Delivered == null)
                            NewCustomer.ParcelsOnTheWay++;
                        else
                            NewCustomer.ParcelsRecived++;
                    }
                    if (parcel.SenderID == customer.ID)
                    {
                        if (parcel.Delivered == null)
                            NewCustomer.SentAndNOTDeliverd++;
                        else
                            NewCustomer.SentAndDeliverd++;
                    }
                }
                customers.Add(NewCustomer);
            }
            return customers;
        }
        
        public List<ParcelToList> DispalyAllParcels()
        {
            List<ParcelToList> parcels = new();
            foreach (Parcel parcel in Data.GetAllParcels())
            {
                ParcelToList NewParcel = new();
                NewParcel.ID = parcel.ID;
                NewParcel.Priority = parcel.Priority;
                NewParcel.TargetName = Data.GetCustomer(parcel.TargetID).Name;
                NewParcel.SenderName = Data.GetCustomer(parcel.SenderID).Name;
                NewParcel.Weight = parcel.Weight;
                if (parcel.Delivered != null)
                    NewParcel.Status = ParcelStatus.Delivered;
                else if (parcel.PickedUp != null)
                    NewParcel.Status = ParcelStatus.PickedUp;
                else if (parcel.Scheduled != null)
                    NewParcel.Status = ParcelStatus.Scheduled;
                else
                    NewParcel.Status = ParcelStatus.Requested;
                parcels.Add(NewParcel);
            }
            return parcels;
        }
        
        public List<ParcelToList> DispalyAllUnassignedParcels()
        {
            List<ParcelToList> UnassignedParcels = new();
            foreach (Parcel parcel in Data.GetAllParcels())
            {
                ParcelToList NewParcel = new();
                if (parcel.DroneID == 0)
                {
                    NewParcel.ID = parcel.ID;
                    NewParcel.Priority = parcel.Priority;
                    NewParcel.TargetName = Data.GetCustomer(parcel.TargetID).Name;
                    NewParcel.SenderName = Data.GetCustomer(parcel.SenderID).Name;
                    NewParcel.Weight = parcel.Weight;
                    if (parcel.Delivered != null)
                        NewParcel.Status = ParcelStatus.Delivered;
                    else if (parcel.PickedUp != null)
                        NewParcel.Status = ParcelStatus.PickedUp;
                    else if (parcel.Scheduled != null)
                        NewParcel.Status = ParcelStatus.Scheduled;
                    else
                        NewParcel.Status = ParcelStatus.Requested;
                    UnassignedParcels.Add(NewParcel);
                }
            }
            return UnassignedParcels;
        }
       
        public List<StationToList> DispalyAllAvailableStations()
        {
            List<StationToList> AvailableStations = new();
            foreach (Station station in Data.GetAllStations())
            {
                StationToList NewStation = new();
                NewStation.ID = station.ID;
                NewStation.Name = station.Name;
                NewStation.AvailableChargeSlots = station.ChargeSlots;
                foreach (IDAL.DO.DroneCharge drone in Data.GetAllDronesCharge())
                {
                    if (drone.StationID == station.ID)
                        NewStation.UsedChargeSlots++;
                }
                AvailableStations.Add(NewStation);
            }
            return AvailableStations;
        }
    }
}
