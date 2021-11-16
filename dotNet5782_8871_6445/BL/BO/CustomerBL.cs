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
            public List<ParcelBL> ParcelesSentByCustomer { get; set; }
            public List<ParcelBL> ParcelesSentToCustomer { get; set; }
            public CustomerBL(int id, string name, string phone, Location location)
            {
                if (id < 100000000 || id > 999999999)
                    throw new InvalidIDException("Invalid customer ID number");
                char str = phone[0];
                bool success = int.TryParse(phone, out int PhoneNumber);
                if (success || str != '0' || PhoneNumber < 500000000 || PhoneNumber > 5999999999) ///Checking if the number starts with a '05' and contain 10 numbers
                    throw new InvalidPhoneNumberException("Invalid phone number");
                if ((int)location.Latitude != 31 || (int)location.Longitude != 35)
                    throw new OutOfRangeLocationException("The location is outside of Jerusalem"); ///We assume for now that all the locations
                ID = id;
                Name = name;
                Phone = phone;
                Location = location;
                ParcelesSentByCustomer = new();
                ParcelesSentToCustomer = new();
            }
            public override string ToString()
            {
                return ($"{Name} #{ID}, Phone number: {Phone}, location: {Location}");
            }
        }
    }
    
}
