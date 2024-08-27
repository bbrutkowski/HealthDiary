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
        public async Task<IActionResult> GetWeather(CancellationToken token)
        {
            var result = await _mediator.Send(new GetWeatherRequest(), token);
            if (result.IsFailure) return BadRequest(result);
            return Ok(result);
        }
    }
}
