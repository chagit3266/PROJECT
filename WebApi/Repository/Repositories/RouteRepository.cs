using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class RouteRepository : IRepository<Route>
    {
        private readonly IContext _context;
        public RouteRepository(IContext _context)
        {
            this._context = _context;
        }

        public async Task<Route> Add(Route item)
        {
            await _context.Route.AddAsync(item);
            _context.Save();
            return item;
        }


        public Task<Route> GetAll()
        {
            
        }

        public Task<Route> GetById(int id)
        {
            
        }
        public Task Delete(int id)
        {
           
        }
        public Task<Route> Update(Route item)
        {
            
        }
    }
}
