using HealthDiary.BusinessLogic.Models.Main;
using HealthDiary.Database;
using HealthDiary.Infrastructure.Repositories;
using HealthDiary.Infrastructure.UnitOfWork.Interface;

namespace HealthDiary.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _dataContext;
        private IRepository<User> _userRepository;

        public UnitOfWork(DataContext dataContext) => _dataContext = dataContext;

        public IRepository<User> UserRepository
        { 
            get 
            {
                if (_userRepository == null) _userRepository = new UserRepository(_dataContext);
                return _userRepository;
            }        
        }

        public void SaveChanges() => _dataContext.SaveChanges();
    }
}
