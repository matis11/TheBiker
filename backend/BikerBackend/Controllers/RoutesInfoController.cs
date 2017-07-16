using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BikerBackend.Entities;
using BikerBackend.DAL;
using BikerBackend.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BikerBackend.Controllers
{
    [Route("api/[controller]")]
    public class RoutesInfoController : Controller
    {
        private BikerDbContext _dbContext;

        public RoutesInfoController()
        {
            _dbContext = new BikerDbContext();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public RouteInfo Get(int id)
        {
            var route = _dbContext.Routes.Where(r => r.RouteId == id).FirstOrDefault();
            var dataForRoute = _dbContext.VibrationDatas.Where(vd => vd.RouteId == id).ToList();
            var routeInfo = new RouteInfo { Route = route, Data = dataForRoute };
            return routeInfo;
        }

        [HttpGet("RoutesInfoForUser/{id}")]
        public IEnumerable<RouteInfo> GetRoutesInfoForUser(int id)
        {
            var userRoutes = _dbContext.Routes.Where(r => r.UserId == id).ToList();
            var resultList = new List<RouteInfo>();
            var comparer = new DataEqualityComparer();

            foreach(var userRoute in userRoutes)
            {
                var dataForRoute = _dbContext.VibrationDatas.Where(vd => vd.RouteId == userRoute.RouteId).ToList();
                var minSpeed = 0.0;
                var maxSpeed = 0.0;
                var averageSpeed = 0.0;
                var distance = 0.0;
                var timeSpan = userRoute.EndTime - userRoute.BeginTime;
                var duration = $"{timeSpan.Value.Hours.ToString("00")}:{timeSpan.Value.Minutes.ToString("00")}:{timeSpan.Value.Seconds.ToString("00")}";

                if (dataForRoute.Any())
                {
                    minSpeed = dataForRoute.First().Speed;
                    maxSpeed = dataForRoute.First().Speed;

                    foreach (var data in dataForRoute)
                    {
                        if (data.Speed < minSpeed) minSpeed = data.Speed;
                        if (data.Speed > maxSpeed) maxSpeed = data.Speed;
                        averageSpeed += data.Speed;
                    }

                    averageSpeed /= dataForRoute.Count();
                    for(var i = 0; i < dataForRoute.Count(); i++)
                    {
                        if(i==0)
                        {
                            distance += SpeedCalculator.CalculateDistance(userRoute.StartLocationLongitude, userRoute.StartLocationLatitude, dataForRoute[i].LocationLongitude, dataForRoute[i].LocationLatitude);
                        }

                        else if(i== dataForRoute.Count()-1)
                        {
                            distance += SpeedCalculator.CalculateDistance(dataForRoute[i].LocationLongitude, dataForRoute[i].LocationLatitude, userRoute.EndLocationLongitude.Value, userRoute.EndLocationLatitude.Value);
                        }

                        else
                        {
                            distance += SpeedCalculator.CalculateDistance(dataForRoute[i-1].LocationLongitude, dataForRoute[i-1].LocationLatitude, dataForRoute[i].LocationLongitude, dataForRoute[i].LocationLatitude);
                        }
                    }
                }

                else
                {
                    distance = SpeedCalculator.CalculateDistance(userRoute.StartLocationLongitude, userRoute.StartLocationLatitude, userRoute.EndLocationLongitude.Value, userRoute.EndLocationLatitude.Value);
                    var timeDiff = userRoute.EndTime - userRoute.BeginTime;
                    var speed = distance / Math.Abs(timeDiff.Value.TotalHours);
                    minSpeed = speed;
                    maxSpeed = speed;
                    averageSpeed = speed;
                }
                
                var result = new RouteInfo { Route = userRoute, Data = dataForRoute.Distinct(comparer),MaximumSpeed = maxSpeed, MinimumSpeed = minSpeed, AverageSpeed = averageSpeed, Distance = distance, Duration = duration };
                resultList.Add(result);
            }
            return resultList;
        }
    }
}
