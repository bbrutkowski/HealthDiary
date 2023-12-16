using System;

namespace HealthDiary.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        void SaveChanges();
        void SaveChangesAsync();
    }
}
