
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static HealthDiary.API.MediatR.Handlers.Auth.LoginUser;

namespace HealthDiary.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator) => _mediator = mediator;

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(request, token);
            if (result.IsFailure) return BadRequest(result);
            return Ok(result);
        }
    }
}