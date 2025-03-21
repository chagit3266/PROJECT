using Microsoft.IdentityModel.Tokens;
using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;


namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly IRepository<User> _repository;
        public UserService(IRepository<User> _repository,IConfiguration _config)
        {
            this._repository = _repository;
            this._config = _config;
        }
        public async Task<User> Add(User item)
        {
            await _repository.Add(item);
            return item;
        }

        public async Task<User> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<List<User>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<User> GetById(int id)
        {
            return await _repository.GetById(id);
        }
        public async Task<User> GetByEmailAndPassword(string email, string password)
        {
            var users = await GetAll();
            return users.FirstOrDefault(x => ( x.Password==password && x.Email==email));
        }
        public async Task<User> Update(int id, User item)
        {
            return await _repository.Update(id,item);
        }
        //Token-פונקציה שיוצרת טוקן 
        public async Task<string> Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials=new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            //אוביקט שמגדירים בו מה מצפינים
            var claims = new[] {
               new Claim(ClaimTypes.NameIdentifier, user.UserName),
               new Claim(ClaimTypes.Email, user.Email)
               //,new Claim(ClaimTypes.Password,user.Password)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"],
              claims,
              expires: DateTime.UtcNow.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        //פונקציה שמחזירה את המשתמש עם המייל והסיסמה הנשלחים
        public async Task<User> Authenticate(string email, string password)
        {
            var user =await GetByEmailAndPassword(email,password);
            if (user != null)
                return user;
            return null; 

        }
    }
}
