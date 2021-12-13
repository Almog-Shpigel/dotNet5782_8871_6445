using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace BO
    {
        public class StationBL
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int ChargeSlots { get; set; }
            public Location Location { get; set; }
            public List<DroneChargeBL> ChargingDrones;

        public StationBL(int id = 0, string name = "", int slots = 0, Location location = null)
            {
                ID = id;
                Name = name;
                ChargeSlots = slots;
                Location = location;
                ChargingDrones = new();
            }
            public override string ToString()
            {
                string combindedString = string.Join("",ChargingDrones);
                return $"{Name} #{ID}:\n" +
                    $"Charge slots available: {ChargeSlots}\n" +
                    $"Location: {Location}\n" +
                    $"{combindedString}\n";
            }
        }
    }

