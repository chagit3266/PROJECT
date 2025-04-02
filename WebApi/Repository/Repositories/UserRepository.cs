using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class UserRepository : IRepository<User,int>
    {
        private readonly IContext _context;
        public UserRepository(IContext _context)
        {
            this._context = _context;
        }

        public async Task<User> Add(User item)
        {
            await _context.Users.AddAsync(item);
            _context.Save();
            return item;
        }

        public async Task<User> Delete(int id)
        {
            var user = await GetById(id);
            _context.Users.Remove(user);
            _context.Save();
            return user;
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> Update(int id,User item)
        {
            var user =await GetById(id);
            user.Email = item.Email;
            user.Password = item.Password;
            user.UserName = item.UserName;
            return user;
        }

    }
}
