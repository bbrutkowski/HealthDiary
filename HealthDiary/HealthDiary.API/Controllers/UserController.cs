using HealthDiary.BusinessLogic.Models;
using HealthDiary.BusinessLogic.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace HealthDiary.API.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<IHttpActionResult> Login([FromBody] UserDto userDto, CancellationToken token)
        {
            return Ok(await _userService.LoginUser(userDto, token));
        }
    }
}