

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

            public Drone(int id, string model, WeightCategories maxWeight,
                         DroneStatuses status, double battery)
            {
                ID = id;
                Model = model;
                MaxWeight = maxWeight;
                Status = status;
                Battery = battery;
            }

            public override string ToString()
            {
                return ($"{Model} #{ID}:\n" +
                    $"Max Weight: {MaxWeight}\n" +
                    $"Drone Statuses: {Status}\n" +
                    $"Battery {Battery}\n");
            }
        }
    }
}
