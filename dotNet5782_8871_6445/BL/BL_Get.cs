using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using BO;

namespace BlApi
{
    partial class BL
    {
        /// <summary>
        /// Returns a list of every customer that a parcel was sent to him at some point 
        /// </summary>
        /// <returns>list of customer</returns>
        private IEnumerable<Customer> GetPastCustomers()
        {
            IEnumerable<Parcel> DeliverdParcels = Data.GetParcels(parcel => parcel.Delivered != null);
            IEnumerable<Customer> PastCustomersList = DeliverdParcels.Select(parcel=>Data.GetCustomer(parcel.TargetID));
            //foreach (Parcel parcel in Data.GetParcels(parcel => true))
            //    if (parcel.Delivered != null)
            //        PastCustomersList.Add(Data.GetCustomer(parcel.TargetID));
            return PastCustomersList;
        }

        /// <summary>
        /// Returns a list of all the stations that has available charge slots
        /// </summary>
        /// <returns>list of available stations</returns>
        private List<Station> GetAllAvailableStationsDO()
        {
            return Data.GetStations(station => station.ChargeSlots > 0).ToList();
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

        public IEnumerable<Station> GatAllStationsDO()
        {
            return Data.GetStations(station => true);
        }

        //public IEnumerable<Location> GetAllDroneLocations()
        //{
        //    return DroneList.Select(drone => drone.CurrentLocation);
        //}

        //public List<Location> GetAllStationsLocations()
        //{
        //    Location NewLocation = new();
        //    List<Location> AllLocations = new();
        //    foreach (var item in Data.GetStations(station => true))
        //    {
        //        NewLocation = new(item.Latitude, item.Longitude);
        //        AllLocations.Add(NewLocation);
        //    }
        //    return AllLocations;
        //}
    }
}
