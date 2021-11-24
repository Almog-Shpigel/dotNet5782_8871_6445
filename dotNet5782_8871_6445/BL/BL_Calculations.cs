using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL.DO;

namespace IBL
{
    partial class BL
    {
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
            return Distance(customer.Latitude, customer.Longitude, station.Latitude, station.Longitude);
        }

        private bool PossibleDelivery(DroneToList drone, Parcel parcel)
        {
            double total;
            Customer sender = Data.GetCustomer(parcel.SenderID), target = Data.GetCustomer(parcel.TargetID);
            Station NearestStat = GetNearestStation(target.Latitude, target.Longitude, Data.GetAllStations());
            Location SenderLoc, TargetLoc, StationLoc;

            SenderLoc = new(sender.Latitude, sender.Longitude);
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
    }
}
