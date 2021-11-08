using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    class Update
    {
        public static void DroneName(BL.BL IBL)
        {
            IBL.UpdateDroneName(Request.DroneID(), Request.ModelName());
        }
        public static void Station(BL.BL IBL)
        {
            IBL.UpdateStation(Request.StationID(), Request.StationName(), Request.ChargeSlots());
        }
        public static void Customer(BL.BL IBL)
        {
            IBL.UpdateCustomer(Request.CustomerID("customer"), Request.CustomerName(), Request.PhoneNumber());
        }
        public static void PairParcelToDrone(BL.BL IBL)
        {
            IBL.UpdateParcelToDrone(Request.DroneID());
        }

        public static void ParcelCollectedByDrone(BL.BL IBL)
        {
            IBL.UpdateParcelCollectedByDrone(Request.DroneID());
        }

        public static void ParcelDeleiveredByDrone(BL.BL IBL)
        {
            IBL.UpdateParcelDeleiveredByDrone(Request.DroneID);
        }

        public static void DroneToBeCharged(BL.BL IBL)
        {
            IBL.UpdateDroneToBeCharged(Request.DroneID());
        }

        public static void DroneAvailable(BL.BL IBL)
        {
            IBL.UpdateDroneAvailable(Request.DroneID(), Request.ChargeTime());
        }
    }
}
