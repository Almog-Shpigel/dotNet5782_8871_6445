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

        public Parcel(int id = 0, int sender = 0, int target = 0, int drone = 0,
                        WeightCategories weight = WeightCategories.Light, Priorities priority = Priorities.Regular,
                        DateTime? requested = null, DateTime? schedued = null,
                        DateTime? pickedUp = null, DateTime? delivered = null)
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