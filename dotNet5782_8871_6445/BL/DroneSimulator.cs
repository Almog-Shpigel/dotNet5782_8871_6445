using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BO.EnumsBL;
using DO;

namespace BL
{
    internal class DroneSimulator
    {
        private const double SPEED = 2.0;
        private const int DELAY = 500;

        public DroneSimulator(BlApi.BL bl, int droneID, Action updateView, Func<bool> checkIfCanceled)
        {
            DroneBL drone;
            DroneToList droneToList;
            while (!checkIfCanceled())
            {
                drone = bl.GetDrone(droneID);
                droneToList = new(drone.ID, drone.Model, drone.MaxWeight, drone.BatteryStatus, drone.Status, drone.CurrentLocation, drone.Parcel.ID);
                switch (drone.Status)
                {
                    case DroneStatus.Available:
                        lock (bl) lock (bl.Data)
                            {
                                bl.UpdateParcelAssignToDrone(droneID);
                            }
                        break;
                    case DroneStatus.Charging:
                        lock (bl) lock (bl.Data)
                            {
                                bl.UpdateDroneToBeAvailable(droneID);
                                drone = bl.GetDrone(droneID);
                                if (drone.BatteryStatus < 100)
                                    bl.UpdateDroneToBeCharged(droneID);
                            }
                        break;
                    case DroneStatus.Delivery:
                        Parcel ParcelToBeCollected;
                        ParcelToBeCollected = bl.Data.GetParcel(droneToList.ParcelID);
                        if (ParcelToBeCollected.PickedUp == null)
                            bl.UpdateParcelCollectedByDrone(droneID);
                        if (drone.Parcel.DeliveryDistance <= 0.1)
                        {
                            bl.UpdateParcelDeleiveredByDrone(droneID);
                            bl.UpdateDroneToBeCharged(droneID);
                        }
                        else
                            drone.Parcel.DeliveryDistance *= 0.90;
                        break;
                }
            }
        }
    }
}
