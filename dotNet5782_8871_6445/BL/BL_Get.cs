using System.Collections.Generic;
using System.Linq;
using DO;
using BO;
using static BO.EnumsBL;
using System;
using System.Runtime.CompilerServices;


namespace BlApi
{
    partial class BL
    {
        #region Get All
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetAllDrones()
        {
            lock (Data)
            {
                return DroneList.Select(d => new DroneToList(d.ID, d.Model, d.MaxWeight, (int)d.BatteryStatus, d.Status, d.CurrentLocation, d.ParcelID));
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetAllParcels()
        {
            lock (Data)
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
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> GetAllStations()
        {
            lock (Data)
            {
                IEnumerable<StationToList> stations = Data.GetStations(station => true).Select(station => new StationToList(station.ID, station.Name, station.ChargeSlots,
                Data.GetDroneCharge(droneCharge => droneCharge.StationID == station.ID).Count()));
                return stations;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerToList> GetAllCustomers()
        {
            lock (Data)
            {
                List<CustomerToList> customers = new();
                foreach (Customer customer in Data.GetCustomers(customer => true))
                {
                    CustomerToList NewCustomer = new(customer.ID, customer.Name, customer.Phone);
                    NewCustomer.ParcelsOnTheWay = Data.GetParcels(parcel => parcel.TargetID == NewCustomer.ID && parcel.Delivered == null).Count();
                    NewCustomer.ParcelsRecived = Data.GetParcels(parcel => parcel.TargetID == NewCustomer.ID && parcel.Delivered != null).Count();
                    NewCustomer.SentAndNOTDeliverd = Data.GetParcels(parcel => parcel.SenderID == NewCustomer.ID && parcel.Delivered == null).Count();
                    NewCustomer.SentAndDeliverd = Data.GetParcels(parcel => parcel.SenderID == NewCustomer.ID && parcel.Delivered != null).Count();
                    customers.Add(NewCustomer);
                }
                return customers;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GatAllStationsDO()
        {
            lock (Data)
            {
                return Data.GetStations(station => true);
            }
        }
        #endregion

        #region Get Some


        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> GetAvailableStations()
        {
            lock (Data)
            {
                IEnumerable<StationToList> AvailableStations = Data
                .GetStations(station => station.ChargeSlots > 0)
                .Select(station => new StationToList()
                {
                    ID = station.ID,
                    AvailableChargeSlots = station.ChargeSlots,
                    Name = station.Name,
                    UsedChargeSlots = Data.GetDroneCharge(droneCharge => droneCharge.StationID == station.ID).Count()
                });
                return AvailableStations;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerInParcel> GetAllCustomerInParcels()
        {
            lock (Data)
            {
                IEnumerable<CustomerInParcel> customers = Data.GetCustomers(item => true).Select(customer => new CustomerInParcel(customer.ID, customer.Name));
                return customers;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetDrones(DroneStatus status, WeightCategories weight)
        {
            lock (Data)
            {
                IEnumerable<DroneToList> SelectedDrones = GetAllDrones();
                if ((int)weight != -1)
                    SelectedDrones = SelectedDrones.Where(drone => drone.MaxWeight == weight);
                if ((int)status != -1)
                    SelectedDrones = SelectedDrones.Where(drone => drone.Status == status);
                return SelectedDrones;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetParcels(Priorities priorities, WeightCategories weight, ParcelStatus status, DateTime? from, DateTime? to)
        {
            lock (Data)
            {
                IEnumerable<ParcelToList> SelectedParcels = GetAllParcels();
                if ((int)weight != -1)
                    SelectedParcels = SelectedParcels.Where(parcel => parcel.Weight == weight);
                if ((int)priorities != -1)
                    SelectedParcels = SelectedParcels.Where(parcel => parcel.Priority == priorities);
                if ((int)status != -1)
                    SelectedParcels = SelectedParcels.Where(parcel => parcel.Status == status);
                if (from != null)
                    SelectedParcels = SelectedParcels.Where(parcel => from <= Data.GetParcel(parcel.ID).TimeRequested);
                if (to != null)
                    SelectedParcels = SelectedParcels.Where(parcel => to >= Data.GetParcel(parcel.ID).TimeRequested);
                return SelectedParcels;
            }
        }

        #endregion

        #region Get one
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneBL GetDrone(int DroneID)
        {
            lock (Data)
            {
                DroneToList drone;
                if (DroneList.Any(d => d.ID == DroneID))
                    drone = DroneList.Where(d => d.ID == DroneID).First();
                else
                    throw new DroneExistException();

                DroneBL droneBL = new(drone.ID, drone.Model, drone.MaxWeight, (int)drone.BatteryStatus, drone.Status);
                droneBL.CurrentLocation = drone.CurrentLocation;
                if (drone.ParcelID == 0)
                    return droneBL;
                if (droneBL.Status == DroneStatus.Delivery)
                    droneBL.Parcel = InitParcelInDelivery(Data.GetParcel(drone.ParcelID));
                droneBL.Parcel.DeliveryDistance = Distance(droneBL.CurrentLocation, droneBL.Parcel.TargetLocation);
                return droneBL;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public ParcelBL GetParcel(int ParcelID)
        {
            lock (Data)
            {
                Parcel parcel = Data.GetParcel(ParcelID);
                Customer sender = Data.GetCustomer(parcel.SenderID);
                Customer target = Data.GetCustomer(parcel.TargetID);
                ParcelBL parcelBL = new(new(sender.ID, sender.Name), new(target.ID, target.Name), parcel.Weight, parcel.Priority);
                parcelBL.ID = parcel.ID;
                parcelBL.TimeRequested = parcel.TimeRequested;
                parcelBL.Scheduled = parcel.Scheduled;
                parcelBL.PickedUp = parcel.PickedUp;
                parcelBL.Delivered = parcel.Delivered;
                parcelBL.Sender.Name = sender.Name;
                parcelBL.Target.Name = target.Name;
                if (parcel.DroneID != 0)
                    parcelBL.DroneInParcel = CreateDroneInParcel(parcel.DroneID);
                return parcelBL;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public StationBL GetStation(int StationID)
        {
            lock (Data)
            {
                Station station;
                try
                {
                    station = Data.GetStation(StationID);
                }
                catch (StationExistException ex)
                {
                    throw new EntityExistException("Wrong id.\n ", ex);
                }

                Location location = new(station.Latitude, station.Longitude);
                StationBL stationBL = new(station.ID, station.Name, station.ChargeSlots, location);
                foreach (DroneCharge droneCharge in Data.GetDroneCharge(droneCharge => droneCharge.StationID == station.ID))
                {
                    foreach (DroneToList DroneItem in DroneList.Where(d => d.ID == droneCharge.DroneID))
                    {
                        DroneChargeBL drone = new(droneCharge.DroneID, DroneItem.BatteryStatus);
                        stationBL.ChargingDrones.Add(drone);
                    }
                }
                return stationBL;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public CustomerBL GetCustomer(int CustomerID)
        {
            lock (Data)
            {
                Customer customer;
                try
                {
                    customer = Data.GetCustomer(CustomerID);
                }
                catch (CustomerExistException ex)
                {
                    throw new InvalidIDException("Wrong id!\n", ex);
                }

                Location location = new(customer.Latitude, customer.Longitude);
                CustomerBL CustomerToDisplay = new CustomerBL(customer.ID, customer.Name, customer.Phone, location);
                foreach (var parcel in Data.GetParcels(parcel => true))
                {
                    if (parcel.SenderID == CustomerToDisplay.ID)
                        CustomerToDisplay.ParcelesSentByCustomer.Add(CreateParcelAtCustomer(parcel, parcel.TargetID));
                    if (parcel.TargetID == CustomerToDisplay.ID)
                        CustomerToDisplay.ParcelesSentToCustomer.Add(CreateParcelAtCustomer(parcel, parcel.SenderID));
                }
                return CustomerToDisplay;
            }
        }
        #endregion

        #region Get other
        [MethodImpl(MethodImplOptions.Synchronized)]
        public ParcelToList GetParcelToList(ParcelAtCustomer parcel)
        {
            lock (Data)
            {
                string target = Data.GetCustomer(Data.GetParcel(parcel.ID).TargetID).Name;
                ParcelToList parcelToList = new(parcel.ID, parcel.Customer.Name, target, parcel.Weight, parcel.Priority, parcel.Status);
                return parcelToList;
            }
        }

        /// <summary>
        /// Function that receive a parcel and initialize ParcelInDelivery entity 
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns> ParcelInDelivery entity </returns>
        private ParcelInDelivery InitParcelInDelivery(Parcel parcel)
        {
            lock (Data)
            {
                Customer sender = Data.GetCustomer(parcel.SenderID), target = Data.GetCustomer(parcel.TargetID);
                CustomerInParcel customer1 = new(sender.ID, sender.Name), customer2 = new(target.ID, target.Name);
                Location pickUpLocation = new(sender.Latitude, sender.Longitude), targetLocation = new(target.Latitude, target.Longitude);
                double distance = DistanceCustomerCustomer(sender, target);
                ParcelInDelivery UpdateParcelInDelivery = new(parcel.ID, parcel.Weight, parcel.Priority, customer1, customer2, pickUpLocation, targetLocation, distance);
                return UpdateParcelInDelivery;
            }
        }

        /// <summary>
        /// Receive a parcel and customer id and create a ParcelAtCustomer entity
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="CustomerID"></param>
        /// <returns>ParcelAtCustomer entity</returns>
        private ParcelAtCustomer CreateParcelAtCustomer(Parcel parcel, int customerID)
        {
            lock (Data)
            {
                ParcelStatus status = GetParcelStatus(parcel);
                Customer cust = Data.GetCustomer(customerID);
                CustomerInParcel customer = new(cust.ID, cust.Name);
                ParcelAtCustomer NewParcel = new(parcel.ID, parcel.Weight, parcel.Priority, status, customer);
                return NewParcel;
            }
        }

        /// <summary>
        /// Receiving the parcel's status according to it's updated times.
        /// Requested -> Schedualed -> PickedUp -> Delivered
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns>Parcel status</returns>
        private ParcelStatus GetParcelStatus(Parcel parcel)
        {
            if (parcel.Scheduled == null)
                return ParcelStatus.Requested;
            if (parcel.PickedUp == null)
                return ParcelStatus.Scheduled;
            if (parcel.Delivered == null)
                return ParcelStatus.PickedUp;
            return ParcelStatus.Delivered;
        }

        /// <summary>
        /// Receive drone id and create a DroneInParcel entity
        /// </summary>
        /// <param name="droneID"></param>
        /// <returns>DroneInParcel entity</returns>
        private DroneInParcel CreateDroneInParcel(int droneID)
        {
            if (!DroneList.Any(d => d.ID == droneID))
                throw new EntityExistException($"Drone {droneID} doesn't exist in the data!");
            DroneToList drone = DroneList.Find(d => d.ID == droneID);
            return new DroneInParcel(drone.ID, drone.BatteryStatus, drone.CurrentLocation);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetParcelsGroupBy(string groupByString)
        {
            List<ParcelToList> groupedList = new();
            switch(groupByString)
            {
                case "sender":
                    {
                        var parcelList = GetAllParcels().GroupBy(p => p.SenderName);
                        foreach (var groupedParcels in parcelList)
                            foreach(var parcel in groupedParcels)
                                groupedList.Add(parcel);
                        break;
                    }
                case "reciver":
                    {
                        var parcelList = GetAllParcels().GroupBy(p => p.TargetName);
                        foreach (var groupedParcels in parcelList)
                            foreach (var parcel in groupedParcels)
                                groupedList.Add(parcel);
                        break;
                    }
                default:
                    throw new InvalidInputException("Invalid grouping paremeter");
            }
            return groupedList;
        }

        /// <summary>
        /// The function returns the location of the nearest available station.
        /// <para>
        /// If there is no available station, the function will <b>throw</b> an StationExistException.
        /// </para>
        /// </summary>
        /// <returns></returns>
        internal Station GetNearestStation(Location location, IEnumerable<Station> AllStations)
        {
            lock (Data)
            {
                if (!AllStations.Any())
                    throw new NoAvailableStation("There are no station available.");
                Station nearestStation = AllStations.First();
                double distance, minDistance;
                minDistance = Distance(location, new(nearestStation.Latitude, nearestStation.Longitude));
                foreach (Station station in AllStations)
                {
                    distance = Distance(location, new(station.Latitude, station.Longitude));
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestStation = station;
                    }
                }
                return nearestStation;
            }
        }
        #endregion
    }
}
