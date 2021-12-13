using DO;
using static BO.EnumsBL;


    namespace BO
    {
        public class DroneToList
        {
            public int ID { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public double BatteryStatus { get; set; }
            public DroneStatus Status { get; set; }
            public Location CurrentLocation { get; set; }
            public int ParcelID { get; set; }

            public DroneToList()
            {
                ID = 0;
                Model = "";
                MaxWeight = WeightCategories.Light;
                BatteryStatus = 0;
                Status = DroneStatus.Charging;
                CurrentLocation = new(0,0);
                ParcelID = 0;
            }
        public DroneToList(int id = 0, string model = "", WeightCategories weight = WeightCategories.Light,
            double battery = 0, DroneStatus status = DroneStatus.Charging, Location location = null, int ParcelId = 0)
            {
                ID = id;
                Model = model;
                MaxWeight = weight;
                BatteryStatus = battery;
                Status = status;
                CurrentLocation = location;
                ParcelID = ParcelId;
            }
            public override string ToString()
            {
                return $"Drone #{ID}:\n" +
                    $"Model {Model}, {Status}, {MaxWeight}.\tBattery: {BatteryStatus}%\n" +
                    $"Carring parcel #{ParcelID}\tLocation: {CurrentLocation}.\n";
            }
        }
    }
