using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BikerBackend.DAL;
using BikerBackend.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BikerBackend.Controllers
{
    [Route("api/[controller]")]
    public class RoutesController : Controller
    {
        private BikerDbContext _dbContext;

        public RoutesController()
        {
            _dbContext = new BikerDbContext();
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Route> Get()
        {
            return _dbContext.Routes.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Route Get(int id)
        {
            return _dbContext.Routes.Where(r => r.RouteId == id).FirstOrDefault();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Route data)
        {
            _dbContext.Add(data);
            _dbContext.SaveChanges();
        }
    }
}
