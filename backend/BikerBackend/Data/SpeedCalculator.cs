using BikerBackend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikerBackend.Data
{
    public static class SpeedCalculator
    {
        public static void CalculateSpeed(VibrationData previousData, VibrationData data)
        {
            var distance = CalculateDistance(previousData.LocationLongitude, previousData.LocationLatitude, data.LocationLongitude, data.LocationLatitude);
            var timeDiff = previousData.TimeStamp - data.TimeStamp;
            var speed = distance / Math.Abs(timeDiff.TotalHours);
            data.Speed = speed;
        }

        public static void CalculateSpeed(RouteStart routeStart, VibrationData data)
        {
            var distance = CalculateDistance(routeStart.StartLocationLongitude, routeStart.StartLocationLatitude, data.LocationLongitude, data.LocationLatitude);
            var timeDiff = routeStart.BeginTime - data.TimeStamp;
            var speed = distance / Math.Abs(timeDiff.TotalHours);
            data.Speed = speed;
        }

        private static double CalculateDistance(double firstLong, double firstLat, double secondLong, double secondLat)
        {
            //Rough estimation
            var longitudeDiff = Math.Abs(secondLong - firstLong);
            var latitudeDiff = Math.Abs(secondLat - firstLat);
            var geoDistance = Math.Sqrt(Math.Pow(longitudeDiff, 2) + Math.Pow(latitudeDiff, 2)) * 77;
            return geoDistance;
        }
    }
}
