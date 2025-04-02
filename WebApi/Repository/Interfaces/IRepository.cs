using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IRepository<T,W>
    {
       //כאן יופיעו הפעולות שרלונטיות לכל המחלקות
       Task<T> Add(T item);
       Task<List<T>> GetAll();
       Task<T> GetById(W id);
       Task<T> Delete(W id);
       Task<T> Update(W id,T item);
    }
}
