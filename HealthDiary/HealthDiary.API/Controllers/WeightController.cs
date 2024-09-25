using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HealthDiary.API.MediatR.Handlers.Weight.GetWeightGoal;
using static HealthDiary.API.MediatR.Handlers.Weight.GetWeightsByMonth;
using static HealthDiary.API.MediatR.Handlers.Weight.GetYearlyWeightById;

namespace HealthDiary.API.Controllers
{
    [Authorize]
    [Route("api/weight")]
    [ApiController]
    public class WeightController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeightController(IMediator mediator) => _mediator = mediator;

        [HttpGet("getWeightsByMonth")]
        public async Task<IActionResult> GetWeightsByMonth(int Id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetWeightsByMonthRequest(Id), token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }

        [HttpGet("getYearlyWeightById")]
        [Route(nameof(GetYearlyWeightById))]
        public async Task<IActionResult> GetYearlyWeightById(int id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetYearlyWeightByIdRequest(id), token);
            if (result.IsFailure) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("getWeightGoal")]
        public async Task<IActionResult> GetWeightGoalByUserId(int Id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetWeightGoalRequest(Id), token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}
