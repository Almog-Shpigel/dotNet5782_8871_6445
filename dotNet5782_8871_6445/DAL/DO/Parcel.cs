using System;


namespace DO
{
    public struct Parcel
    {
        public int ID { get; set; }
        public int SenderID { get; set; }
        public int TargetID { get; set; }
        public int DroneID { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DateTime? TimeRequested { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }

        public Parcel(int id, int sender, int target, int drone,
                        WeightCategories weight, Priorities priority,
                        DateTime? requested, DateTime? schedued,
                        DateTime? pickedUp, DateTime? delivered)
        {
            ID = id;
            SenderID = sender;
            TargetID = target;
            DroneID = drone;
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
                $"Sender ID:        {SenderID}\n" +
                $"Target ID:        {TargetID}\n" +
                $"Drone ID:         {DroneID}\n" +
                $"Parcel weight:    {Weight}\n" +
                $"Parcel priority:  {Priority}\n" +
                $"Time Requested:   {TimeRequested}\n" +
                $"Scheduled:        {Scheduled}\n" +
                $"Picked up:        {PickedUp}\n" +
                $"Delivered:        {Delivered}\n";
        }
    }
}