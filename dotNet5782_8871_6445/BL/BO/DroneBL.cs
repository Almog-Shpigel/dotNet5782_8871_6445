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
       public class DroneBL
        {
            public int ID { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public double BatteryStatus { get; set; }
            public DroneStatus Status { get; set; }
            public ParcelInDelivery Parcel { get; set; }
            public Location CurrentLocation { get; set; }


            public DroneBL(int id = 0, string model = "", WeightCategories maxWeight = WeightCategories.Light, double batteryStatus = 0, DroneStatus status = DroneStatus.Charging)
            {
                if (id < 100000 || id > 999999)
                    throw new InvalidIDException("Invalid customer ID number");
                ID = id;
                Model = model;
                MaxWeight = maxWeight;
                BatteryStatus = batteryStatus;
                Status = status;
            }

            public override string ToString()
            {
                return ($"{Model} #{ID}:    Max Weight: {MaxWeight},    battery status: {BatteryStatus}%,  Drone status: {Status}, parcel in delivery: {Parcel},   loctaion: {CurrentLocation}");
            }

        }
    }
}
