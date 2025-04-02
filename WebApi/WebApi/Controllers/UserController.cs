using Azure.Core;
using Google.Apis.Auth;
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
            // חיפוש המשתמש לפי אימייל
            var user = await _service.GetByEmail(userLogin.Email);

            if (user == null)
            {
                return NotFound("User not found"); // 404 - המשתמש לא קיים
            }

            // בדיקת סיסמה
            var isAuthenticated = await _service.Authenticate(userLogin.Email, userLogin.Password);

            if (isAuthenticated==null)
            {
                return BadRequest("Invalid password"); // 400 - סיסמה שגויה
            }

            // יצירת טוקן
            var token = await _service.Generate(user);
            return Ok(token);
        }
        [HttpPost("signUp")]
        public async Task<IActionResult> Post(User user)
        {
            var user1 = await _service.GetByEmail(user.Email.ToLower());
            if (user1==null)//משתמש לא קיים
            {
                await _service.Add(user);
                return Ok(user);
            }
            //יש מייל אבל אין סיסמה כלומר התחברו דרך גוגל
            else if(await _service.Authenticate(user.Email.ToLower(), user.Password) == null)//אם יש מייל אבל אין סיסמה אז צריך לעדכן
            {
                await _service.Update(user1.Id,user);
                return Ok(user);
            }
            return BadRequest("User already exists"); //singInבקלינט נשלח אחרי התחברות או אם כבר רשום ל
        }
        [HttpPost("googleAuthPage")]
        public async Task<IActionResult> Post([FromBody] GoogleTokenRequest request)
        {
            //נותן אופציה רק למי שכבר רשום
            try
            {
                //Google אימות הטוקן של 
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token);

                var user = await _service.GetByEmail(payload.Email.ToLower());
                if (user == null)
                {
                    user = new User
                    {
                        Email = payload.Email.ToLower(),
                        UserName = payload.Name,
                        Password = null,                           // כל מידע נוסף שתרצה לשמור
                    };
                    await _service.Add(user);
                }
                var token = await _service.Generate(user);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = "Invalid token", error = ex.Message });
            }
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
        public class GoogleTokenRequest
        {
            public string Token { get; set; }
        }
    }
}
