using IDAL.DO;
using System;

namespace ConsoleUI.IDAL.DO
{
    public struct Parcel
    {
        public int ID { get; set; }
        public int SenderID { get; set; }
        public int TargetID { get; set; }
        public int DroneID { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DateTime TimeRequested { get; set; }
        public DateTime Schedued { get; set; }
        public DateTime PickedUp { get; set; }
        public DateTime Delivered { get; set; }

        public Parcel(int id, int senderID, int targetID, int droneID,
                      WeightCategories weight, Priorities priority,
                      DateTime timeRequested, DateTime schedued,
                      DateTime pickedUp, DateTime delivered)
        {
            ID = id;
            SenderID = senderID;
            TargetID = targetID;
            DroneID = droneID;
            Weight = weight;
            Priority = priority;
            TimeRequested = timeRequested;
            Schedued = schedued;
            PickedUp = pickedUp;
            Delivered = delivered;
        }
    }
}