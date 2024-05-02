using Microsoft.EntityFrameworkCore;
using Pal.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Data.Contexts
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDbContext Context { get; set; }

        public RepositoryBase(ApplicationDbContext context)
        {
            Context = context;
        }

        //------------
        public IQueryable<T> FindAll()
        {
            var x = Context.Set<T>();
           return Context.Set<T>().AsNoTracking();
        }
        //------------
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            var typ = typeof(T);
            var x = Context.Set<T>().Where(expression);
            return Context.Set<T>().Where(expression).AsNoTracking();
        }
        //------------
        public void CreateAsync(T entity)
        {
           Context.Set<T>().Add(entity);
        }
        //------------
        public void UpdateAsync(T entity)
        {
            Context.Set<T>().Update(entity);
        }
        //------------
        public void DeleteAsync(T entity)
        {
            Context.Set<T>().Remove(entity);
        }
    }
}
