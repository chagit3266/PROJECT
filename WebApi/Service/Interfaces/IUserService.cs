using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IUserService:IService<User,int>
    {
        Task<string> Generate(User user);
        Task<User> Authenticate(string email, string password);
        Task<User> GetByEmailAndPassword(string email, string password);
        Task<User> GetByEmail(string email);
    }
}
