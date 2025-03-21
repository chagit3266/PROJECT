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
    public class UserRoutesService : IService<UserRoutes>
    {
        private readonly IRepository<UserRoutes> _repository;
        public UserRoutesService(IRepository<UserRoutes> _repository)
        {
            this._repository = _repository;
        }
        public async Task<UserRoutes> Add(UserRoutes item)
        {
            await _repository.Add(item);
            return item;
        }

        public async Task<UserRoutes> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<List<UserRoutes>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<UserRoutes> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<UserRoutes> Update(int id,UserRoutes item)
        {
            return await _repository.Update(id, item);
        }
    }
}
