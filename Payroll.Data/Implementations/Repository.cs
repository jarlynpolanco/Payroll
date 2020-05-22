using Microsoft.EntityFrameworkCore;
using Payroll.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Payroll.Data.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IDbContext _context;
        private readonly DbSet<T> _dbset;

        public Repository(IDbContext context)
        {
            _context = context;
            _dbset = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll(Func<T, bool> predicate = null)
        {
            if (predicate != null)
                return _dbset.Where(predicate);

            return _dbset;
        }

        public virtual IEnumerable<T> GetAllWithInclude(string include) => _dbset.Include(include);

        public virtual T Get(Func<T, bool> predicate) => _dbset.FirstOrDefault(predicate);

        public virtual T Get(Func<T, bool> predicate, string include) =>
            _dbset.Include(include).FirstOrDefault(predicate);

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbset.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual void Add(T entity) => _dbset.Add(entity);

        public virtual void Delete(T entity)
        {
            var entry = _context.Entry(entity);
            entry.State = EntityState.Deleted;
            _dbset.Remove(entity);
        }

        public virtual void DeleteAll(IEnumerable<T> entity)
        {
            foreach (var ent in entity)
            {
                var entry = _context.Entry(ent);
                entry.State = EntityState.Deleted;
                _dbset.Remove(ent);
            }
        }

        public virtual void Update(T entity)
        {
            var entry = _context.Entry(entity);
            _dbset.Attach(entity);
            entry.State = EntityState.Modified;
        }

        public virtual bool Any() => _dbset.Any();

        public long Count(Func<T, bool> predicate = null) => _dbset.Count(predicate);
    }
}
