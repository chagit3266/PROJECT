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
    public class UserRoutesService : IService<UserWay,int>
    {
        private readonly IRepository<UserWay, int> _repository;
        public UserRoutesService(IRepository<UserWay, int> _repository)
        {
            this._repository = _repository;
        }
        public async Task<UserWay> Add(UserWay item)
        {
            await _repository.Add(item);
            return item;
        }

        public async Task<UserWay> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<List<UserWay>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<UserWay> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public async Task<UserWay> Update(int id,UserWay item)
        {
            return await _repository.Update(id, item);
        }
    }
}
