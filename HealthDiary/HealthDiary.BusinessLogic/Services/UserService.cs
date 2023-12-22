using HealthDiary.BusinessLogic.Models;
using HealthDiary.BusinessLogic.Services.Interfaces;
using HealthDiary.Database.Model.Main;
using HealthDiary.Repository.Interfaces;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UserDto = HealthDiary.BusinessLogic.Models.UserDto;

namespace HealthDiary.BusinessLogic
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> LoginUser(UserDto userDto, CancellationToken token)
        {
            var isExists = await _unitOfWork.Repository<User>()
                                            .GetQueryable(x => x.Name == userDto.UserName && x.Password == userDto.Password)
                                            .Where(x => x.BasicEntity.IsActive)
                                            .AnyAsync(token);

            return isExists;
        }
    }
}
