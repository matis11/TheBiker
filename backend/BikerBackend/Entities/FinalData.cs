using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikerBackend.Entities
{
    public class FinalData
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double SurfaceDistortionRatio { get; set; }
    }
}
