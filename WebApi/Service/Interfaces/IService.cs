using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IService<T>
    {
        Task<T> Add(T item);
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Delete(int id);
        Task<T> Update(int id, T item);
    }
}
