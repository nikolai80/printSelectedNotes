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
        bool Remove(Guid itemId);

        bool Update(Guid itemId, string text);
    }
}
