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
    internal class UserRoutesRepository : IRepository<UserRoutes>
    {
        private readonly IContext _context;
        public UserRoutesRepository(IContext _context)
        {
            this._context = _context;
        }
        public async Task<UserRoutes> Add(UserRoutes item)
        {
            await _context.UsersRoutes.AddAsync(item);
            _context.Save();
            return item;
        }

        public async Task<UserRoutes> Delete(int id)
        {
            var userRoutes = await GetById(id);
            _context.UsersRoutes.Remove(userRoutes);
            _context.Save();
            return userRoutes;
        }

        public async Task<List<UserRoutes>> GetAll()
        {
            return await _context.UsersRoutes.ToListAsync();
        }

        public async Task<UserRoutes> GetById(int id)
        {
            return await _context.UsersRoutes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UserRoutes> Update(int id,UserRoutes item)
        {
            var userRoutes = await GetById(id);
            return userRoutes;
        }
    }
}
