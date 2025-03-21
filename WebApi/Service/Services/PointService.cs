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
    public class PointService : IService<Node>
    {
        private readonly IRepository<Node> _repository;
        public PointService(IRepository<Node> _repository)
        {
            this._repository = _repository;
        }
        public async Task<Node> Add(Node item)
        {
            await _repository.Add(item);
            return item;
        }

        public async Task<Node> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<List<Node>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Node> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<Node> Update(int id, Node item)
        {
            return await _repository.Update(id,item);
        }
    }
}
