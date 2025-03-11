using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly IContext _context;
        public UserRepository(IContext _context)
        {
            this._context = _context;
        }

        public Task<User> Add(User item)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> Update(User item)
        {
            throw new NotImplementedException();
        }

        Task<User> IRepository<User>.Delete(int id)
        {
            throw new NotImplementedException();
        }

        Task<User> IRepository<User>.GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
