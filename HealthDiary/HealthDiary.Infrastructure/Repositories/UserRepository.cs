using HealthDiary.BusinessLogic.Models.Main;
using HealthDiary.Database;

namespace HealthDiary.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(DataContext dataContext): base(dataContext) { }
    
    }
}
