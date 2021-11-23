﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalObject;
using IBL.BO;
using IDAL;
using IDAL.DO;
using static IBL.BO.EnumsBL;
//using static IBL.BO.Exceptions;

namespace IBL
{
    public class BL: IBL
    {
        private IDal Data;
        private List<DroneToList> DroneList;
        private Double[] BatteryUsed;
        private static Random rand = new();

        public BL()
        {
            Data = new DalObject.DalObject();
            DroneList = new List<DroneToList>();
            BatteryUsed = Data.GetBatteryUsed();

            foreach (Drone drone in Data.GetAllDrones())        
            {
                DroneToList NewDrone = new();
                Parcel NewParcel = new(); 
                NewDrone.ID = drone.ID;                         
                NewDrone.Model = drone.Model;
                NewDrone.MaxWeight = drone.MaxWeight;
                foreach (Parcel parcel in Data.GetAllParcels())
                {
                    if (parcel.DroneID == drone.ID && parcel.Delivered == DateTime.MinValue)    
                        NewParcel = parcel;
                }
                if (NewParcel.DroneID == drone.ID && NewParcel.Delivered == DateTime.MinValue)  
                    NewDrone = InitDroneInDelivery(NewDrone, NewParcel);
                else                                                                            
                    NewDrone = InitDroneNOTinDelivery(NewDrone);
                
                DroneList.Add(NewDrone); 
            }
        }

        private DroneToList InitDroneInDelivery(DroneToList NewDrone, Parcel parcel)    ///Function to initialize a drone in delivery
        {
            double total;
            Random rand = new();
            NewDrone.Status = DroneStatus.Delivery;    ///Updating drone's status
            NewDrone.ParcelID = parcel.ID;
            Customer sender = Data.GetCustomer(parcel.SenderID), target = Data.GetCustomer(parcel.TargetID);
            Station NearestStatTarget = GetNearestStation(target.Latitude, target.Longitude, Data.GetAllStations());
            Station NearestStat = GetNearestStation(sender.Latitude, sender.Longitude, Data.GetAllStations());
            if (parcel.PickedUp == DateTime.MinValue)      ///Checking if the drone already picked up the parcel or not     
                NewDrone.CurrentLocation = new(NearestStat.Latitude, NearestStat.Longitude);
            else    ///means it did get pickedup
                NewDrone.CurrentLocation = new(sender.Latitude, sender.Longitude);

            total = BatteryUsageCurrStation(NewDrone, parcel.SenderID, parcel.TargetID, NearestStat, parcel.Weight); ///Returns the amount of battery needed to complete the delivery
            NewDrone.BatteryStatus = GetRandBatteryStatus(total, 100);  ///Choosing random battery number between the minimum needed to complete the delivery and full battery
            return NewDrone;
        }

        private DroneToList InitDroneNOTinDelivery(DroneToList NewDrone)
        {
            Random rand = new Random();
            Station nearest;
            int RandomStation, RandomCustomer;
            DroneStatus option = (DroneStatus) rand.Next(0, 2);     ///Random status, Available or Charging
            List<Station> AllAvailableStations = GetAllAvailableStations();
            List<Customer> AllPastCustomers = GetPastCustomers();
            if (AllPastCustomers.Count() == 0)                      ///We are assuming that the odds that there are no available stations are very unlikley
                option = DroneStatus. Charging;
            switch (option)
            {
                case DroneStatus.Available:
                    NewDrone.Status = DroneStatus.Available;
                    RandomCustomer = rand.Next(0, AllPastCustomers.Count());
                    NewDrone.CurrentLocation.Latitude = AllPastCustomers[RandomCustomer].Latitude;
                    NewDrone.CurrentLocation.Longitude = AllPastCustomers[RandomCustomer].Longitude;
                    nearest = GetNearestStation(NewDrone.CurrentLocation.Latitude, NewDrone.CurrentLocation.Longitude, Data.GetAllStations());
                    NewDrone.BatteryStatus = RandBatteryToStation(NewDrone, new Location(nearest.Latitude,nearest.Longitude), BatteryUsed[0]);
                    break;
                case DroneStatus.Charging:
                    NewDrone.Status = DroneStatus.Charging;
                    RandomStation = rand.Next(0, AllAvailableStations.Count());
                    Data.DroneToBeCharge(NewDrone.ID, AllAvailableStations[RandomStation].ID,DateTime.Now);
                    NewDrone.CurrentLocation.Latitude = AllAvailableStations[RandomStation].Latitude;
                    NewDrone.CurrentLocation.Longitude = AllAvailableStations[RandomStation].Longitude;
                    NewDrone.BatteryStatus = GetRandBatteryStatus(0,21);
                    break;
            }
            //NewDrone.Status =(DroneStatus)rand.Next(0,2);

            return NewDrone;
        }

