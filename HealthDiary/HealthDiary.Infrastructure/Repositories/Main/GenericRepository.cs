using HealthDiary.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace HealthDiary.Infrastructure
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly DataContext _dataContext;

        public GenericRepository(DataContext dataContext) => _dataContext = dataContext;

        public virtual T Add(T entity) => _dataContext.Add(entity).Entity;

        public virtual async Task<T> AddAsync(T entity)
        {
            var result = await _dataContext.AddAsync(entity);
            return result.Entity;
        }

        public virtual async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate) => await _dataContext.Set<T>().AsQueryable().Where(predicate).ToListAsync();

        public virtual T Get(int id) => _dataContext.Find<T>(id);

        public virtual async Task<T> GetAsync(int id) => await _dataContext.FindAsync<T>(id);

        public virtual async void SaveChangesAsync() => await _dataContext.SaveChangesAsync();

        public virtual void SaveChanges() => _dataContext.SaveChanges();

        public virtual List<T> Find(Expression<Func<T, bool>> predicate) => _dataContext.Set<T>().AsQueryable().Where(predicate).ToList();
    }
}
