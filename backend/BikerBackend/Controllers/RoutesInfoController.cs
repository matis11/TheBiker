using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BikerBackend.Entities;
using BikerBackend.DAL;

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

            foreach(var userRoute in userRoutes)
            {
                var dataForRoute = _dbContext.VibrationDatas.Where(vd => vd.RouteId == id).ToList();
                var result = new RouteInfo { Route = userRoute, Data = dataForRoute };
                resultList.Add(result);
            }
            return resultList;
        }
    }
}
