using System.Collections.Generic;
using System.Linq;
using DO;
using BO;
using static BO.EnumsBL;

namespace BlApi
{
    partial class BL
    {
        #region Get All
        public IEnumerable<DroneToList> GetAllDrones()
        {
            return DroneList;
        }

        public IEnumerable<ParcelToList> GetAllParcels()
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

        public IEnumerable<StationToList> GetAllStations()
        {
            List<StationToList> stations = new();
            foreach (Station station in Data.GetStations(station => true))
            {
                StationToList NewStation = new();
                NewStation.ID = station.ID;
                NewStation.Name = station.Name;
                NewStation.AvailableChargeSlots = station.ChargeSlots;
                NewStation.UsedChargeSlots = Data.GetDroneCharge(droneCharge => droneCharge.StationID == NewStation.ID).Count();
                stations.Add(NewStation);
            }
            return stations;
        }

        public IEnumerable<CustomerToList> GetAllCustomers()
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

        public IEnumerable<Station> GatAllStationsDO()
        {
            return Data.GetStations(station => true);
        }
        #endregion

        #region Get Some
        public IEnumerable<ParcelToList> GetUnassignedParcels()
        {
            IEnumerable<ParcelToList> UnassignedParcels = Data
                .GetParcels(parcel => parcel.ID == 0)
                .Select(parcel => new ParcelToList()
                {
                    ID = parcel.ID,
                    SenderName = Data.GetCustomer(parcel.SenderID).Name,
                    TargetName = Data.GetCustomer(parcel.TargetID).Name,
                    Weight = parcel.Weight,
                    Priority = parcel.Priority,
                    Status = GetParcelStatus(parcel)
                });
            return UnassignedParcels;
        }

        public IEnumerable<StationToList> GetAvailableStations()
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

        public IEnumerable<DroneToList> GetDrones(DroneStatus status, WeightCategories weight)
        {
            IEnumerable<DroneToList> SelectedDrones = DroneList;
            if ((int)weight != -1)
                SelectedDrones = SelectedDrones.Where(drone => drone.MaxWeight == weight);
            if ((int)status != -1)
                SelectedDrones = SelectedDrones.Where(drone => drone.Status == status);
            return SelectedDrones;
        }

        public IEnumerable<ParcelToList> GetParcels(Priorities priorities, WeightCategories weight)
        {
            IEnumerable<ParcelToList> SelectedParcels = GetAllParcels();
            if ((int)weight != -1)
                SelectedParcels = SelectedParcels.Where(parcel => parcel.Weight == weight);
            if ((int)priorities != -1)
                SelectedParcels = SelectedParcels.Where(parcel => parcel.Priority == priorities);
            return SelectedParcels;
        }
        #endregion

        #region Get one
        public DroneBL GetDrone(int DroneID)
        {
            DroneBL DroneToDisplay;
            foreach (DroneToList drone in DroneList)
                if (drone.ID == DroneID)
                {
                    DroneToDisplay = new(drone.ID, drone.Model, drone.MaxWeight, drone.BatteryStatus, drone.Status); /// battery status needs to be updated every time
                    DroneToDisplay.CurrentLocation = drone.CurrentLocation;
                    if (drone.ParcelID == 0)
                        return DroneToDisplay;
                    if (DroneToDisplay.Status == DroneStatus.Delivery)
                    {
                        DroneToDisplay.Parcel = InitParcelInDelivery(Data.GetParcel(drone.ParcelID));
                    }
                    return DroneToDisplay;
                }
            throw new DroneExistException();
        }

        public ParcelBL GetParcel(int ParcelID)
        {
            Parcel parcel = Data.GetParcel(ParcelID);
            Customer sender = Data.GetCustomer(parcel.SenderID);
            Customer target = Data.GetCustomer(parcel.TargetID);
            ParcelBL ParcelToDisplay = new(new(sender.ID,sender.Name), new(target.ID, target.Name), parcel.Weight, parcel.Priority);
            ParcelToDisplay.ID = parcel.ID;
            ParcelToDisplay.TimeRequested = parcel.TimeRequested;
            ParcelToDisplay.Scheduled = parcel.Scheduled;
            ParcelToDisplay.PickedUp = parcel.PickedUp;
            ParcelToDisplay.Delivered = parcel.Delivered;
            ParcelToDisplay.Sender.Name = sender.Name;
            ParcelToDisplay.Target.Name = target.Name;
            if (parcel.DroneID != 0)
                ParcelToDisplay.DroneInParcel = CreateDroneInParcel(parcel.DroneID);
            return ParcelToDisplay;
        }

        public StationBL GetStation(int StationID)
        {
            Station station = Data.GetStation(StationID);
            Location location = new(station.Latitude, station.Longitude);
            StationBL StationToPrint = new(station.ID, station.Name, station.ChargeSlots, location);
            foreach (DroneCharge DroneCharge in Data.GetDroneCharge(droneCharge => true))
            {
                foreach (DroneToList DroneItem in DroneList)
                {
                    if (DroneItem.ID == DroneCharge.DroneID && DroneCharge.StationID == StationID)
                    {
                        DroneChargeBL drone = new(DroneCharge.DroneID, DroneItem.BatteryStatus);
                        StationToPrint.ChargingDrones.Add(drone);
                    }
                }
            }
            return StationToPrint;
        }
        public IEnumerable<CustomerInParcel> GetAllCustomerInParcels()
        {
            IEnumerable<CustomerInParcel> customers = Data.GetCustomers(item => true).Select(customer => new CustomerInParcel(customer.ID, customer.Name));
            return customers;
        }

