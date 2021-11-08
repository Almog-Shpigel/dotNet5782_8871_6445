using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    class Update
    {
        public static void DroneName(IBL.BL IBL)
        {
            IBL.UpdateDroneName(Request.DroneID(), Request.ModelName());
        }
        public static void Station(IBL.BL IBL)
        {
            IBL.UpdateStation(Request.StationID(), Request.StationName(), Request.ChargeSlots());
        }
        public static void Customer(IBL.BL IBL)
        {
            IBL.UpdateCustomer(Request.CustomerID("customer"), Request.CustomerName(), Request.PhoneNumber());
        }
        public static void PairParcelToDrone(IBL.BL IBL)
        {
            IBL.UpdateParcelToDrone(Request.DroneID());
        }

        public static void ParcelCollectedByDrone(IBL.BL IBL)
        {
            IBL.UpdateParcelCollectedByDrone(Request.DroneID());
        }

        public static void ParcelDeleiveredByDrone(IBL.BL IBL)
        {
            IBL.UpdateParcelDeleiveredByDrone(Request.DroneID);
        }

        public static void DroneToBeCharged(IBL.BL IBL)
        {
            IBL.UpdateDroneToBeCharged(Request.DroneID());
        }

        public static void DroneAvailable(IBL.BL IBL)
        {
            IBL.UpdateDroneAvailable(Request.DroneID(), Request.ChargeTime());
        }
    }
}
