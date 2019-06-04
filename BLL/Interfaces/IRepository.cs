using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintSelected.BLL
{
   public interface IRepository<T> where T : class
    {
        T GetById(Guid id);
        List<T> GetAll();
        bool Create(string text);
        void Remove(List<Guid> listId);

        bool Update(Guid Id, string text);
    }
}
