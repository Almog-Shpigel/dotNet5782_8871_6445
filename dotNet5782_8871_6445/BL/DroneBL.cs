using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IBL.BO.EnumsBL;

namespace IBL
{
    namespace BO
    {
        class DroneBL
        {
            public int ID { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }

            public DroneBL(int id, string model, WeightCategories maxWeight)
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
