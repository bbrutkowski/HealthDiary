using MediatR;
using Microsoft.AspNetCore.Mvc;
using static HealthDiary.API.MediatR.Handlers.Weight.GetWeights;

namespace HealthDiary.API.Controllers
{
    [Route("api/Weight")]
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
    }
}
