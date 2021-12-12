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
        public void AddNewStation(Station station);
        public void AddNewCustomer(Customer customer);
        public void AddNewParcel(Parcel parcel);
        public void AddNewDrone(Drone drone , int StationID);
        public void DroneAvailable(int DroneID);
        public void DroneToBeCharge(int DroneID, int StationID, DateTime? start);
        public void ParcelDelivery(int idNum);
        public void ParcelCollected(int id);
        public void PairParcelToDrone(int ParcelID, int DroneID);
        public double DistanceFromStation(double x1, double y1, int StationID);
        public double DistanceFromCustomer(double x1, double y1, int CustomerID);
        public IEnumerable<Parcel> GetParcels(Predicate<Parcel> ParcelPredicate);
        public IEnumerable<Station> GetStations(Predicate<Station> StationPredicate);
        public IEnumerable<Customer> GetCustomers(Predicate<Customer> CustomerPredicate);
        public IEnumerable<Drone> GetDrones(Predicate<Drone> DronePredicate);
        public IEnumerable<DroneCharge> GetDroneCharge(Predicate<DroneCharge> DroneChargePredicate);
        public Drone GetDrone(int id);
        public Station GetStation(int id);
        public Customer GetCustomer(int id);
        public Parcel GetParcel(int id);
        public double[] GetBatteryUsed();
        public void UpdateDroneName(int id, string model);
        public void UpdateStationName(int stationID, string name);
        public void UpdateStationSlots(int stationID, int slots);
        public void UpdateCustomerName(int id, string name);
        public void UpdateCustomerPhone(int id, int phone);
        DroneCharge GetDroneCharge(int droneID);
        //IEnumerable<Parcel> GetAllAvailableParcels();
    }
}
