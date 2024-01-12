using HealthDiary.API.MediatR.Handlers.Weather;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HealthDiary.API.Controllers
{
    [Route("api/WeatherInfo")]
    [ApiController]
    public class WeatherInfoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeatherInfoController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [Route(nameof(GetWeather))]
        public async Task<IActionResult> GetWeather(CancellationToken token)
        {
            var result = await _mediator.Send(new GetWeatherRequest(), token);
            if (result.IsFailure) return BadRequest(result);
            return Ok(result);
        }
    }
}
