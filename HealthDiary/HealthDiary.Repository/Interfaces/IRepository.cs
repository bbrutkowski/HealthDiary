using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

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
    }
}
