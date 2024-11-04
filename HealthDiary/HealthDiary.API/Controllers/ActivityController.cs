using HealthDiary.API.Helpers.Interface;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HealthDiary.API.MediatR.Handlers.Activity.GetMonthlyActivity;

namespace HealthDiary.API.Controllers
{
    [Authorize]
    [Route("api/activity")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityVerifier _identityVerifier;

        public ActivityController(IMediator mediator, IIdentityVerifier identityVerifier)
        {
            _mediator = mediator;
            _identityVerifier = identityVerifier;
        }

        [HttpGet("get-activity")]
        public async Task<IActionResult> GetMonthlyActivityByUserId(int id, CancellationToken token)
        {
            var verificationResult = _identityVerifier.IsIdentityConfirmed(id);
            if (verificationResult.IsFailure) return Forbid();

            var result = await _mediator.Send(new GetActivityRequest(id), token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}
