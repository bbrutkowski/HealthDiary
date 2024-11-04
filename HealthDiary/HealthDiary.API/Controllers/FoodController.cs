using HealthDiary.API.Helpers.Interface;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HealthDiary.API.MediatR.Handlers.Food.GetMealNutritionFromWeek;

namespace HealthDiary.API.Controllers
{
    [Authorize]
    [Route("api/food")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityVerifier _identityVerifier;

        public FoodController(IMediator mediator, IIdentityVerifier identityVerifier)
        {
            _mediator = mediator;
            _identityVerifier = identityVerifier;
        }

        [HttpGet("get-nutrition-info")]
        public async Task<IActionResult> GetMealNutritionByUserId(int id, CancellationToken token)
        {
            var verificationResult = _identityVerifier.IsIdentityConfirmed(id);
            if (verificationResult.IsFailure) return Forbid();

            var result = await _mediator.Send(new GetNutritionInfoRequest(id), token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}
