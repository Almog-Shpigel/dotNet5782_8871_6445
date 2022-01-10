using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{ 
    public interface IDal
    {
        #region Add
        public void AddNewStation(Station station);
        public void AddNewCustomer(Customer customer);
        public void AddNewParcel(Parcel parcel);
        public void AddNewDrone(Drone drone, Station station);
        #endregion

        #region Update
        public void UpdateDroneName(Drone drone);
        public void UpdateStationName(Station station);
        public void UpdateCustomerName(Customer customer);
        public void UpdateCustomerPhone(Customer customer);
        public void UpdateStationSlots(Station station);
        public void UpdateDroneToBeAvailable(Drone drone);
        public void UpdateDroneToBeCharge(Drone drone, Station station, DateTime? start);
        public void UpdateParcelInDelivery(Parcel parcel);
        public void UpdateParcelCollected(Parcel parcel);
        public void PairParcelToDrone(Parcel parcel, Drone drone);

        public void UpdateDeleteParcel(Parcel parcel);
        #endregion

        #region Get
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> ParcelPredicate);
        public IEnumerable<Station> GetStations(Predicate<Station> StationPredicate);
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> CustomerPredicate);
        public IEnumerable<Drone> GetDrones(Predicate<Drone> DronePredicate);
        public IEnumerable<DroneCharge> GetDroneCharge(Predicate<DroneCharge> DroneChargePredicate);
        public Drone GetDrone(int id);
        public Station GetStation(int id);
        public Customer GetCustomer(int id);
        public Parcel GetParcel(int id);
        DroneCharge GetDroneCharge(int id);
        public double GetBatteryProperty(string value);

        #endregion
    }
}
