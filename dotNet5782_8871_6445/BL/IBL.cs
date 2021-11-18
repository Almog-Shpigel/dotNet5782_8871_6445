using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public interface IBL
    {
        public List<StationToList> DispalyAllStations();
        public List<DroneToList> DispalyAllDrones();
        public List<CustomerToList> DispalyAllCustomers();
        public List<ParcelToList> DispalyAllParcels();
        public List<ParcelToList> DispalyAllUnassignedParcels();
        public List<StationToList> DispalyAllAvailableStations();
        public string DisplayStation(int StationID);
        public string DisplayDrone(int DroneID);
        public string DisplayCustomer(int CustomerID);
        public string DisplayParcel(int ParcelID);
        public string DisplayDistanceFromStation(double longitude1, double latitude1, int StationID);
        public string DisplayDistanceFromCustomer(double longitude1, double latitude1, int CustomerID);
        public void UpdateDroneName(int v1, string v2);
        public void UpdateStation(int v1, string v2, int v3);
        public void UpdateCustomer(int v1, string v2, string v3);
        public void UpdateParcelToDrone(int v);
        public void UpdateParcelDeleiveredByDrone(int droneID);
        public void UpdateDroneToBeCharged(int v);
        public void UpdateParcelCollectedByDrone(int v);
        public void UpdateDroneAvailable(int v1, int v2);
        public void AddNewCustomer(CustomerBL customer);
        public void AddNewStation(StationBL station);
        public void AddNewDrone(DroneBL drone);
        public void AddNewParcel(ParcelBL parcel);
        //public List<string> PrintAllAvailableStations(); // I'm not sure what this is for...
    }
}
