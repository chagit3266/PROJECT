using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IService<T,W>
    {
        Task<T> Add(T item);
        Task<List<T>> GetAll();
        Task<T> GetById(W id);
        Task<T> Delete(W id);
        Task<T> Update(W id, T item);
    }
}
