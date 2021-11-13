using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    namespace BO
    {
        class StationToList
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int AvailableChargeSlots { get; set; }
            public int UsedChargeSlots { get; set; }

            public StationToList(int id, string name, int AvailableSlots, int UsedSlots)
            {
                ID = id;
                Name = name;
                AvailableChargeSlots = AvailableSlots;
                UsedChargeSlots = UsedSlots;
            }
        }
    }
}
