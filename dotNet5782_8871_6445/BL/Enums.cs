﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class Enums
        {
            /// Enums for all the menus:
            public enum CHOICE { EXIT, ADD, UPDATE, DISPLAY, DATA_PRINT }
            public enum ADD_CHOICE { ADD_STATION, ADD_DRONE, ADD_CUSTOMER, ADD_PARCEL }
            public enum UPDATE_CHOICE { PARCEL_PAIRING, PARCEL_COLLECTED, PARCEL_DELEIVERY, DRONE_TO_CHARGE, DRONE_AVAILABLE }
            public enum DISPLAY_CHOICE { DISPLAY_STATION, DISPLAY_DRONE, DISPLAY_CUSTOMER, DISPLAY_PARCEL, DISTANCE_STATION, DISTANCE_CUSTOMER }
            public enum PRINT_CHOICE { PRINT_STATIONS, PRINT_DRONES, PRINT_CUSTOMERS, PRINT_PARCELS, PRINT_UNASSIGNED_PARCELS, PRINT_AVAILABLE_STATIONS }
            public enum WeightCategories { Light, Medium, Heavy }
            public enum Priorities { Regular, Express, Urgent }
            public enum ParcelStatus { Requested, Scheduled , PickedUp , Delivered }
            public enum DroneModels { Mavic, Skydio, Phantom }
            public enum CustomerNames { Almog, Devora, Amitai, Alon, Gila, Rachel, Yair, Yishai, Ariel, David, Merav, Yoav, Noam, Efart, Rotem, Dor, Shoshana }
        }
    }
}