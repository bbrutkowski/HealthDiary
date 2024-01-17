using MediatR;
using Microsoft.AspNetCore.Mvc;
using static HealthDiary.API.MediatR.Handlers.User.GetUser;
using static HealthDiary.API.MediatR.Handlers.User.RegisterUser;
using static HealthDiary.API.MediatR.Handlers.User.UpdateUser;

namespace HealthDiary.API.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [Route(nameof(Register))]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(request, token);
            if (result.IsFailure) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet]
        [Route(nameof(GetUserById))]
        public async Task<IActionResult> GetUserById(int id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetUserRequest(id), token);
            if (result.IsFailure) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost]
        [Route(nameof(Update))]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(request, token);
            if (result.IsFailure) return BadRequest(result);
            return Ok(result);
        }
    }
}
