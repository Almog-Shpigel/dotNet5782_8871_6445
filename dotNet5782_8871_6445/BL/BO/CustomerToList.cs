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
            public CustomerToList(int id = 0, string name = "", string phone = "", int delivered = 0, int NotDelivered = 0, int recived = 0)
            {
                ID = id;
                Name = name;
                Phone = phone;
                SentAndDeliverd = delivered;
                SentAndNOTDeliverd = NotDelivered;
                ParcelsRecived = recived;
            }
            public override string ToString()
            {
                return $"Name: {Name}, #{ID}\n" +
                    $"phone number: {Phone}\tDeliverd: {SentAndDeliverd}    Not deliverd: {SentAndNOTDeliverd}  Parcels received: {ParcelsRecived}  Parcels on the way: {ParcelsOnTheWay}";
            }
        }
    }
}
