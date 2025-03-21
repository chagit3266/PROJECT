using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointController : ControllerBase
    {
        private readonly IService<Node> _service;
        public PointController(IService<Node> service)
        {
            _service = service;
        }

        // GET: api/<PointController>
        [HttpGet]
        public Task<List<Node>> Get()
        {

            return _service.GetAll();
        }

        // GET api/<PointController>/5
        [HttpGet("{id}")]
        public Task<Node> Get(int id)
        {
            return _service.GetById(id);
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
        public Task<Node> Delete(int id)
        {
            return _service.Delete(id);
        }
    }
}
