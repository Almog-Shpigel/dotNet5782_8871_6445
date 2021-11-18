using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class StationBL
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int ChargeSlots { get; set; }
            public Location Location { get; set; }
            public List<DroneCharge> ChargingDrones;

        public StationBL(int id, string name, int slots, Location location)
            {
                if (id < 100000 || id > 999999)
                    throw new InvalidIDException("Invalid station ID number");
                if (slots < 0)
                    throw new InvalidSlotsException("Charge slots can't be a negative number");
                if ((int)location.Latitude != 31 || (int)location.Longitude != 35)
                    throw new OutOfRangeLocationException("The location is outside of Jerusalem"); ///We assume for now that all the locations are inside Jerusalem
                ID = id;
                Name = name;
                ChargeSlots = slots;
                Location = location;
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
