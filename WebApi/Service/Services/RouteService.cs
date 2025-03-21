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
    public class RouteService : IService<Way>
    {
        private readonly IRepository<Way> _repository;
        public RouteService(IRepository<Way> _repository)
        {
            this._repository = _repository;
        }

        public async Task<Way> Add(Way item)
        {
            await _repository.Add(item);
            return item;
        }

        public async Task<Way> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<List<Way>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Way> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<Way> Update(int id, Way item)
        {
            return await _repository.Update(id,item);
        }
    }
}
