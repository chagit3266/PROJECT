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
    public class WayRepository : IRepository<Way,string>
    {
        private readonly IContext _context;
        public WayRepository(IContext _context)
        {
            this._context = _context;
        }

        public async Task<Way> Add(Way item)
        {
            await _context.Ways.AddAsync(item);
            _context.Save();
            return item;
        }

        public async Task<Way> Delete(string id)
        {
            var ways = await GetById(id);
            _context.Ways.Remove(ways);
            _context.Save();
            return ways;
        }

        public async Task<List<Way>> GetAll()
        {
            return await _context.Ways.ToListAsync();
        }

        public async Task<Way> GetById(string id)
        {
            return await _context.Ways.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Way> Update(string id,Way item)
        {
            //Point לא צריך לעדכן ישירות נקודות אלא לשלוח לעדכון במחלקת 
            var ways= await GetById(id);
            //update nodes in way
            return ways;
        }
    }
}
