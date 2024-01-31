using MediatR;
using Microsoft.AspNetCore.Mvc;
using static HealthDiary.API.MediatR.Handlers.Sleep.GetLastSleepInfo;

namespace HealthDiary.API.Controllers
{
    [Route("api/sleep")]
    [ApiController]
    public class SleepController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SleepController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [Route(nameof(GetLastSleepInformationByUserId))]
        public async Task<IActionResult> GetLastSleepInformationByUserId(int id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetLastSleepInfoByUserIdRequest(id), token);
            if (result.IsFailure) return BadRequest(result);
            return Ok(result);
        }
    }
}
