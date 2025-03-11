using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class PointRepository : IRepository<Point>
    {
        private readonly IContext _context;
        public PointRepository(IContext _context)
        {
            this._context = _context;
        }

        public Task<Point> Add(Point item)
        {
            
        }

        public Task<Point> Delete(int id)
        {
            
        }

        public Task<Point> GetAll()
        {
            
        }

        public Task<Point> GetById(int id)
        {
            
        }

        public Task<Point> Update(Point item)
        {
            
        }
    }
}
