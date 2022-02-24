using System;
using System.Linq;
using System.Linq.Expressions;

namespace ECom.Contracts.Data.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        T Get(object id);
        void Add(T entity);
        void Update(T entity);
        void Delete(object id);
        int Count();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition);
    }
}