using MediatR;
using Microsoft.AspNetCore.Mvc;
using static HealthDiary.API.MediatR.Handlers.Weather.GetWeather;

namespace HealthDiary.API.Controllers
{
    [Route("api/weather")]
    [ApiController]
    public class WeatherInfoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeatherInfoController(IMediator mediator) => _mediator = mediator;

        [HttpGet("getWeather")]
        public async Task<IActionResult> GetWeather(GetWeatherRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(request, token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}
