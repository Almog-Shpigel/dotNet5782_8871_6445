

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int ID { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatuses Status { get; set; }
            public double Battery { get; set; }

            public override string ToString()
            {
                return $"Drone #{ID}: model={Model}, {Status}, {MaxWeight}, battery={(int)Battery}";
            }
            public Drone(int id, string model, WeightCategories maxWeight,
                         DroneStatuses status, double battery)
            {
                ID = id;
                Model = model;
                MaxWeight = maxWeight;
                Status = status;
                Battery = battery;
            }
            public void print()
            {
                System.Console.WriteLine($"{Model} #{ID}:\n" +
                    $"Max Weight: {MaxWeight}\n" +
                    $"Drone Statuses: {Status}\n" +
                    $"Battery {Battery}\n");
            }
        }
    }
}
