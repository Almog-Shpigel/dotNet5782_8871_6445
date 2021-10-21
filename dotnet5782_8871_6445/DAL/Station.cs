﻿using DalObject;
using static DalObject.DataSource;

namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int ChargeSlots { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }

            public Station(int id, string name, int chargeSlots,
                           double longitude, double latitude)
            {
                
                ID = id;
                Name = name;
                ChargeSlots = chargeSlots;
                Longitude = longitude;
                Latitude = latitude;
            }
            public void print()
            {
                string newLong, newLatt;

                System.Console.WriteLine($"{Name} #{ID}:\n" +
                    $"Charge slots available: {ChargeSlots}\n" +
                    $"Location: {Longitude}, {Latitude}\n");
            }
        }
    }
}