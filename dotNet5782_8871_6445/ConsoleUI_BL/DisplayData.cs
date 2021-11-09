﻿using IBL;
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
            List<string> stations = IBL.DispalyAllStations();
            foreach (string station in stations)
                Console.WriteLine(station);
        }

        public static void Drones(IBL.BL IBL)
        {
            List<string> drones = IBL.DispalyAllDrones();
            foreach (string drone in drones)
                Console.WriteLine(drone);
        }

        public static void Customers(IBL.BL IBL)
        {
            
            List<string> customers = IBL.DispalyAllCustomers();
            foreach (string customer in customers)
                Console.WriteLine(customer);
        }

        public static void Parcels(IBL.BL IBL)
        {
            List<string> parcels = IBL.DispalyAllParcels();
            foreach (string parcel in parcels)
                Console.WriteLine(parcel);
        }

        public static void UnassignedParcels(IBL.BL IBL)
        {
            List<string> UnassiPars = IBL.DispalyAllUnassignedParcels();
            foreach (string UnassiPar in UnassiPars)
                Console.WriteLine(UnassiPar);
        }

        public static void AvailableStations(IBL.BL IBL)
        {
            List<string> AvailStats = IBL.DispalyAllAvailableStations();
            foreach (string AvailStat in AvailStats)
                Console.WriteLine(AvailStat);
        }
    }
}
