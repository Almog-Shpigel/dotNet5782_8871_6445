﻿using IDAL.DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI_BL
{
    class Add
    {
        public static void NewStation(IBL.BL IBL)
        {
            IBL.AddNewStation(Request.StationID(), Request.StationName(), Request.Longitude(), Request.Latitude(), Request.ChargeSlots());
        }

        public static void NewDrone(IBL.BL IBL)
        {
            IBL.AddNewDrone(Request.DroneID(), Request.ModelName(), Request.WeightCategorie(), Request.StationID());
        }

        public static void NewCustomer(IBL.BL IBL)
        {
            IBL.AddNewCustomer(Request.CustomerID("customer"), Request.CustomerName(), Request.PhoneNumber(), Request.Longitude(), Request.Latitude());
        }

        public static void NewParcel(IBL.BL IBL)
        {
            IBL.AddNewParcel(Request.CustomerID("sender"), Request.CustomerID("receiver"), Request.WeightCategorie(), Request.Prioritie());
        }

    }
}