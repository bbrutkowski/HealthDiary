using HealthDiary.API.Helpers.Interface;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HealthDiary.API.MediatR.Handlers.Activity.GetActivities;
using static HealthDiary.API.MediatR.Handlers.Activity.GetMonthlyActivity;
using static HealthDiary.API.MediatR.Handlers.Activity.GetYearlyActivity;
using static HealthDiary.API.MediatR.Handlers.Activity.SaveActivity;

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

        [HttpGet("get-activities")]
        public async Task<IActionResult> GetActivities(int id, CancellationToken token)
        {
            var verificationResult = _identityVerifier.IsIdentityConfirmed(id);
            if (verificationResult.IsFailure) return Forbid();

            var result = await _mediator.Send(new GetActivitiesRequest(id), token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpPost("save-activity")]
        public async Task<IActionResult> SaveActivity([FromBody] SaveActivityRequest request, CancellationToken token)
        {
            var verificationResult = _identityVerifier.IsIdentityConfirmed(request.Id);
            if (verificationResult.IsFailure) return Forbid();

            var result = await _mediator.Send(request, token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("get-weekly-activity")]
        public async Task<IActionResult> GetWeeklyActivity(int id, CancellationToken token)
        {
            var verificationResult = _identityVerifier.IsIdentityConfirmed(id);
            if (verificationResult.IsFailure) return Forbid();

            var result = await _mediator.Send(new GetWeeklyActivityRequest(id), token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}
