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

        public SleepController(IMediator mediator) => _mediator = mediator;

        [HttpGet("getSleepInfo")]
        public async Task<IActionResult> GetSleepInfoByUserId(int id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetSleepInfoRequest(id), token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}
