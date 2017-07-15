using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikerBackend.Entities
{
    public class Route
    {
        [Key]
        public int RouteId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime BeginTime {get;set;}
        public DateTime EndTime { get; set; }
        public double StartLocationLatitude { get; set; }
        public double StartLocationLongitude{ get; set; }
        public double EndLocationLatitude { get; set; }
        public double EndLocationLongitude { get; set; }
    }
}
