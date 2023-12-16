using HealthDiary.Database;
using HealthDiary.Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace HealthDiary.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dbContext;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(DataContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _repositories = new Dictionary<Type, object>();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return _repositories[typeof(TEntity)] as IRepository<TEntity>;
            }

            var repository = new Repository<TEntity>(_dbContext);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public async void SaveChangesAsync()
        {
           await _dbContext.SaveChangesAsync();
        }
    }
}
