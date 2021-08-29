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
    public class RobotController : ControllerBase
    {
        private static MarsRobots.Business.MarsRobotsBusiness marsMngr = new Business.MarsRobotsBusiness();

        // GET: api/<RobotController>
        [HttpGet]
        public IEnumerable<int> Get()
        {
            return marsMngr.GetRobots().Select(r => r.Id);
        }

        // GET api/<RobotController>/5
        [HttpGet("{id}")]
        public Robot Get(int id)
        {
            return marsMngr.GetRobot(id);
        }

        // POST api/<RobotController>
        [HttpPost]
        public void Post([FromBody] Robot value)
        {
            marsMngr.InsertRobot(value);
        }

        // PUT api/<RobotController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Robot value)
        {
            marsMngr.UpdateRobot(value);
        }

        // DELETE api/<RobotController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            marsMngr.DeleteRobot(id);
        }
    }
}
