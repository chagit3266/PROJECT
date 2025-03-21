using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoutesController : ControllerBase
    {
        private readonly IService<UserRoutes> _service;
        public UserRoutesController(IService<UserRoutes> service)
        {
            _service = service;
        }
        // GET: api/<UserRoutesController>
        [HttpGet]
        public Task<List<UserRoutes>> Get()
        {
            return _service.GetAll();
        }

        // GET api/<UserRoutesController>/5
        [HttpGet("{id}")]
        public Task<UserRoutes> Get(int id)
        {
            return _service.GetById(id);
        }

        // POST api/<UserRoutesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserRoutesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserRoutesController>/5
        [HttpDelete("{id}")]
        public Task<UserRoutes> Delete(int id)
        {
            return _service.Delete(id);
        }
    }
}
