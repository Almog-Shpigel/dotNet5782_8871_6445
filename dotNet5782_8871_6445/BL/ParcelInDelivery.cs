﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL.DO;

namespace BL
{
    namespace BO
    {
        class ParcelInDelivery
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
            public ParcelInDelivery(int iD, WeightCategories weight, Priorities priority, bool status, CustomerInParcel sender, CustomerInParcel target, Location pickUpLocation, Location targetLocation, double deliveryDistance)
            {
                ID = iD;
                Weight = weight;
                Priority = priority;
                Status = status;
                Sender = sender;
                Target = target;
                PickUpLocation = pickUpLocation;
                TargetLocation = targetLocation;
                DeliveryDistance = deliveryDistance;
            }

        }
    }
}
