using IBL;
using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    class DisplayData
    {
        public static void Stations(IBL.BL IBL)
        {
            List<StationToList> stations = IBL.DispalyAllStations();
            foreach (var station in stations)
                Console.WriteLine(station.ToString());
        }

        public static void Drones(IBL.BL IBL)
        {
            List<DroneToList> drones = IBL.DispalyAllDrones();
            foreach (var drone in drones)
                Console.WriteLine(drone.ToString());
        }

        public static void Customers(IBL.BL IBL)
        {

            List<CustomerToList> customers = IBL.DispalyAllCustomers();
            foreach (var customer in customers)
                Console.WriteLine(customer.ToString());
        }

        public static void Parcels(IBL.BL IBL)
        {
            List<ParcelToList> parcels = IBL.DispalyAllParcels();
            foreach (var parcel in parcels)
                Console.WriteLine(parcel.ToString());
        }

        public static void UnassignedParcels(IBL.BL IBL)
        {
            List<ParcelToList> UnassiPars = IBL.DispalyAllUnassignedParcels();
            foreach (var UnassiPar in UnassiPars)
                Console.WriteLine(UnassiPar.ToString());
        }

        public static void AvailableStations(IBL.BL IBL)
        {
            List<StationToList> AvailStats = IBL.DispalyAllAvailableStations();
            foreach (var AvailStat in AvailStats)
                Console.WriteLine(AvailStat.ToString());
        }
    }
}
