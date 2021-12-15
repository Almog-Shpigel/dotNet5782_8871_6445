namespace BO
{
    public class StationToList
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int AvailableChargeSlots { get; set; }
        public int UsedChargeSlots { get; set; }


        public StationToList(int id= 0, string name = "", int AvailableSlots = 0, int UsedSlots = 0)
        {
            ID = id;
            Name = name;
            AvailableChargeSlots = AvailableSlots;
            UsedChargeSlots = UsedSlots;
        }
        public override string ToString()
        {
            return $"{Name} #{ID}  \n" +
                $"No. of charge slots: \n" +
                $"Available: {AvailableChargeSlots} Used: {UsedChargeSlots}\n";
        }
    }
}

