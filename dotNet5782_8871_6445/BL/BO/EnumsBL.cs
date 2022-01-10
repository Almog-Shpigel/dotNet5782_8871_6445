using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace BO
    {
        public class EnumsBL
        {
            /// Enums for all the menus:
            public enum DroneStatus { Available, Charging, Delivery }
            public enum ParcelStatus { Requested, Scheduled , PickedUp , Delivered }
        }
    }
