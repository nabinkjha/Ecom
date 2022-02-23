using System;
using System.Collections.Generic;

namespace ECom.Contracts.Data.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(object id);
        void Add(T entity);
        void Update(T entity);
        void Delete(object id);
        int Count();
        IEnumerable<T> Where(Func<T, bool> condition);
    }
}