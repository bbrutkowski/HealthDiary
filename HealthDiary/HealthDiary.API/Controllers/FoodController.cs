using MediatR;
using Microsoft.AspNetCore.Mvc;
using static HealthDiary.API.MediatR.Handlers.Food.GetLastMealInformation;
using static HealthDiary.API.MediatR.Handlers.Food.GetMealNutritionFromWeek;

namespace HealthDiary.API.Controllers
{
    [Route("api/food")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FoodController(IMediator mediator) => _mediator = mediator;

        [HttpGet("getMealInfo")]
        public async Task<IActionResult> GetLastMealByUserId(int id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetMealInfoRequest(id), token);
            if (result.IsFailure) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet("getNutritionInfo")]
        public async Task<IActionResult> GetMealNutritionByUserId(int id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetNutritionInfoRequest(id), token);
            if (result.IsFailure) return BadRequest(result);
            return Ok(result);
        }
    }
}
