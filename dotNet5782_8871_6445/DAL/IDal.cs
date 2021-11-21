using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{ 
    public interface IDal
    {
        public void AddNewStation(Station station);
        public void AddNewCustomer(Customer customer);
        public void AddNewParcel(Parcel parcel);
        public void AddNewDrone(Drone drone);
        public void DroneAvailable(int DroneID);
        public void DroneToBeCharge(int DroneID, int StationID, DateTime start);
        public void ParcelDeleivery(int idNum);
        public void ParcelCollected(int id);
        public void PairParcelToDrone(int ParcelID, int DroneID);
        public double DistanceFromStation(double x1, double y1, int StationID);
        public double DistanceFromCustomer(double x1, double y1, int CustomerID);
        public string DisplayDrone(int DroneID);
        public string DisplayCustomer(int CustomerID);
        public string DisplayParcel(int ParcelID);
        public string DisplayStation(int StationID);
        public IEnumerable<string> PrintAllStations();
        public IEnumerable<string> PrintAllDrones();
        public IEnumerable<string> PrintAllCustomers();
        public IEnumerable<string> PrintAllParcels();
        public IEnumerable<string> PrintAllUnassignedParcels();
        public IEnumerable<string> PrintAllAvailableStations();
        public IEnumerable<Parcel> GetAllParcels();
        public IEnumerable<Station> GetAllStations();
        public IEnumerable<Customer> GetAllCustomers();
        public IEnumerable<Drone> GetAllDrones();
        public IEnumerable<DroneCharge> GetAllDronesCharge();
        //public List<Station> GetAllAvailableStations();
        public Drone GetDrone(int id);
        public Station GetStation(int id);
        public Customer GetCustomer(int id);
        public Parcel GetParcel(int id);
        public double[] GetBatteryUsed();
    }
}
