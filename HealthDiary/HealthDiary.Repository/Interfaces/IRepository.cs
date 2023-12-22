using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq.Expressions;
using System;
using System.Linq;

namespace HealthDiary.Repository.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token);
        Task<TEntity> GetByIdAsync(int id);
        bool Any(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> expression);
    }
}
