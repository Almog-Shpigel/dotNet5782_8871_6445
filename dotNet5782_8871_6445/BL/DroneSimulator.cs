using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static BO.EnumsBL;
using DO;
using BlApi;

namespace BL
{
    internal class DroneSimulator
    {
        private const double SPEED = 2.0;
        private const int DELAY = 1000;

        public DroneSimulator(BlApi.BL bl, int droneID, Action updateView, Func<bool> checkIfCanceled)
        {
            DroneBL drone = bl.GetDrone(droneID);
            DroneToList droneToList = new(drone.ID, drone.Model, drone.MaxWeight, drone.BatteryStatus, drone.Status, drone.CurrentLocation, drone.Parcel.ID);
            double distance = drone.Parcel.DeliveryDistance;
            while (!checkIfCanceled())
            {
                 drone = bl.GetDrone(droneID);
                 droneToList = new(drone.ID, drone.Model, drone.MaxWeight, drone.BatteryStatus, drone.Status, drone.CurrentLocation, drone.Parcel.ID);
                switch (drone.Status)
                {
                    case DroneStatus.Available:
                        var AllAvailableParcels = bl.Data.GetParcels(p => p.DroneID == 0).Where(p => bl.PossibleDelivery(droneToList, p))
                            .Where(p => drone.MaxWeight >= p.Weight).OrderBy(p => p.Priority).ThenBy(p => p.Weight);
                        Parcel MaxParcel = AllAvailableParcels.FirstOrDefault();
                        if (!sleepDelayTime()) break;
                        if ((MaxParcel.ID == 0 && drone.BatteryStatus < 100) || !bl.PossibleDelivery(droneToList, MaxParcel))
                        {
                            Station near = bl.GetNearestStation(drone.CurrentLocation, bl.Data.GetStations(station => station.ChargeSlots > 0));
                            if (drone.BatteryStatus < bl.Distance(droneToList.CurrentLocation, new(near.Latitude, near.Longitude)) * bl.BatteryUsageEmpty)
                            {
                                near = bl.GetNearestStation(drone.CurrentLocation, bl.Data.GetStations(station => true));
                                StationBL nearStation = bl.GetStation(near.ID);
                                bl.UpdateStationSlots(nearStation.ID, nearStation.ChargingDrones.Count() + 1, nearStation.ChargingDrones.Count());
                                updateView();
                            }
                            lock (bl) lock (bl.Data)
                                {
                                    bl.UpdateDroneToBeCharged(drone.ID);
                                    updateView();
                                }
                        }
                        else if (MaxParcel.ID != 0 && bl.PossibleDelivery(droneToList,MaxParcel))
                            {
                            lock (bl) lock (bl.Data)
                                {
                                    bl.UpdateParcelAssignToDrone(drone.ID);
                                    updateView();
                                }
                            }
                        break;
                    case DroneStatus.Charging:
                        lock (bl) lock (bl.Data)
                            {
                                bl.UpdateDroneToBeAvailable(drone.ID);
                                drone = bl.GetDrone(drone.ID);
                                if (drone.BatteryStatus < 100)
                                    bl.UpdateDroneToBeCharged(drone.ID);
                                updateView();
                            }
                        break;
                    case DroneStatus.Delivery:
                        Parcel ParcelToBeCollected = bl.Data.GetParcel(drone.Parcel.ID);
                        double distanceDroneCustomer = bl.DistanceDroneCustomer(droneToList, bl.Data.GetCustomer(ParcelToBeCollected.SenderID));
                        if (ParcelToBeCollected.PickedUp == null)
                        {
                            //TO DO: Distance prograss from the drone and the customer
                            bl.UpdateParcelCollectedByDrone(droneID);
                        }
                        else
                        {
                            if (distance <= 1)
                            {
                                bl.UpdateParcelDeleiveredByDrone(drone.ID);
                                updateView();
                            }
                            else
                            {
                                distance -= 1;
                            }
                        }                       
                        break;
                }
                updateView();
                Thread.Sleep(DELAY);
            }
        }

        private static bool sleepDelayTime()
        {
            try { Thread.Sleep(DELAY); } catch (ThreadInterruptedException) { return false; }
            return true;
        }
    }
}
