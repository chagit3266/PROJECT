using Microsoft.AspNetCore.Mvc;
using Repository.Entities;
using Service.Interfaces;
using WebApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }
        // GET: api/<UserController>
        [HttpGet]
        public Task<List<User>> Get()//GetAll
        {
            return _service.GetAll();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public Task<User> Get(int id)
        {
            return _service.GetById(id);
        }

        // POST api/<UserController>
        [HttpPost("login")]
        public async Task<IActionResult> Post(UserLogin userLogin)
        {
            //אבטחה
            var user =(await _service.Authenticate(userLogin.Email, userLogin.Password));
            if (user != null)
            {
                var token = await _service.Generate(user);
                return Ok(token);
            }
            return BadRequest("Invalid email or password.");

        }
        //[HttpPost("")]
        //public async Task<IActionResult> Post(User user)
        //{
            
        //}
        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<User> Put(int id, [FromBody] User item)
        {
            return await _service.Update(id,item);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public Task<User> Delete(int id)
        {
            return _service.Delete(id);
        }
        
    }
}
