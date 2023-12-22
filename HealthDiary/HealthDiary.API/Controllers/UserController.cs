using HealthDiary.BusinessLogic.Models;
using HealthDiary.BusinessLogic.Services.Interfaces;
using System.Threading;
using System.Web.Http;

namespace HealthDiary.API.Controllers
{
    [RoutePrefix("api/UserAuth")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) => _userService = userService;

        [HttpPost]
        [Route(nameof(Login))]
        public IHttpActionResult Login([FromBody] UserDto userDto, CancellationToken token)
        {
            var result = _userService.LoginUser(userDto, token);
            return (IHttpActionResult)result;
        }
    }
}