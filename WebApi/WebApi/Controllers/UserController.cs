using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()//GetAll
        {
            
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            
        }

        // POST api/<UserController>
        [HttpPost("{username}/{password}")]
        public IActionResult Post(string username,string password)
        {
            //אבטחה
            var user = Authenticate(username, password);
            if (user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }
            return BadRequest("");

        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        private string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey();

        }
    }
}
