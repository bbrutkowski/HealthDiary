using HealthDiary.API.Helpers.Interface;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HealthDiary.API.MediatR.Handlers.Sleep.GetLastSleepInfo;

namespace HealthDiary.API.Controllers
{
    [Authorize]
    [Route("api/sleep")]
    [ApiController]
    public class SleepController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityVerifier _identityVerifier;

        public SleepController(IMediator mediator, IIdentityVerifier identityVerifier)
        {
            _mediator = mediator;
            _identityVerifier = identityVerifier;
        }

        [HttpGet("getSleepInfo")]
        public async Task<IActionResult> GetSleepInfoByUserId(int id, CancellationToken token)
        {
            var verificationResult = _identityVerifier.IsUserVerified(id);
            if (verificationResult.IsFailure) return Forbid();

            var result = await _mediator.Send(new GetSleepInfoRequest(id), token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}
