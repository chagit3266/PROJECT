using Repository.Entities;
using Repository.Interfaces;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class NodeService : IService<Node,long>
    {
        private readonly IRepository<Node,long> _repository;
        public NodeService(IRepository<Node,long> _repository)
        {
            this._repository = _repository;
        }
        public async Task<Node> Add(Node item)
        {
            await _repository.Add(item);
            return item;
        }

        public async Task<Node> Delete(long id)
        {
            return await _repository.Delete(id);
        }

        public async Task<List<Node>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Node> GetById(long id)
        {
            return await _repository.GetById(id);
        }

        public async Task<Node> Update(long id, Node item)
        {
            return await _repository.Update(id,item);
        }
    }
}
