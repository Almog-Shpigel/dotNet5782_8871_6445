
using DO;
using System;


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
        public DateTime? TimeRequested { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }

        public ParcelBL(CustomerInParcel sender, CustomerInParcel target, WeightCategories weight, Priorities priority)
        {
            Sender = new(sender.ID, sender.Name);
            Target = new(target.ID, target.Name);
            DroneInParcel = null;
            Weight = weight;
            Priority = priority;
            TimeRequested = DateTime.Now;
            Scheduled = null;
            PickedUp = null;
            Delivered = null;
        }
        public override string ToString()
        {
            return  $"Parcel #{ID}:\n" +
                    $"Sender:\t\t\t{Sender}\n" +
                    $"Target:\t\t\t{Target}\n" +
                    $"Assigned Drone:\n" +
                    $"{DroneInParcel}\n" +
                    $"Parcel weight:\t\t{Weight}\n" +
                    $"Parcel priority:\t{Priority}\n" +
                    $"Time Requested: \t{TimeRequested}\n" +
                    $"Time Scheduled: \t{Scheduled}\n" +
                    $"Time Picked up: \t{PickedUp}\n" +
                    $"Time Delivered: \t{Delivered}\n";
        }
    }
}

