using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class EnumsBL
        {
            /// Enums for all the menus:
            public enum DroneStatus { Available, Charging, Delivery, Any }
            public enum ParcelStatus { Requested, Scheduled , PickedUp , Delivered }
        }
    }
}