        #region Add
        public void AddNewStation(StationBL StationBO)
        {
            if (StationBO.ID < 100000 || StationBO.ID > 999999)
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
            if (DroneBL.ID < 100000 || DroneBL.ID > 999999)
                throw new InvalidIDException("Drone ID has to have 6 positive digits.");
            DroneBL.BatteryStatus = GetRandBatteryStatus(20, 41);
            DroneBL.Status = DroneStatus.Charging;
            IEnumerable<Station> stations = Data.GetAllStations();
            foreach (Station station in stations)
                if (station.ID == StationID)
                {
                    if (station.ChargeSlots <= 0)
                        throw new InvalidSlotsException("There are no slots available at this station.");
                    DroneBL.CurrentLocation = new(station.Latitude, station.Longitude);
                }
            Drone NewDrone = new Drone(DroneBL.ID, DroneBL.Model, DroneBL.MaxWeight);
            Data.AddNewDrone(NewDrone, StationID);              ///Sending the new drone to the data
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
        #endregion

        #region Update
        public void UpdateDroneName(int id, string model)
        {
            if (id < 100000 || id > 999999)
                throw new InvalidIDException("Drone ID has to have 6 positive digits.");
            Data.UpdateDroneName(id, model);
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
            if (ParcelToBeDelivered.Delivered != DateTime.MinValue || ParcelToBeDelivered.PickedUp == DateTime.MinValue)
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
            for ( i = 0; i < DroneList.Count; i++)
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
            DroneToBeAvailable.BatteryStatus += BatteryUsed[5] * TimeCharged;
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

            //Need to Lesanen all parcel that are to far and that are not waiting for delivery
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
        #endregion

        #region Display
        public StationBL DisplayStation(int StationID)
        {
            Station station = Data.GetStation(StationID); 
            Location location = new(station.Latitude, station.Longitude);
            StationBL StationToPrint = new(station.ID,station.Name,station.ChargeSlots, location);
            foreach (DroneCharge DroneCharge in Data.GetAllDronesCharge())
            {
              foreach (DroneToList DroneItem in DroneList)
              {
                if(DroneItem.ID == DroneCharge.DroneID && DroneCharge.StationID == StationID)
                {
                   DroneChargeBL drone = new(DroneCharge.DroneID, DroneItem.BatteryStatus);
                   StationToPrint.ChargingDrones.Add(drone);
                }
              }
            }           
        return StationToPrint;
        }

        public DroneBL DisplayDrone(int DroneID)
        {
            DroneBL DroneToDisplay = new DroneBL();
            foreach (DroneToList drone in DroneList)
                if (drone.ID == DroneID)
                {
                    DroneToDisplay.ID = drone.ID;
                    DroneToDisplay.Model = drone.Model;
                    DroneToDisplay.MaxWeight = drone.MaxWeight;
                    DroneToDisplay.Status = drone.Status;
                    DroneToDisplay.CurrentLocation = drone.CurrentLocation;
                    DroneToDisplay.BatteryStatus = drone.BatteryStatus;
                    Parcel parcel = Data.GetParcel(drone.ParcelID);
                    if(DroneToDisplay.Status == DroneStatus.Delivery)
                    {
                        DroneToDisplay.Parcel = InitParcelInDelivery(parcel);
                    }
                    return DroneToDisplay;
                }
            throw new DroneExistException();
        }

        private ParcelInDelivery InitParcelInDelivery(Parcel parcel)
        {
            ParcelInDelivery ParcelDelivery = new ParcelInDelivery();
            ParcelDelivery.ID = parcel.ID;
            ParcelDelivery.Priority = parcel.Priority;
            ParcelDelivery.Weight = parcel.Weight;
            Customer sender = Data.GetCustomer(parcel.SenderID), target = Data.GetCustomer(parcel.TargetID);
            ParcelDelivery.Sender.ID = sender.ID;
            ParcelDelivery.Sender.Name = sender.Name;
            ParcelDelivery.Target.ID = target.ID;
            ParcelDelivery.Target.Name = target.Name;
            ParcelDelivery.PickUpLocation.Latitude = sender.Latitude;
            ParcelDelivery.PickUpLocation.Longitude = sender.Longitude;
            ParcelDelivery.TargetLocation.Latitude = target.Latitude;
            ParcelDelivery.TargetLocation.Longitude = target.Longitude;
            ParcelDelivery.DeliveryDistance = DistanceCustomerCustomer(sender.ID, target.ID);
            ParcelDelivery.Status = FindParcelStatus(parcel);
            return ParcelDelivery;
        }

        private bool FindParcelStatus(Parcel parcel)
        {
            if (parcel.Scheduled == DateTime.MinValue || parcel.Delivered != DateTime.MinValue)
                return false;
            return true;
        }

        public CustomerBL DisplayCustomer(int CustomerID)
        {
            Customer customer = Data.GetCustomer(CustomerID);
            Location location = new(customer.Latitude, customer.Longitude);
            CustomerBL CustomerToDisplay = new CustomerBL(customer.ID,customer.Name,customer.Phone,location);
            foreach (var parcel in Data.GetAllParcels())
            {
                if (parcel.SenderID == CustomerToDisplay.ID)
                    CustomerToDisplay.ParcelesSentByCustomer.Add(CreateParcelAtCustomer(parcel,parcel.TargetID));
                if (parcel.TargetID == CustomerToDisplay.ID)
                    CustomerToDisplay.ParcelesSentToCustomer.Add(CreateParcelAtCustomer(parcel,parcel.SenderID));
            }
            return CustomerToDisplay;
            
        }

        private ParcelAtCustomer CreateParcelAtCustomer(Parcel parcel, int CustomerID)
        {
            ParcelStatus status = GetParcelStatus(parcel);
            Customer cust = Data.GetCustomer(CustomerID);
            CustomerInParcel customer = new(cust.ID, cust.Name);
            ParcelAtCustomer NewParcel = new(parcel.ID,parcel.Weight,parcel.Priority,status,customer);
            return NewParcel;

        }
/// <summary>
/// Reciving the parcel's status according to it's updated times.
/// Requested -> Schedualed -> PickedUp -> Delivered
/// </summary>
/// <param name="parcel"></param>
/// <returns>Parcel status</returns>
        private ParcelStatus GetParcelStatus(Parcel parcel) 
        {
            if (parcel.Scheduled == DateTime.MinValue)
                return ParcelStatus.Requested;
            if (parcel.PickedUp == DateTime.MinValue)
                return ParcelStatus.Scheduled;
            if (parcel.Delivered == DateTime.MinValue)
                return ParcelStatus.PickedUp;
            return ParcelStatus.Delivered;
        }

        public ParcelBL DisplayParcel(int ParcelID)
        {
            Parcel parcel = Data.GetParcel(ParcelID);
            ParcelBL ParcelToDisplay = new(parcel.SenderID, parcel.TargetID, parcel.Weight, parcel.Priority);
            ParcelToDisplay.TimeRequested = parcel.TimeRequested;
            ParcelToDisplay.Scheduled = parcel.Scheduled;
            ParcelToDisplay.PickedUp = parcel.PickedUp;
            ParcelToDisplay.Delivered = parcel.Delivered;
            Customer sender = Data.GetCustomer(parcel.SenderID), target = Data.GetCustomer(parcel.TargetID);
            ParcelToDisplay.Sender.Name = sender.Name;
            ParcelToDisplay.Target.Name = target.Name;
            if (parcel.DroneID != 0)
                ParcelToDisplay.DroneInParcel = CreateDroneInParcel(parcel.DroneID);
            return ParcelToDisplay;
        }

        private DroneInParcel CreateDroneInParcel(int droneID)
        {
            DroneToList tempDrone = new DroneToList();
            foreach (var drone in DroneList)
            {
                if (drone.ID == droneID)
                    tempDrone = drone;
            }
            DroneInParcel Drone = new(tempDrone.ID, tempDrone.BatteryStatus, tempDrone.CurrentLocation);
            return Drone;
        }

        public string DisplayDistanceFromStation(double longitude1, double latitude1, int StationID)
        {
            double longitude2 = 0, latitude2 = 0;
            IEnumerable<Station> stations = Data.GetAllStations();
            foreach (Station station in stations)
                if (station.ID == StationID)
                {
                    longitude2 = station.Longitude;
                    latitude2 = station.Latitude;
                }

            return "The distance is: " + Distance(longitude1, latitude1, longitude2, latitude2) + " km";
        }

        public string DisplayDistanceFromCustomer(double longitude1, double latitude1, int CustomerID)
        {
            double longitude2 = 0, latitude2 = 0;
            IEnumerable<Customer> customers = Data.GetAllCustomers();
            foreach (Customer customer in customers)
                if (customer.ID == CustomerID)
                {
                    longitude2 = customer.Longitude;
                    latitude2 = customer.Latitude;
                }

            return "The distance is: " + Distance(longitude1, latitude1, longitude2, latitude2) + " km";
        }
        #endregion

        #region Display All
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
                        if (parcel.Delivered == DateTime.MinValue)
                            NewCustomer.ParcelsOnTheWay++;
                        else
                            NewCustomer.ParcelsRecived++;
                    }
                    if (parcel.SenderID == customer.ID)
                    {
                        if (parcel.Delivered == DateTime.MinValue)
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
                if (parcel.Delivered != DateTime.MinValue)
                    NewParcel.Status = ParcelStatus.Delivered;
                else if (parcel.PickedUp != DateTime.MinValue)
                    NewParcel.Status = ParcelStatus.PickedUp;
                else if (parcel.Scheduled != DateTime.MinValue)
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
                if (parcel.DroneID != 0)
                {
                    NewParcel.ID = parcel.ID;
                    NewParcel.Priority = parcel.Priority;
                    NewParcel.TargetName = Data.GetCustomer(parcel.TargetID).Name;
                    NewParcel.SenderName = Data.GetCustomer(parcel.SenderID).Name;
                    NewParcel.Weight = parcel.Weight;
                    if (parcel.Delivered != DateTime.MinValue)
                        NewParcel.Status = ParcelStatus.Delivered;
                    else if (parcel.PickedUp != DateTime.MinValue)
                        NewParcel.Status = ParcelStatus.PickedUp;
                    else if (parcel.Scheduled != DateTime.MinValue)
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
        #endregion

        #region Get
        private List<Customer> GetPastCustomers()
        {
            List<Customer> PastCustomersList = new();
            foreach (Parcel parcel in Data.GetAllParcels())
                if (parcel.Delivered != DateTime.MinValue)
                    PastCustomersList.Add(Data.GetCustomer(parcel.TargetID));
            return PastCustomersList;
        }

        private List<Station> GetAllAvailableStations()
        {
            List<Station> AvailableStationsList = new();
            foreach (Station station in Data.GetAllStations())
                if (station.ChargeSlots > 0)
                    AvailableStationsList.Add(station);
            return AvailableStationsList;
        }

        /// <summary>
        /// The function returns the location of the nearest available station.
        /// <para>
        /// If there is no available station, the function will <b>throw</b> an StationExistException.
        /// </para>
        /// </summary>
        /// <returns></returns>
        private Station GetNearestStation(double latitude, double longitude, IEnumerable<Station> AllStation)
        {
            if (AllStation.Count() == 0)
                throw new NoAvailableStation("There are no station available.");
            Station GetNearestStation = new(AllStation.First().ID,
                AllStation.First().Name,
                AllStation.First().ChargeSlots,
                AllStation.First().Latitude,
                AllStation.First().Longitude);

            double distance, MinDistance;
            MinDistance = Distance(latitude, longitude, GetNearestStation.Latitude, GetNearestStation.Longitude);
            foreach (Station station in AllStation)
            {
                distance = Distance(latitude, longitude, station.Latitude, station.Longitude);
                if (distance < MinDistance)
                {
                    MinDistance = distance;
                    GetNearestStation = new(station.ID, station.Name, station.ChargeSlots, station.Latitude, station.Longitude);
                }
            }
            return GetNearestStation;
        }
        #endregion

        #region Calculations
        private double RandBatteryToStation(DroneToList drone, Location location, double battery)
        {
            battery = Distance(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, location.Latitude, location.Longitude) * battery;
            return GetRandBatteryStatus(battery, 100);
        }

        private double GetRandBatteryStatus(double min, double max)
        {
            Random rand = new();
            double MinBattery = 0, MaxBattery = 100, swap, battery; 
            if (min > max) { swap = min; min = max; max = swap; }
            if (min >= MaxBattery)
                return MaxBattery;
            if (max <= MinBattery)
                return MinBattery;
            double remider = (int)min + (int)max + rand.Next(50);
            remider /= 100;
            battery = rand.Next((int)min, (int)max) + remider;
            if (battery > MaxBattery)
                return MaxBattery;
            return battery;
        }

        private double GetWeightMultiplier(WeightCategories weight,Double [] BatteryUse)
        {
            switch (weight)
            {
                case WeightCategories.Light:
                    return BatteryUse[1]; //Light
                case WeightCategories.Medium:
                    return BatteryUse[2]; //Medium
                case WeightCategories.Heavy:
                    return BatteryUse[3]; //Heavy
                default:
                    break;
            }
            return BatteryUse[0]; //Empty
        }

        private double Distance(double x1, double y1, double x2, double y2)
        {
            x1 = (x1 * Math.PI) / 180;
            y1 = (y1 * Math.PI) / 180;
            x2 = (x2 * Math.PI) / 180;
            y2 = (y2 * Math.PI) / 180;
            double result1 = Math.Pow(Math.Sin((x2 - x1) / 2), 2) + Math.Cos(y1) * Math.Cos(y2) * Math.Pow(Math.Sin((y2 - y1) / 2), 2);
            double result2 = 2 * Math.Asin(Math.Sqrt(result1));
            double radius = 3956;
            return (result2 * radius);
        }

        private double DistanceDroneCustomer(DroneToList drone, int CustomerID)
        {
            Customer customer = Data.GetCustomer(CustomerID);
            return Distance(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, customer.Latitude, customer.Longitude);
        }

        private double DistanceCustomerCustomer(int SenderID, int TargetID)
        {
            Customer sender = Data.GetCustomer(SenderID), target = Data.GetCustomer(TargetID);
            return Distance(sender.Latitude, sender.Longitude, target.Latitude, target.Longitude);
        }

        private double DistanceCustomerStation(int CustomerID, Station station)
        {
            Customer customer = Data.GetCustomer(CustomerID);
            return Distance(customer.Latitude, customer.Longitude,station.Latitude, station.Longitude);
        }

        private bool PossibleDelivery(DroneToList drone, Parcel parcel)
        {
            double total;
            Customer sender = Data.GetCustomer(parcel.SenderID), target = Data.GetCustomer(parcel.TargetID);
            Station NearestStat = GetNearestStation(target.Latitude, target.Longitude, Data.GetAllStations());
            Location SenderLoc, TargetLoc, StationLoc;

            SenderLoc = new(sender.Latitude,sender.Longitude);
            TargetLoc = new(target.Latitude, target.Longitude);
            StationLoc = new(NearestStat.Latitude, NearestStat.Longitude);
            total = BatteryUsageCurrStation(drone, parcel.SenderID, parcel.TargetID, NearestStat, parcel.Weight);

            if (total > drone.BatteryStatus)
                return false;
            return true;
        }

        private double BatteryUsageCurrStation(DroneToList drone, int SenderID, int TargetID, Station Station, WeightCategories Weight)
        {
            double DisDroneSender, DisSenderTarget, DisTargetStation;
            DisDroneSender = DistanceDroneCustomer(drone, SenderID);
            DisSenderTarget = DistanceCustomerCustomer(SenderID, TargetID);
            DisTargetStation = DistanceCustomerStation(TargetID, Station);
            return BatteryUsed[0] * (DisTargetStation + DisDroneSender) + GetWeightMultiplier(Weight, BatteryUsed) * DisSenderTarget;
        }
        #endregion
    }
}
