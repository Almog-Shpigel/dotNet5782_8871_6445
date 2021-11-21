
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
       public class ParcelBL
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

            public ParcelBL( int sender, int target,
                          WeightCategories weight, Priorities priority)
            {
                Sender = new(sender);
                Target = new(target);
                DroneInParcel = null;
                Weight = weight;
                Priority = priority;
                TimeRequested = DateTime.Now;
                Scheduled = DateTime.MinValue;
                PickedUp = DateTime.MinValue;
                Delivered = DateTime.MinValue;
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
