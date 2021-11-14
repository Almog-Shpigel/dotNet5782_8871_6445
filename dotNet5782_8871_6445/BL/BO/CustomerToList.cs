using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class CustomerToList
        {
            public int ID { get; set; }            
            public string Name { get; set; }
            public string Phone { get; set; }
            public int SentAndDeliverd { get; set; }
            public int SentAndNOTDeliverd { get; set; }
            public int ParcelsRecived { get; set; }
            public int ParcelsOnTheWay { get; set; }
            public CustomerToList(int iD = 0, string name = "", string phone = "", int delivered = 0, int NotDelivered = 0, int recived = 0)
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
