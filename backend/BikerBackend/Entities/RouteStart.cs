using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikerBackend.Entities
{
    public class RouteStart
    {
        public int UserId { get; set; }
        public DateTime BeginTime { get; set; }
        public double StartLocationLatitude { get; set; }
        public double StartLocationLongitude { get; set; }
    }
}
