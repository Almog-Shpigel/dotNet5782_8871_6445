namespace DO
{
    public struct Drone
    {
        public int ID { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }

        public Drone(int id = 0, string model = "", WeightCategories maxWeight = WeightCategories.Light)
        {
            ID = id;
            Model = model;
            MaxWeight = maxWeight;
        }

        public override string ToString()
        {
            return $"{Model} #{ID}:\n" +
                $"Max Weight: {MaxWeight}\n";
        }
    }
}

