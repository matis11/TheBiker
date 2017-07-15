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
            _dbContext.Add(data);
            _dbContext.SaveChanges();
        }
    }
}
