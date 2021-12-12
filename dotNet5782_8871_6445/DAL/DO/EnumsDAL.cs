using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    namespace DO
    {
        /// Enums for all the menus:
        public enum CHOICE { EXIT, ADD, UPDATE, DISPLAY, DATA_PRINT }
        public enum ADD_CHOICE { ADD_STATION = 1, ADD_DRONE, ADD_CUSTOMER, ADD_PARCEL }
        public enum UPDATE_CHOICE { DRONE = 1, STATION, CUSTOMER, PARCEL_PAIRING, PARCEL_COLLECTED, PARCEL_DELEIVERY, DRONE_TO_CHARGE, DRONE_AVAILABLE }
        public enum DISPLAY_CHOICE { DISPLAY_STATION = 1, DISPLAY_DRONE, DISPLAY_CUSTOMER, DISPLAY_PARCEL, DISTANCE_STATION, DISTANCE_CUSTOMER }
        public enum PRINT_CHOICE { PRINT_STATIONS = 1, PRINT_DRONES, PRINT_CUSTOMERS, PRINT_PARCELS, PRINT_UNASSIGNED_PARCELS, PRINT_AVAILABLE_STATIONS }
        public enum WeightCategories { Light, Medium, Heavy, Any }
        public enum Priorities { Regular, Express, Urgent }
        public enum DroneModels { Mavic, Skydio, Phantom }
        public enum CustomerNames { Almog, Devora, Amitai, Alon, Gila, Rachel, Yair, Yishai, Ariel, David, Merav, Yoav, Noam, Efart, Rotem, Dor, Shoshana }
    }
}