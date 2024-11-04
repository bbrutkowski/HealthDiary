using HealthDiary.API.Helpers.Interface;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HealthDiary.API.MediatR.Handlers.Weight.AddWeight;
using static HealthDiary.API.MediatR.Handlers.Weight.GetBMI;
using static HealthDiary.API.MediatR.Handlers.Weight.GetWeightGoal;
using static HealthDiary.API.MediatR.Handlers.Weight.GetWeightGoalProgress;
using static HealthDiary.API.MediatR.Handlers.Weight.GetWeightsByMonth;
using static HealthDiary.API.MediatR.Handlers.Weight.GetYearlyWeight;
using static HealthDiary.API.MediatR.Handlers.Weight.SaveBMI;
using static HealthDiary.API.MediatR.Handlers.Weight.SaveWeightGoal;

namespace HealthDiary.API.Controllers
{
    [Authorize]
    [Route("api/weight")]
    [ApiController]
    public class WeightController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityVerifier _identityVerifier;

        public WeightController(IMediator mediator, IIdentityVerifier identityVerifier)
        {
            _mediator = mediator;
            _identityVerifier = identityVerifier;
        }

        [HttpGet("get-weights-by-month")]
        public async Task<IActionResult> GetWeightsByMonth(int id, CancellationToken token)
        {
            var verificationResult = _identityVerifier.IsIdentityConfirmed(id);
            if (verificationResult.IsFailure) return Forbid();

            var result = await _mediator.Send(new GetWeightsByMonthRequest(id), token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("get-yearly-weight")]
        public async Task<IActionResult> GetYearlyWeight(int id, CancellationToken token)
        {
            var verificationResult = _identityVerifier.IsIdentityConfirmed(id);
            if (verificationResult.IsFailure) return Forbid();

            var result = await _mediator.Send(new GetYearlyWeightRequest(id), token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("get-weight-goal")]
        public async Task<IActionResult> GetWeightGoal(int id, CancellationToken token)
        {
            var verificationResult = _identityVerifier.IsIdentityConfirmed(id);
            if (verificationResult.IsFailure) return Forbid();

            var result = await _mediator.Send(new GetWeightGoalRequest(id), token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpPost("save-weight-goal")]
        public async Task<IActionResult> SaveWeightGoal([FromBody] SaveWeightGoalRequest request, CancellationToken token)
        {
            var verificationResult = _identityVerifier.IsIdentityConfirmed(request.UserId);
            if (verificationResult.IsFailure) return Forbid();

            var result = await _mediator.Send(request, token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("get-bmi")]
        public async Task<IActionResult> GetBMI(int id, CancellationToken token)
        {
            var verificationResult = _identityVerifier.IsIdentityConfirmed(id);
            if (verificationResult.IsFailure) return Forbid();

            var result = await _mediator.Send(new GetBmiRequest(id), token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpPost("save-bmi")]
        public async Task<IActionResult> SaveBMI([FromBody] SaveBmiRequest request, CancellationToken token)
        {
            var verificationResult = _identityVerifier.IsIdentityConfirmed(request.UserId);
            if (verificationResult.IsFailure) return Forbid();

            var result = await _mediator.Send(request, token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("get-weight-goal-progress")]
        public async Task<IActionResult> GetWeightGoalProgress(int id, CancellationToken token)
        {
            var verificationResult = _identityVerifier.IsIdentityConfirmed(id);
            if (verificationResult.IsFailure) return Forbid();

            var result = await _mediator.Send(new GetWeightGoalProgressRequest(id), token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpPost("add-weight")]
        public async Task<IActionResult> AddWeight([FromBody] AddWeightRequest request, CancellationToken token)
        {
            var verificationResult = _identityVerifier.IsIdentityConfirmed(request.Id);
            if (verificationResult.IsFailure) return Forbid();

            var result = await _mediator.Send(request, token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}
