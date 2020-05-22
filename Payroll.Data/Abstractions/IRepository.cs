using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Payroll.Data.Abstractions
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Func<T, bool> predicate = null);
        IEnumerable<T> GetAllWithInclude(string include);
        long Count(Func<T, bool> predicate = null);
        T Get(Func<T, bool> predicate);
        T Get(Func<T, bool> predicate, string include);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
