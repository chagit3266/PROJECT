using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WayController : ControllerBase
    {
        private readonly IAlgorithem algorithem;
        private readonly IService<Way> _service;
        public WayController(IService<Way> service, IAlgorithem algorithem)
        {
            _service = service;
            this.algorithem = algorithem;
        }
        // GET: api/<RouteController>
        [HttpGet]
        public async Task<ActionResult<List<Way>>> Get()
        {
            string join= await algorithem.GetOsmData(51.5, -0.1);
            return algorithem.ExtractWays(join);
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
