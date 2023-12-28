using System.Threading.Tasks;
using System.Threading;
using HealthDiary.BusinessLogic.Models;
using HealthDiary.Database.Model.Main;

namespace HealthDiary.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> LoginUser(UserDto userDto, CancellationToken token);
        Task<User> Find(string userName, string password);
    }
}
