using BL;
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
        class ParcelBL
        {
            public int ID { get; set; }
            public CustomerInParcel Sender { get; set; }
            public CustomerInParcel Target { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public DroneInParcel DroneInParcel { get; set; }
            public DateTime TimeRequested { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }

            public ParcelBL(int id, CustomerInParcel sender, CustomerInParcel target,
                          WeightCategories weight, Priorities priority,
                          DateTime requested, DateTime scheduled,
                          DateTime pickedUp, DateTime delivered, DroneInParcel droneInParcel)
            {
                ID = id;
                Sender= sender;
                Target = target;
                DroneInParcel = droneInParcel;
                Weight = weight;
                Priority = priority;
                TimeRequested = requested;
                Scheduled = scheduled;
                PickedUp = pickedUp;
                Delivered = delivered;
                DroneInParcel = droneInParcel;
            }
            public override string ToString()
            {
                return $"Parcel #{ID}:\n" +
                    $"Sender ID:        {Sender}\n" +
                    $"Target ID:        {Target}\n" +
                    $"Drone ID:         {DroneInParcel}\n" +
                    $"Parcel weight:    {Weight}\n" +
                    $"Parcel priority:  {Priority}\n" +
                    $"Time Requested:   {TimeRequested}\n" +
                    $"Scheduled:        {Scheduled}\n" +
                    $"Picked up:        {PickedUp}\n" +
                    $"Delivered:        {Delivered}\n";
            }
        }
    }
}
