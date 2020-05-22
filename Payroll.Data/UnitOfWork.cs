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
            // Checks if the Dictionary Key contains the Model class
            if (_repositories.Keys.Contains(typeof(TEntity)))
            {
                // Return the repository for that Model class
                return _repositories[typeof(TEntity)] as IRepository<TEntity>;
            }

            // If the repository for that Model class doesn't exist, create it
            var repository = new Repository<TEntity>(Context);

            // Add it to the dictionary
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
