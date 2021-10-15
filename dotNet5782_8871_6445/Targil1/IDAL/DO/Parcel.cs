using IDAL.DO;
using System;

namespace ConsoleUI.IDAL.DO
{
    internal struct Parcel
    {
        public int ID;
        public int SenderID;
        public int TargetID;
        public int DroneID;
        public WeightCategories Weight;
        public Priorities Priority;
        public DateTime TimeRequested;
        public DateTime Schedued;
        public DateTime PickedUp;
        public DateTime Delivered;

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