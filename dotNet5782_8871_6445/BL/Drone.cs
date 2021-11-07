using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.Enums;

namespace IBL
{
    namespace BO
    {
        class Drone
        {
            public int ID { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }

            public Drone(int id, string model, WeightCategories maxWeight)
            {
                ID = id;
                Model = model;
                MaxWeight = maxWeight;

            }

            public override string ToString()
            {
                return ($"{Model} #{ID}:\n" +
                    $"Max Weight: {MaxWeight}\n");
            }

        }
    }
}
