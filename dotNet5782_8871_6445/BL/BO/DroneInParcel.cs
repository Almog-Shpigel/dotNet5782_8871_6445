using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class DroneInParcel
{
    public int ID { get; set; }
    public double BatteryStatus { get; set; }
    public Location CurrentLocation { get; set; }
    public DroneInParcel(int id, double battery, Location location)
    {
        ID = id;
        BatteryStatus = battery;
        CurrentLocation = location;
    }
    public override string ToString()
    {
        return  $"Drone #{ID}:\n" +
                $"Location: {CurrentLocation}\n" +
                $"Battery: {BatteryStatus}%";
    }
}

