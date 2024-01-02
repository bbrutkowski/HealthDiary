using HealthDiary.API.Model;
using System.Threading;
using System.Web.Http;

namespace HealthDiary.API.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        [HttpPost]
        [Route(nameof(Login))]
        public void Login([FromBody] UserDto userDto, CancellationToken token)
        {         
            Ok();
        }

        [HttpPost]
        [Route(nameof(Register))]
        public void Register([FromBody] UserDto user, CancellationToken token)
        {
            Ok();
        }
    }
}
