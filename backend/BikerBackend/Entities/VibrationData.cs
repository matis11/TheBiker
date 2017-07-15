using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BikerBackend.Entities
{
    public class VibrationData
    {
        [Key]
        public int VibrationDataId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public DateTime TimeStamp { get; set; }
        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }

        public Route Route { get; set; }
        public int RouteId { get; set; }
    }
}
