using HealthDiary.BusinessLogic.Models.Main;

namespace HealthDiary.Infrastructure.UnitOfWork.Interface
{
    public interface IUnitOfWork
    {
        IRepository<User> UserRepository { get; }

        void SaveChanges();
    }
}
