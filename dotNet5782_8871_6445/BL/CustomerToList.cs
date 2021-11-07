using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class CustomerToList
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public int SentAndDeliverd;
            public int SentAndNOTDeliverd;
            public int Recived;
            public int OnTheWay;

        }
    }
}
