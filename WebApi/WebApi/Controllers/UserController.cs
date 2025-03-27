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
        [HttpPost("signIn")]
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
        [HttpPost("signUp")]
        public async Task<IActionResult> Post(User user)
        {
            if (await _service.Authenticate(user.Email, user.Password) == null)
            {
                await _service.Add(user);
                return Ok(user);
            }
            return BadRequest("User already exists."); //singInבקלינט נשלח אחרי התחברות או אם כבר רשום ל
        }
        // PUT api/<UserController>/5
        [HttpPut("{id}")]//עדכון לפי מייל
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
