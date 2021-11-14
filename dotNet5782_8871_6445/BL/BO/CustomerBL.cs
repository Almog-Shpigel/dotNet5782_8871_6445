using System;
using IBL.BO;
using System.Collections.Generic;

namespace IBL
{
    namespace BO
    {
        class CustomerBL
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public Location Location { get; set; }
            public IEnumerable<ParcelBL> ParcelesSentByCustomer { get; set; }
            public IEnumerable<ParcelBL> ParcelesSentToCustomer { get; set; }
            public CustomerBL(int id, string name, string phone,
                            double longitude, double latitude)
            {
                ID = id;
                Name = name;
                Phone = phone;
                Location.Longitude = longitude;
                Location.Latitude = latitude;
            }
            public override string ToString()
            {
                return ($"{Name} #{ID}, Phone number: {Phone}, location: {Location}");
            }
        }
    }
    
}
