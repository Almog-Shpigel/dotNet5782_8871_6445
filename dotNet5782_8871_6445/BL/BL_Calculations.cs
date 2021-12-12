using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using DO;

namespace IBL
{
    partial class BL
    {
        /// <summary>
        /// Returning a random battery value (100-0 %) when the minimum determined by the location of the drone and
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="location"></param>
        /// <param name="battery"></param>
        /// <returns></returns>
        private double RandBatteryToStation(DroneToList drone, Location location, double battery)
        {
            battery = Distance(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, location.Latitude, location.Longitude) * battery;
            return GetRandBatteryStatus(battery, 100);
        }
        /// <summary>
        /// reciving a random battery value from the min and max value recived as paremeters
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>batter value %</returns>
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
        /// <summary>
        /// Reciving a paremeter multiplier of battey useage according to the weight given, the larger the weight the larger the battery usage
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="BatteryUse"></param>
        /// <returns>Weight multiplier paremeter</returns>
        private double GetWeightMultiplier(WeightCategories weight, Double[] BatteryUse)
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
        /// <summary>
        /// Returns the distance (in km) between 2 points
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns>distance (km)</returns>
        private double Distance(double lat1, double lon1, double lat2, double lon2)
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            dist = (int)(dist * 160.9344);
            return dist / 100;
        }
        /// <summary>
        /// Return the distance between a specific drone and a specific customer
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="CustomerID"></param>
        /// <returns>distance (km)</returns>
        private double DistanceDroneCustomer(DroneToList drone, int CustomerID)
        {
            Customer customer = Data.GetCustomer(CustomerID);
            return Distance(drone.CurrentLocation.Latitude, drone.CurrentLocation.Longitude, customer.Latitude, customer.Longitude);
        }
        /// <summary>
        /// Return the distance between two specific customers
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="CustomerID"></param>
        /// <returns>distance (km)</returns>
        private double DistanceCustomerCustomer(int SenderID, int TargetID)
        {
            Customer sender = Data.GetCustomer(SenderID), target = Data.GetCustomer(TargetID);
            return Distance(sender.Latitude, sender.Longitude, target.Latitude, target.Longitude);
        }
        /// <summary>
        /// Return the distance between a specific customer and a specific station
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="CustomerID"></param>
        /// <returns>distance (km)</returns>
        private double DistanceCustomerStation(int CustomerID, Station station)
        {
            Customer customer = Data.GetCustomer(CustomerID);
            return Distance(customer.Latitude, customer.Longitude, station.Latitude, station.Longitude);
        }
        /// <summary>
        /// Function to check if a drone is capable of doing a delivery, return true if he can or false if can't
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="parcel"></param>
        /// <returns></returns>
        private bool PossibleDelivery(DroneToList drone, Parcel parcel)
        {
            double total;
            Customer sender = Data.GetCustomer(parcel.SenderID), target = Data.GetCustomer(parcel.TargetID);
            Station NearestStat = GetNearestStation(target.Latitude, target.Longitude, Data.GetStations(station => true));
            Location SenderLoc, TargetLoc, StationLoc;

            SenderLoc = new(sender.Latitude, sender.Longitude);
            TargetLoc = new(target.Latitude, target.Longitude);
            StationLoc = new(NearestStat.Latitude, NearestStat.Longitude);
            total = BatteryUsageCurrStation(drone, parcel.SenderID, parcel.TargetID, NearestStat, parcel.Weight);

            if (total > drone.BatteryStatus)
                return false;
            return true;
        }
        /// <summary>
        /// Checking if a drone can complete the delivery according to the amount of battery he has and the distance he needs to do for the delivery
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="SenderID"></param>
        /// <param name="TargetID"></param>
        /// <param name="Station"></param>
        /// <param name="Weight"></param>
        /// <returns></returns>
        private double BatteryUsageCurrStation(DroneToList drone, int SenderID, int TargetID, Station Station, WeightCategories Weight)
        {
            double DisDroneSender, DisSenderTarget, DisTargetStation;
            DisDroneSender = DistanceDroneCustomer(drone, SenderID);
            DisSenderTarget = DistanceCustomerCustomer(SenderID, TargetID);
            DisTargetStation = DistanceCustomerStation(TargetID, Station);
            return BatteryUsed[0] * (DisTargetStation + DisDroneSender) + GetWeightMultiplier(Weight, BatteryUsed) * DisSenderTarget;
        }
    }
}
