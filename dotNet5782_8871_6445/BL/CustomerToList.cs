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
            public int SentAndDeliverd { get; set; }
            public int SentAndNOTDeliverd { get; set; }
            public int ParcelsRecived { get; set; }
            public int ParcelsOnTheWay { get; set; }
            public CustomerToList(int iD, string name, string phone, int delivered, int NotDelivered, int recived)
            {
                ID = iD;
                Name = name;
                Phone = phone;
                SentAndDeliverd = delivered;
                SentAndNOTDeliverd = NotDelivered;
                ParcelsRecived = recived;
            }

        }
    }
}
