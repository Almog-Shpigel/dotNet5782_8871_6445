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
            //public DroneInParcel DroneInParcel { get; set; }
            public DateTime TimeRequested { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }

            public ParcelBL(int id, int sender, int target, int drone,
                          WeightCategories weight, Priorities priority,
                          DateTime requested, DateTime schedued,
                          DateTime pickedUp, DateTime delivered)
            {
                ID = id;
                //SenderID = sender;
                //TargetID = target;
                //DroneID = drone;
                Weight = weight;
                Priority = priority;
                TimeRequested = requested;
                Scheduled = schedued;
                PickedUp = pickedUp;
                Delivered = delivered;
            }
            public override string ToString()
            {
                return $"Parcel #{ID}:\n" +
                    //$"Sender ID:        {SenderID}\n" +
                    //$"Target ID:        {TargetID}\n" +
                    //$"Drone ID:         {DroneID}\n" +
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