        public CustomerBL GetCustomer(int CustomerID)
        {
            Customer customer;
            try
            {
                customer = Data.GetCustomer(CustomerID);
            }
            catch (CustomerExistException msg)
            {
                throw new InvalidIDException("Wrong id!\n", msg);
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
        #endregion

        #region Get other
        private DroneToList GetDroneToList(int DroneID)
        {
            foreach (DroneToList drone in DroneList)
                if (drone.ID == DroneID)
                    return drone;
            throw new DroneExistException();
        }
        public ParcelToList GetParcelToList(ParcelAtCustomer parcel)
        {
            string target = Data.GetCustomer((Data.GetParcel(parcel.ID).TargetID)).Name;
            ParcelToList parcelToList = new(parcel.ID, parcel.Customer.Name, target, parcel.Weight, parcel.Priority, parcel.Status);
            return parcelToList;
        }

        /// <summary>
        /// Function that receive a parcel and initialize ParcelInDelivery entity 
        /// </summary>
        /// <param name="parcel"></param>
        /// <returns> ParcelInDelivery entity </returns>
        private ParcelInDelivery InitParcelInDelivery(Parcel parcel)
        {
            Customer sender = Data.GetCustomer(parcel.SenderID), target = Data.GetCustomer(parcel.TargetID);
            CustomerInParcel customer1 = new(sender.ID, sender.Name), customer2 = new(target.ID, target.Name);
            Location pickUpLocation = new(sender.Latitude, sender.Longitude), targetLocation = new(target.Latitude, target.Longitude);
            double distance = DistanceCustomerCustomer(sender.ID, target.ID);
            ParcelInDelivery UpdateParcelInDelivery = new(parcel.ID, parcel.Weight, parcel.Priority, customer1, customer2, pickUpLocation, targetLocation, distance);
            return UpdateParcelInDelivery;
        }

        /// <summary>
        /// Receive a parcel and customer id and create a ParcelAtCustomer entity
        /// </summary>
        /// <param name="parcel"></param>
        /// <param name="CustomerID"></param>
        /// <returns>ParcelAtCustomer entity</returns>
        private ParcelAtCustomer CreateParcelAtCustomer(Parcel parcel, int CustomerID)
        {
            ParcelStatus status = GetParcelStatus(parcel);
            Customer cust = Data.GetCustomer(CustomerID);
            CustomerInParcel customer = new(cust.ID, cust.Name);
            ParcelAtCustomer NewParcel = new(parcel.ID, parcel.Weight, parcel.Priority, status, customer);
            return NewParcel;

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
            foreach (var drone in DroneList)
            {
                if (drone.ID == droneID)
                    return new DroneInParcel(drone.ID, drone.BatteryStatus, drone.CurrentLocation);
            }
            throw new DroneExistExceptionBL();
        }

        public string GetDistanceFromStation(double latitude, double longitude, int StationID)
        {
            Station station = Data.GetStation(StationID);
            Location StationLocation = new(station.Latitude, station.Longitude), location = new(latitude, longitude);
            return "The distance is: " + Distance(StationLocation, location) + " km";
        }

        public string GetDistanceFromCustomer(double longitude1, double latitude1, int CustomerID)
        {
            Customer customer = Data.GetCustomer(CustomerID);
            Location CustomerLocation = new(customer.Latitude, customer.Longitude), location = new(latitude1, longitude1);
            return "The distance is: " + Distance(CustomerLocation, location) + " km";
        }

        /// <summary>
        /// The function returns the location of the nearest available station.
        /// <para>
        /// If there is no available station, the function will <b>throw</b> an StationExistException.
        /// </para>
        /// </summary>
        /// <returns></returns>
        private Station GetNearestStation(Location location, IEnumerable<Station> AllStation)
        {
            if (AllStation.Count() == 0)
                throw new NoAvailableStation("There are no station available.");
            Station NearestStation = new(AllStation.First().ID,
                AllStation.First().Name,
                AllStation.First().ChargeSlots,
                AllStation.First().Latitude,
                AllStation.First().Longitude);

            Location StationLocation = new(NearestStation.Latitude, NearestStation.Longitude);
            double distance, MinDistance;
            MinDistance = Distance(location, StationLocation);
            foreach (Station station in AllStation)
            {
                StationLocation = new(station.Latitude, station.Longitude);
                distance = Distance(location, StationLocation);
                if (distance < MinDistance)
                {
                    MinDistance = distance;
                    NearestStation = new(station.ID, station.Name, station.ChargeSlots, station.Latitude, station.Longitude);
                }
            }
            return NearestStation;
        }
        #endregion
    }
}
