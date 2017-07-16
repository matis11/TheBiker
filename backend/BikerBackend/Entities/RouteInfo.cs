using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikerBackend.Entities
{
    public class RouteInfo
    {
        public Route Route { get; set; }
        public double MinimumSpeed { get; set; }
        public double MaximumSpeed { get; set; }
        public double AverageSpeed { get; set; }
        public string Duration { get; set; }
        public double Distance { get; set; }
        public IEnumerable<VibrationData> Data { get; set; }
    }
}
