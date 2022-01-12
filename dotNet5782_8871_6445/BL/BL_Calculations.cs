using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using DO;

namespace BlApi
{
    partial class BL
    {
        /// <summary>
        /// reciving a random battery value from the min and max value recived as paremeters
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns>batter value %</returns>
        private double GetRandBatteryStatus(double min, double max)
        {
            Random rand = new();
            double minBattery = 0, maxBattery = 100, swap, battery;
            if (min > max) { swap = min; min = max; max = swap; }
            if (min >= maxBattery)
                return maxBattery;
            if (max <= minBattery)
                return minBattery;
            double remider = (int)min + (int)max + rand.Next(50);
            remider /= 100;
            battery = rand.Next((int)min, (int)max) + remider;
            if (battery > maxBattery)
                return maxBattery;
            return battery;
        }

        /// <summary>
        /// Reciving a paremeter multiplier of battey useage according to the weight given, the larger the weight the larger the battery usage
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="BatteryUse"></param>
        /// <returns>Weight multiplier paremeter</returns>
        private double GetWeightMultiplier(WeightCategories weight)
        {
            switch (weight)
            {
                case WeightCategories.Light:
                    return BatteryUsageLightWight; //Light
                case WeightCategories.Medium:
                    return BatteryUsageMediumWight; //Medium
                case WeightCategories.Heavy:
                    return BatteryUsageHaevyWight; //Heavy
                default:
                    break;
            }
            return BatteryUsageEmpty; //Empty
        }

        /// <summary>
        /// Returns the distance (in km) between 2 points
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lon1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2"></param>
        /// <returns>distance (km)</returns>
        internal double Distance(Location location1, Location location2)
        {
            double rlat1 = Math.PI * location1.Latitude / 180;
            double rlat2 = Math.PI * location2.Latitude / 180;
            double theta = location1.Longitude - location2.Longitude;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) +
                Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
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
        internal double DistanceDroneCustomer(DroneToList drone, Customer customer)
        {
            Location customerLocation = new(customer.Latitude, customer.Longitude);
            return Distance(drone.CurrentLocation, customerLocation);
        }

        /// <summary>
        /// Return the distance between two specific customers
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="CustomerID"></param>
        /// <returns>distance (km)</returns>
        private double DistanceCustomerCustomer(Customer sender, Customer target)
        {
            Location senderLocation = new(sender.Latitude, sender.Longitude), targetLocation = new(target.Latitude, target.Longitude);
            return Distance(senderLocation, targetLocation);
        }

        /// <summary>
        /// Return the distance between a specific customer and a specific station
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="CustomerID"></param>
        /// <returns>distance (km)</returns>
        private double DistanceCustomerStation(Customer customer, Station station)
        {
            Location customerLocation = new(customer.Latitude, customer.Longitude), stationLocation = new(station.Latitude, station.Longitude);
            return Distance(customerLocation, stationLocation);
        }

        /// <summary>
        /// Function to check if a drone is capable of doing a delivery, return true if he can or false if can't
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="parcel"></param>
        /// <returns></returns>
        public bool PossibleDelivery(DroneToList drone, Parcel parcel)
        {
            lock (Data)
            {
                if (parcel.ID == 0)
                    return false;
                double total;
                Customer sender, target;
                try
                {
                    sender = Data.GetCustomer(parcel.SenderID);
                    target = Data.GetCustomer(parcel.TargetID);
                }
                catch (CustomerExistException ex)
                {
                    throw new EntityExistException("Invalid id. ",ex);
                }
                Location targetLocation = new(target.Latitude, target.Longitude);
                Station nearestStat = GetNearestStation(targetLocation, Data.GetStations(station => true));
                total = BatteryUsageCurrStation(drone, sender, target, nearestStat, parcel.Weight);

                if (total > drone.BatteryStatus)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Calculates the battery needed to make a delivery and get back to a station
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="SenderID"></param>
        /// <param name="TargetID"></param>
        /// <param name="Station"></param>
        /// <param name="Weight"></param>
        /// <returns></returns>
        private double BatteryUsageCurrStation(DroneToList drone, Customer sender, Customer target, Station Station, WeightCategories Weight)
        {
            double disDroneSender, disSenderTarget, disTargetStation;
            disDroneSender = DistanceDroneCustomer(drone, sender);
            disSenderTarget = DistanceCustomerCustomer(sender, target);
            disTargetStation = DistanceCustomerStation(target, Station);
            return BatteryUsageEmpty * (disDroneSender + disTargetStation) + GetWeightMultiplier(Weight) * disSenderTarget;
        }

        private double CalcBatteryCharged(DroneToList drone)
        {
            lock (Data)
            {
                double timeDifference = (DateTime.Now - (DateTime)Data.GetDroneCharge(drone.ID).Start).TotalSeconds;
                drone.BatteryStatus += BatteryChargeRate * timeDifference;
                if (drone.BatteryStatus > 100)
                    return 100;
                return drone.BatteryStatus;
            }
        }
    }
}
