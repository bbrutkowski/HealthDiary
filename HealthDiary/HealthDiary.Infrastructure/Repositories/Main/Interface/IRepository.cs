using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HealthDiary.Infrastructure
{
    public interface IRepository<T>
    {
        T Add(T entity);
        T Get(int id);
        List<T> Find(Expression<Func<T, bool>> predicate);
        void SaveChangesAsync();
        Task<T> AddAsync(T entity);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(int id);
        void SaveChanges();
    }
}
