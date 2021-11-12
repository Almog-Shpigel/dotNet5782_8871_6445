using IDAL.DO;
using static IBL.BO.EnumsBL;

namespace IBL
{
    namespace BO
    {
        class DroneForList
        {
            public int ID { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public double Battery { get; set; }
            public DroneStatus Status { get; set; }
            public Location CurrentLocation { get; set; }
            public int ParcelID { get; set; }

            public DroneForList()
            {
                ID = 0;
                Model = "";
                MaxWeight = WeightCategories.Light;
                Battery = 0;
                Status = DroneStatus.Charging;
                CurrentLocation = new(0,0);
                ParcelID = 0;
            }

        }
    }
}