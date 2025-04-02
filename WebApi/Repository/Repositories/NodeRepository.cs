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
    public class NodeRepository : IRepository<Node,long>
    {
        private readonly IContext _context;
        public NodeRepository(IContext _context)
        {
            this._context = _context;
        }

        public async Task<Node> Add(Node item)
        {
            await _context.Nodes.AddAsync(item);
            _context.Save();
            return item;
        }

        public async Task<Node> Delete(long id)
        {
            var node = await GetById(id);
            _context.Nodes.Remove(node);
            _context.Save();
            return node;
        }

        public async Task<List<Node>> GetAll()
        {
            return await _context.Nodes.ToListAsync();
        }

        public async Task<Node> GetById(long id)
        {
            return await _context.Nodes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Node> Update(long id,Node item)
        {
            var node= await GetById(id);
            node.Lat = item.Lat;
            node.Lon = item.Lon;
            return node;
        }
    }
}
