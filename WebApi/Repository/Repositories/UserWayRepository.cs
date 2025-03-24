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
    internal class UserWayRepository : IRepository<UserWay>
    {
        private readonly IContext _context;
        public UserWayRepository(IContext _context)
        {
            this._context = _context;
        }
        public async Task<UserWay> Add(UserWay item)
        {
            await _context.UsersWays.AddAsync(item);
            _context.Save();
            return item;
        }

        public async Task<UserWay> Delete(int id)
        {
            var userRoutes = await GetById(id);
            _context.UsersWays.Remove(userRoutes);
            _context.Save();
            return userRoutes;
        }

        public async Task<List<UserWay>> GetAll()
        {
            return await _context.UsersWays.ToListAsync();
        }

        public async Task<UserWay> GetById(int id)
        {
            return await _context.UsersWays.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UserWay> Update(int id,UserWay item)
        {
            var userWays = await GetById(id);
            //אין מה לעדכן
            return userWays;
        }
    }
}
