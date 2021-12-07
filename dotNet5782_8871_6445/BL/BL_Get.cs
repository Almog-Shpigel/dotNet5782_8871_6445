using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IBL
{
    partial class BL
    {
        /// <summary>
        /// Returns a list of every customer that a parcel was sent to him at some point 
        /// </summary>
        /// <returns>list of customer</returns>
        private List<Customer> GetPastCustomers()
        {
            List<Parcel> DeliverdParcels = Data.GetParcels(parcel => parcel.Delivered != null).ToList();
            List<Customer> PastCustomersList = DeliverdParcels.Select(parcel=>Data.GetCustomer(parcel.TargetID)).ToList();
            //foreach (Parcel parcel in Data.GetParcels(parcel => true))
            //    if (parcel.Delivered != null)
            //        PastCustomersList.Add(Data.GetCustomer(parcel.TargetID));
            return PastCustomersList;
        }
        /// <summary>
        /// Returns a list of all the stations that has open charge slots
        /// </summary>
        /// <returns>list of available stations</returns>
        private List<Station> GetAllAvailableStations()
        {
            List<Station> AvailableStationsList = Data.GetStations(station => station.ChargeSlots > 0).ToList();
            //foreach (Station station in Data.GetStations(station => true))
            //    if (station.ChargeSlots > 0)
            //        AvailableStationsList.Add(station);
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
    }
}
