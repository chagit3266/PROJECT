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
    public class WayService : IService<Way,string>
    {
        private readonly IRepository<Way, string> _repository;
        public WayService(IRepository<Way, string> _repository)
        {
            this._repository = _repository;
        }

        public async Task<Way> Add(Way item)
        {
            await _repository.Add(item);
            return item;
        }

        public async Task<Way> Delete(string id)
        {
            return await _repository.Delete(id);
        }

        public async Task<List<Way>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<Way> GetById(string id)
        {
            return await _repository.GetById(id);
        }

        public async Task<Way> Update(string id, Way item)
        {
            return await _repository.Update(id,item);
        }
    }
}
