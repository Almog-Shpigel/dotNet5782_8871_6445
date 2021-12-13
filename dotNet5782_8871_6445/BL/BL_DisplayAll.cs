using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DO;
using static BO.EnumsBL;

namespace BlApi
{
    partial class BL
    {
        
        public List<StationToList> DispalyAllStations()
        {
            List<StationToList> stations = new();
            foreach (Station station in Data.GetStations(station => true))
            {
                StationToList NewStation = new();
                NewStation.ID = station.ID;
                NewStation.Name = station.Name;
                NewStation.AvailableChargeSlots = station.ChargeSlots;
                foreach (DO.DroneCharge drone in Data.GetDroneCharge(droneCharge => true))
                {
                    if (drone.StationID == NewStation.ID)
                        NewStation.UsedChargeSlots++;
                }
                stations.Add(NewStation);
            }
            return stations;
        }

        public List<DroneToList> GetDrones(DroneStatus status, WeightCategories weight)
        {
            List<DroneToList> SelectedDrones = DroneList;
            if (weight != WeightCategories.Any && (int)weight != -1)
                SelectedDrones = SelectedDrones.Where(drone => drone.MaxWeight == weight).ToList();
            if (status != DroneStatus.Any && (int)status != -1)
                SelectedDrones = SelectedDrones.Where(drone => drone.Status == status).ToList();
            return SelectedDrones;
        }
        
        public List<CustomerToList> DispalyAllCustomers()
        {
            List<CustomerToList> customers = new();
            foreach (Customer customer in Data.GetCustomers(customer => true))
            {
                CustomerToList NewCustomer = new(customer.ID, customer.Name, customer.Phone);
                
                foreach (Parcel parcel in Data.GetParcels(parcel => true))
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
            foreach (Parcel parcel in Data.GetParcels(parcel => true))
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
            foreach (Parcel parcel in Data.GetParcels(parcel => true))
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
       
        public List<StationToList> GetAllAvailableStationsToList()
        {
            List<StationToList> AvailableStations = new();

            foreach (Station station in Data.GetStations(station => station.ChargeSlots > 0))
            {
                StationToList NewStation = new(station.ID, station.Name, station.ChargeSlots);
                NewStation.UsedChargeSlots = Data.GetDroneCharge(droneCharge => droneCharge.StationID == station.ID).Count();
                AvailableStations.Add(NewStation);
            }
            return AvailableStations;
        }
    }
}
