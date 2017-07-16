using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikerBackend.Entities
{
    public class RouteInfo
    {
        public Route Route { get; set; }
        public IEnumerable<VibrationData> Data { get; set; }
    }
}
