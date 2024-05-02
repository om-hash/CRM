using Pal.Core.Enums;
using Pal.Core.Enums.Attachment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Domains
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void CreateAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
    }


  
}
 