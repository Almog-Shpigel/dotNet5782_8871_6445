using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    internal static class Distance
    {
        public static double CalcDistance(Location from, Location to)
        {
            int radius = 6371 * 1000;
            double phi1 = from.Latitude * Math.PI / 180;
            double phi2 = to.Latitude * Math.PI / 180;
            double deltaPhi = (to.Latitude - from.Latitude) * Math.PI / 180;
            double deltaLambda = (to.Longitude - from.Longitude) * Math.PI / 180;
            double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi) + Math.Cos(phi1) * Math.Cos(phi2) * Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            double b = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double c = radius * b / 1000;
            return c;
        }
    }
}
