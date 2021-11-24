using System;
using IBL.BO;
using System.Collections.Generic;

namespace IBL
{
    namespace BO
    {
       public class CustomerBL
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public Location Location { get; set; }
            public List<ParcelAtCustomer> ParcelesSentByCustomer { get; set; }
            public List<ParcelAtCustomer> ParcelesSentToCustomer { get; set; }
            public CustomerBL(int id, string name, string phone, Location location)
            {
                
                ID = id;
                Name = name;
                Phone = phone;
                Location = location;
                ParcelesSentByCustomer = new();
                ParcelesSentToCustomer = new();
            }

            public override string ToString()
            {
                string StringSentByCustomer = string.Join("", ParcelesSentByCustomer);
                string StringSentToCustomer = string.Join("", ParcelesSentToCustomer);
                return ($"{Name} #{ID}:\n" +
                    $"Phone number:     {Phone}\n" +
                    $"Location:         {Location}\n" +
                    $"Parcel sent:       {StringSentByCustomer}\n" +
                    $"Parcel received:   {StringSentToCustomer}");
            }
        }
    }
    
}
