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
        }
    }
}