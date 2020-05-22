using System;
using System.Threading.Tasks;

namespace Payroll.Data.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        void Save();
        Task<int> SaveAsync();
    }
}
