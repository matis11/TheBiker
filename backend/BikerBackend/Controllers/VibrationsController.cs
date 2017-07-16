using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BikerBackend.DAL;
using BikerBackend.Entities;
using BikerBackend.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BikerBackend.Controllers
{
    [Route("api/[controller]")]
    public class VibrationsController : Controller
    {
        private BikerDbContext _dbContext;
        public VibrationsController()
        {
            _dbContext = new BikerDbContext();
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<VibrationData> Get()
        {
            return _dbContext.VibrationDatas.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public VibrationData Get(int id)
        {
            return _dbContext.VibrationDatas.Where(m => m.VibrationDataId == id).FirstOrDefault();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] VibrationData data)
        {
            var previousData = _dbContext.VibrationDatas.Where(vb => vb.RouteId == data.RouteId).OrderByDescending(vb => vb.TimeStamp).FirstOrDefault();
            if(previousData == null)   
            {
                //First mesurement in route
                var route = _dbContext.Routes.Where(r => r.RouteId == data.RouteId).FirstOrDefault();
                var routeStart = new RouteStart { BeginTime = route.BeginTime, StartLocationLatitude = route.StartLocationLatitude, StartLocationLongitude = route.StartLocationLongitude };
                SpeedCalculator.CalculateSpeed(routeStart, data);
            }

            else
            {
                SpeedCalculator.CalculateSpeed(previousData,data);
            }
            _dbContext.Add(data);
            _dbContext.SaveChanges();
        }

        // POST api/values
        [HttpPost("StartRoute")]
        public int StartRoute([FromBody] RouteStart route)
        {
            var newRoute = new Route {UserId = route.UserId, BeginTime = route.BeginTime, StartLocationLatitude = route.StartLocationLatitude, StartLocationLongitude = route.StartLocationLongitude };
            _dbContext.Add(newRoute);
            _dbContext.SaveChanges();
            return newRoute.RouteId;
        }

        // POST api/values
        [HttpPost("EndRoute")]
        public void EndRoute([FromBody] RouteEnd route)
        {
            var oldRoute = _dbContext.Routes.Where(r => r.RouteId == route.RouteId).FirstOrDefault();
            oldRoute.EndLocationLatitude = route.EndLocationLatitude;
            oldRoute.EndLocationLongitude = route.EndLocationLongitude;
            oldRoute.EndTime = route.EndTime;
            _dbContext.SaveChanges();

            var distortionData = _dbContext.VibrationDatas.Where(vd => vd.RouteId == route.RouteId).ToList();
            var finalData = DistortionRatioCalculator.CalculateDistortion(distortionData);

            foreach(var data in finalData)
            {
                _dbContext.Add(data);
            }
            _dbContext.SaveChanges();
        }
    }
}
