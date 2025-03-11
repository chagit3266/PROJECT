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

        public Task<Route> Add(Route item)
        {
            throw new NotImplementedException();
        }

        public Task<Route> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Route> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Route> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Route> Update(Route item)
        {
            throw new NotImplementedException();
        }
    }
}
