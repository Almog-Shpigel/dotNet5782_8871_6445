using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using DO;

namespace IBL
{
    namespace BO
    {
       public class ParcelInDelivery
        {
            public int ID { get; set; }
            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public bool Status { get; set; }
            public CustomerInParcel Sender { get; set; }
            public CustomerInParcel Target { get; set; }
            public Location PickUpLocation { get; set; }
            public Location TargetLocation { get; set; }
            public double DeliveryDistance { get; set; }
            public ParcelInDelivery()
            {
                ID = 0;
                Weight = WeightCategories.Light;
                Priority = Priorities.Regular;
                Status = false;
                Sender = new();
                Target = new();
                PickUpLocation = new();
                TargetLocation = new();
                DeliveryDistance = 0;
            }
            public ParcelInDelivery(int id, WeightCategories weight, Priorities priority, bool status, CustomerInParcel sender, CustomerInParcel target, Location pickUpLocation, Location targetLocation, double deliveryDistance)
            {
                ID = id;
                Weight = weight;
                Priority = priority;
                Status = status;
                Sender = sender;
                Target = target;
                PickUpLocation = pickUpLocation;
                TargetLocation = targetLocation;
                DeliveryDistance = deliveryDistance;
            }
            public override string ToString()
            {
                return  $"\nParcel #{ID}:\n" +
                        $"Properties:\t{Weight}, {Priority}.\n" + 
                        $"Sender {Sender}\n" +
                        $"Receiver {Target}\n" +
                        $"Sent from:\t{PickUpLocation}\n" +
                        $"Sent to:\t{TargetLocation}.\n" +
                        $"Overall distance: {DeliveryDistance} km";
            }
        }
    }
}
