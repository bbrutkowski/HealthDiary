using HealthDiary.API.Helpers.Interface;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HealthDiary.API.MediatR.Handlers.User.GetUser;
using static HealthDiary.API.MediatR.Handlers.User.RegisterUser;
using static HealthDiary.API.MediatR.Handlers.User.UpdateUser;

namespace HealthDiary.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityVerifier _identityVerifier;

        public UserController(IMediator mediator, IIdentityVerifier identityVerifier)
        {
            _mediator = mediator;
            _identityVerifier = identityVerifier;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(request, token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [Authorize]
        [HttpGet("getUserInfo")]
        public async Task<IActionResult> GetUserById(int id, CancellationToken token)
        {
            var verificationResult = _identityVerifier.IsIdentityConfirmed(id);
            if (verificationResult.IsFailure) return Forbid();

            var result = await _mediator.Send(new GetUserRequest(id), token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request, CancellationToken token)
        {
            var verificationResult = _identityVerifier.IsIdentityConfirmed(request.Id);
            if (verificationResult.IsFailure) return Forbid();

            var result = await _mediator.Send(request, token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}
