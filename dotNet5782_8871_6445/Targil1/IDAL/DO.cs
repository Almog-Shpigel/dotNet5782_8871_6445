using System.ComponentModel;

namespace IDAL
{
    namespace DO
    {
        public enum WeightCategories
        {
            [Description("Light weight")]
            Light,
            [Description("Medium weight")]
            Medium,
            [Description("Heavy weight")]
            Heavy
        }

        public enum Priorities { Regular, Express, Urgent }

        public enum DroneStatuses
        {
            [Description("Available")]
            Available,
            [Description("Delivery")]
            Delivery,
            [Description("Charging")]
            Charging
        }
    }
}