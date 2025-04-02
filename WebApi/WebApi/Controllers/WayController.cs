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
        private readonly IService<Way,string> _service;
        public WayController(IService<Way,string> service, IAlgorithem algorithem)
        {
            _service = service;
            this.algorithem = algorithem;
        }
        // GET: api/<RouteController>
        [HttpGet]
        public async void Get()
        {
            
        }

        // GET api/<RouteController>/5
        [HttpGet("initialization")]
        public async Task<List<Node>> Get([FromQuery] double startLat, [FromQuery] double startLon, [FromQuery] double endLat, [FromQuery] double endLon)
        {
            return await algorithem.CalculateRoute(new Node { Lat = startLat, Lon = startLon }, new Node { Lat = endLat, Lon = endLon });//כי אני צריכה למצוא נקודה הכי קרובה לנקודה שקיבלתי Id אני לא צריכה לשמור
        }

        // POST api/<RouteController>
        [HttpPost]
        //כשמשתמש מתיחיל מסלול חדש לא מוכר
        public void Post()
        {

        }

        // PUT api/<RouteController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }
        //לכאורה לא צריך אלא אם כן כאשר ידווח על מסלול שהוא בלתי חוקי אז הוא ימחק
        // DELETE api/<RouteController>/5
        [HttpDelete("{id}")]
        public Task<Way> Delete(string id)
        {
            return _service.Delete(id);
        }
    }
}
