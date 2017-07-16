using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikerBackend.Entities
{
    public class DataEqualityComparer : IEqualityComparer<VibrationData>
    {
        public bool Equals(VibrationData x, VibrationData y)
        {
            if (x.LocationLatitude == y.LocationLatitude && x.LocationLongitude == y.LocationLongitude) return true;
            else return false;
        }

        public int GetHashCode(VibrationData obj)
        {
            return obj.GetHashCode();
        }
    }
}
