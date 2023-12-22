using System.Threading.Tasks;
using System.Threading;
using HealthDiary.BusinessLogic.Models;

namespace HealthDiary.BusinessLogic.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> LoginUser(UserDto userDto, CancellationToken token);
    }
}
