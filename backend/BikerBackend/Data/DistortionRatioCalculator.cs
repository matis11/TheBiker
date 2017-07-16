using BikerBackend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikerBackend.Data
{
    public static class DistortionRatioCalculator
    {
        public static IEnumerable<FinalData> CalculateDistortion(IEnumerable<VibrationData> data)
        {
            var sortedList = data.OrderBy(d => d.TimeStamp);
            var resultList = new List<FinalData>();

            foreach(var element in sortedList)
            {
                var result = new FinalData { Latitude = element.LocationLatitude, Longitude = element.LocationLongitude, Speed = element.Speed };
                if(Math.Abs(element.Y)>1)
                {
                    result.SurfaceDistortionRatio = 1;
                }
                resultList.Add(result);
            }

            for(var i =0; i< sortedList.Count(); i++)
            {
                if(resultList[i].SurfaceDistortionRatio >= 0.9)
                {
                    if(i > 0)
                    {
                        resultList[i - 1].SurfaceDistortionRatio += 0.1;
                    }

                    if(i < sortedList.Count() -1)
                    {
                        resultList[i + 1].SurfaceDistortionRatio += 0.1;
                    }

                    resultList[i].SurfaceDistortionRatio -= 0.1;
                }
            }

            return resultList;
        }
    }
}
