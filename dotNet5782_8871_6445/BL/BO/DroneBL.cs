using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BO.EnumsBL;

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
                ID = id;
                Model = model;
                MaxWeight = maxWeight;
                BatteryStatus = batteryStatus;
                Status = status;
                Parcel = new();
                CurrentLocation = null;
            }

            public override string ToString()
            {
                return ($"\n{Model} #{ID}:\n" +
                    $"Max Weight:\t{MaxWeight}\n" +
                    $"Loctaion:\t{CurrentLocation}\n" +
                    $"Battery status:\t{BatteryStatus}%\n" +
                    $"Drone status:\t{Status}\n" +
                    $"Parcel in delivery:\n" +
                    $"{Parcel}");
            }

        }
    }
