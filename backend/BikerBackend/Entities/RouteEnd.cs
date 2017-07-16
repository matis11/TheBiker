using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikerBackend.Entities
{
    public class RouteEnd
    {
        public int RouteId { get; set; }
        public DateTime EndTime { get; set; }
        public double EndLocationLatitude { get; set; }
        public double EndLocationLongitude { get; set; }
    }
}
