using Payroll.Data.Abstractions;
using Payroll.Data.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payroll.Data
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : IDbContext, new()
    {
        private readonly Dictionary<Type, object> _repositories;
        private bool _disposed;
        private readonly TContext Context;

        public UnitOfWork()
        {
            this.Context = new TContext();
            _repositories = new Dictionary<Type, object>();
            _disposed = false;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories.Keys.Contains(typeof(TEntity)))
                return _repositories[typeof(TEntity)] as IRepository<TEntity>;

            var repository = new Repository<TEntity>(Context);
            _repositories.Add(typeof(TEntity), repository);

            return repository;
        }

        public IDbContext GetContext { get { return this.Context; } }

        public void Save() => Context.SaveChanges();

        public Task<int> SaveAsync() => Context.SaveChangesAsync();

        protected virtual void Dispose(bool disposing)
        {
            if (this._disposed) return;

            if (disposing)
                Context.Dispose();

            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
