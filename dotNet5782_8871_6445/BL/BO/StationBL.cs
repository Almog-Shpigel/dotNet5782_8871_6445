using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class StationBL
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int ChargeSlots { get; set; }
            public Location Location { get; set; }
            public List<DroneCharge> ChargingDrones;

        public StationBL(int id, string name, int slots,
                           double longitude, double latitude)
            {
                ID = id;
                Name = name;
                ChargeSlots = slots;
                Location = new (longitude, latitude);
                ChargingDrones = new();
            }
            public override string ToString()
            {
                
                return $"{Name} #{ID}:\n" +
                    $"Charge slots available: {ChargeSlots}\n" +
                    $"Location: {Location}";
            }
        }
    }
}
