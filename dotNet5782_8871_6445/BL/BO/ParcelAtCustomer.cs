using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL.DO;
using static IBL.BO.EnumsBL;

namespace IBL
{
    namespace BO
    {
       public class ParcelAtCustomer
        {
            public int ID { get; set; }

            public WeightCategories Weight { get; set; }
            public Priorities Priority { get; set; }
            public ParcelStatus Status { get; set; }
            public CustomerInParcel Customer { get; set; }

            public ParcelAtCustomer(int iD, WeightCategories weight, Priorities priority, ParcelStatus status, CustomerInParcel customer)
            {
                ID = iD;
                Weight = weight;
                Priority = priority;
                Status = status;
                Customer = customer;
            }

        }
    }
}
