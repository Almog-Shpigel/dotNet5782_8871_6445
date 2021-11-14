using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class CustomerInParcel
        {
            public int ID { get; set; }
            public string Name { get; set; }

            public CustomerInParcel(int id = 0, string name = "")
            {
                ID = id;
                Name = name;
            }
        }
    }
}
