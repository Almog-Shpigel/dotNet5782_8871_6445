using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace BO
    {
       public class CustomerInParcel
        {
            public int ID { get; set; }
            public string Name { get; set; }

            public CustomerInParcel(int id = 0, string name = "")
            {
                ID = id;
                Name = name;
            }
            public override string ToString()
            {
                return $"{Name} #{ID}";
            }
        }
    }

