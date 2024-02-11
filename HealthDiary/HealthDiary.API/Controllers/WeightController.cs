using MediatR;
using Microsoft.AspNetCore.Mvc;
using static HealthDiary.API.MediatR.Handlers.Weight.GetWeights;
using static HealthDiary.API.MediatR.Handlers.Weight.GetWeightsByMonth;
using static HealthDiary.API.MediatR.Handlers.Weight.GetYearlyWeightById;

namespace HealthDiary.API.Controllers
{
    [Route("api/weight")]
    [ApiController]
    public class WeightController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeightController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [Route(nameof(GetWeightsByUserId))]
        public async Task<IActionResult> GetWeightsByUserId(GetWeightsRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(request, token);
            if (result.IsFailure) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet]
        [Route(nameof(GetWeightsByMonth))]
        public async Task<IActionResult> GetWeightsByMonth(int Id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetWeightsByMonthRequest(Id), token);
            if (result.IsFailure) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet]
        [Route(nameof(GetYearlyWeightById))]
        public async Task<IActionResult> GetYearlyWeightById(int id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetYearlyWeightByIdRequest(id), token);
            if (result.IsFailure) return BadRequest(result);
            return Ok(result);
        }
    }
}
