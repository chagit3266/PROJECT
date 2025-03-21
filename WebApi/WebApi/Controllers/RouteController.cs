using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;
using Way = Repository.Entities.Way;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly IService<Way> _service;
        public RouteController(IService<Way> service)
        {
            _service = service;
        }
        // GET: api/<RouteController>
        [HttpGet]
        public Task<List<Way>> Get()
        {
           return _service.GetAll();
        }

        // GET api/<RouteController>/5
        [HttpGet("{id}")]
        public Task<Way> Get(int id)
        {
            return _service.GetById(id);
        }

        // POST api/<RouteController>
        [HttpPost]
        public void Post()
        {

        }

        // PUT api/<RouteController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/<RouteController>/5
        [HttpDelete("{id}")]
        public Task<Way> Delete(int id)
        {
            return _service.Delete(id);
        }
    }
}
