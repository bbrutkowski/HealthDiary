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

        [HttpGet]
        [Route(nameof(GetLastMealInformationByUserId))]
        public async Task<IActionResult> GetLastMealInformationByUserId(int id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetLastMealInformationByUserIdRequest(id), token);
            if (result.IsFailure) return BadRequest(result);
            return Ok(result);
        }

        [HttpGet]
        [Route(nameof(GetMealNutritionFromWeekByUserId))]
        public async Task<IActionResult> GetMealNutritionFromWeekByUserId(int id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetMealNutritionFromWeekByUserIdRequest(id), token);
            if (result.IsFailure) return BadRequest(result);
            return Ok(result);
        }
    }
}
