using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.EnumsBL;

namespace IBL
{
    namespace BO
    {
        class DroneBL
        {
            public int ID { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public double BatteryStatus { get; set; }
            public DroneStatus Status { get; set; }
            public ParcelInDelivery Parcel { get; set; }
            public Location CurrentLocation { get; set; }


            public DroneBL(int id, string model, WeightCategories maxWeight, double batteryStatus, DroneStatus status, ParcelInDelivery parcel, Location currentLocation)
            {
                ID = id;
                Model = model;
                MaxWeight = maxWeight;
                BatteryStatus = batteryStatus;
                Status = status;
                Parcel = parcel;
                CurrentLocation = currentLocation;
            }

            public override string ToString()
            {
                return ($"{Model} #{ID}: Max Weight: {MaxWeight}, battery status: {BatteryStatus}%, Drone status: {Status}, parcel in delivery: {Parcel}, loctaion: {CurrentLocation}");
            }

        }
    }
}
