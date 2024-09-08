using MediatR;
using Microsoft.AspNetCore.Mvc;
using static HealthDiary.API.MediatR.Handlers.Localization.GetCity;

namespace HealthDiary.API.Controllers
{
    public class LocalizationController
    {
        [Route("api/localization")]
        [ApiController]
        public class AuthController : ControllerBase
        {
            private readonly IMediator _mediator;

            public AuthController(IMediator mediator) => _mediator = mediator;

            [HttpPost("getCity")]
            public async Task<IActionResult> GetCity([FromBody] GetCityRequest request, CancellationToken token)
            {
                var result = await _mediator.Send(request, token);
                if (result.IsFailure) return BadRequest(result.Error);
                return Ok(result.Value);
            }
        }
    }
}
