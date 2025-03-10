using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointController : ControllerBase
    {
        // GET: api/<PointController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            
        }

        // GET api/<PointController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            
        }

        // POST api/<PointController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PointController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PointController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

        }
    }
}
