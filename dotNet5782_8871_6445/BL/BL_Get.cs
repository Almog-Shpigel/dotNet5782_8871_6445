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
                    ParcelToList newParcel = new();
                    newParcel.ID = parcel.ID;
                    newParcel.Priority = parcel.Priority;
                    try
                    {
                        newParcel.TargetName = Data.GetCustomer(parcel.TargetID).Name;
                        newParcel.SenderName = Data.GetCustomer(parcel.SenderID).Name;
                    }
                    catch (CustomerExistException ex)
                    {
                        throw new EntityExistException("Invalid id. \n " + ex.Message);
                    }
                    
                    newParcel.Weight = parcel.Weight;
                    if (parcel.Delivered != null)
                        newParcel.Status = ParcelStatus.Delivered;
                    else if (parcel.PickedUp != null)
                        newParcel.Status = ParcelStatus.PickedUp;
                    else if (parcel.Scheduled != null)
                        newParcel.Status = ParcelStatus.Scheduled;
                    else
                        newParcel.Status = ParcelStatus.Requested;
                    parcels.Add(newParcel);
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
                return stations.OrderBy(s => s.ID);
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
                    CustomerToList newCustomer = new(customer.ID, customer.Name, customer.Phone);
                    newCustomer.ParcelsOnTheWay = Data.GetParcels(parcel => parcel.TargetID == newCustomer.ID && parcel.Delivered == null).Count();
                    newCustomer.ParcelsRecived = Data.GetParcels(parcel => parcel.TargetID == newCustomer.ID && parcel.Delivered != null).Count();
                    newCustomer.SentAndNOTDeliverd = Data.GetParcels(parcel => parcel.SenderID == newCustomer.ID && parcel.Delivered == null).Count();
                    newCustomer.SentAndDeliverd = Data.GetParcels(parcel => parcel.SenderID == newCustomer.ID && parcel.Delivered != null).Count();
                    customers.Add(newCustomer);
                }
                return customers;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GatAllStationsDO()
        {
            lock (Data)
            {
                return Data.GetStations(station => true).OrderBy(s => s.ID);
            }
        }
        #endregion

        #region Get Some


        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> GetAvailableStations()
        {
            lock (Data)
            {
                IEnumerable<StationToList> availableStations = Data
                .GetStations(station => station.ChargeSlots > 0)
                .Select(station => new StationToList()
                {
                    ID = station.ID,
                    AvailableChargeSlots = station.ChargeSlots,
                    Name = station.Name,
                    UsedChargeSlots = Data.GetDroneCharge(droneCharge => droneCharge.StationID == station.ID).Count()
                });
                return availableStations;
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
                IEnumerable<DroneToList> selectedDrones = GetAllDrones();
                if ((int)weight != -1)
                    selectedDrones = selectedDrones.Where(drone => drone.MaxWeight == weight);
                if ((int)status != -1)
                    selectedDrones = selectedDrones.Where(drone => drone.Status == status);
                return selectedDrones;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetParcels(Priorities priorities, WeightCategories weight, ParcelStatus status, DateTime? from, DateTime? to)
        {
            lock (Data)
            {
                IEnumerable<ParcelToList> selectedParcels = GetAllParcels();
                if ((int)weight != -1)
                    selectedParcels = selectedParcels.Where(parcel => parcel.Weight == weight);
                if ((int)priorities != -1)
                    selectedParcels = selectedParcels.Where(parcel => parcel.Priority == priorities);
                if ((int)status != -1)
                    selectedParcels = selectedParcels.Where(parcel => parcel.Status == status);
                if (from != null)
                    selectedParcels = selectedParcels.Where(parcel => from <= Data.GetParcel(parcel.ID).TimeRequested);
                if (to != null)
                    selectedParcels = selectedParcels.Where(parcel => to >= Data.GetParcel(parcel.ID).TimeRequested);
                return selectedParcels;
            }
        }

        #endregion

        #region Get one
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneBL GetDrone(int droneID)
        {
            lock (Data)
            {
                DroneToList drone;
                if (DroneList.Any(d => d.ID == droneID))
                    drone = DroneList.Where(d => d.ID == droneID).First();
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
        public ParcelBL GetParcel(int parcelID)
        {
            lock (Data)
            {
                Parcel parcel;
                Customer sender,target;
                try
                {
                    parcel = Data.GetParcel(parcelID);
                }
                catch (ParcelExistException ex)
                {
                    throw new EntityExistException("Invalid id. \n "+ ex.Message);
                }
                try
                {
                    sender = Data.GetCustomer(parcel.SenderID);
                    target = Data.GetCustomer(parcel.TargetID);
                }
                catch (CustomerExistException ex)
                {
                    throw new EntityExistException("Invalid id. \n"+ ex.Message);
                }
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
        public StationBL GetStation(int stationID)
        {
            lock (Data)
            {
                Station station;
                try
                {
                    station = Data.GetStation(stationID);
                }
                catch (StationExistException ex)
                {
                    throw new EntityExistException("Invalid id. \n" + ex.Message);
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
        public CustomerBL GetCustomer(int customerID)
        {
            lock (Data)
            {
                Customer customer;
                try
                {
                    customer = Data.GetCustomer(customerID);
                }
                catch (CustomerExistException ex)
                {
                    throw new InvalidIDException("Invalid id. \n" + ex.Message);
                }

                Location location = new(customer.Latitude, customer.Longitude);
                CustomerBL customerToDisplay = new CustomerBL(customer.ID, customer.Name, customer.Phone, location);
                foreach (var parcel in Data.GetParcels(parcel => true))
                {
                    if (parcel.SenderID == customerToDisplay.ID)
                        customerToDisplay.ParcelesSentByCustomer.Add(CreateParcelAtCustomer(parcel, parcel.TargetID));
                    if (parcel.TargetID == customerToDisplay.ID)
                        customerToDisplay.ParcelesSentToCustomer.Add(CreateParcelAtCustomer(parcel, parcel.SenderID));
                }
                return customerToDisplay;
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
                ParcelInDelivery updateParcelInDelivery = new(parcel.ID, parcel.Weight, parcel.Priority, customer1, customer2, pickUpLocation, targetLocation, distance);
                return updateParcelInDelivery;
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
                ParcelAtCustomer newParcel = new(parcel.ID, parcel.Weight, parcel.Priority, status, customer);
                return newParcel;
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
        internal Station GetNearestStation(Location location, IEnumerable<Station> allStations)
        {
            lock (Data)
            {
                if (!allStations.Any())
                    throw new NoAvailableStation("There are no station available.");
                Station nearestStation = allStations.First();
                double distance, minDistance;
                minDistance = Distance(location, new(nearestStation.Latitude, nearestStation.Longitude));
                foreach (Station station in allStations)
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
