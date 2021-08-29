using MarsRobots.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MarsRobots.REST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarsMapController : ControllerBase
    {
        private static MarsRobots.Business.MarsRobotsBusiness marsMngr = new Business.MarsRobotsBusiness();

        // GET: api/<MarsRobotsController>
        [HttpGet]
        public IEnumerable<int> Get()
        {
            return marsMngr.GetMarsMaps().Select(m => m.Id);
        }

        // GET api/<MarsRobotsController>/5
        [HttpGet("{id}")]
        public Entities.MarsMap Get(int id)
        {
            return marsMngr.GetMarsMap(id);
        }

        // POST api/<MarsRobotsController>
        [HttpPost]
        public void Post([FromBody] MarsMap value)
        {
            marsMngr.UpdateMap(value);
        }

        // PUT api/<MarsRobotsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] MarsMap value)
        {
            marsMngr.InsertMap(value);
        }

        // DELETE api/<MarsRobotsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            marsMngr.DeleteMap(id);
        }
    }
}
