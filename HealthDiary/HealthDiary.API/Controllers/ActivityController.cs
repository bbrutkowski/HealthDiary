﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using static HealthDiary.API.MediatR.Handlers.Activity.GetMonthlyActivity;

namespace HealthDiary.API.Controllers
{
    [Route("api/activity")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ActivityController(IMediator mediator) => _mediator = mediator;

        [HttpGet("getActivity")]
        public async Task<IActionResult> GetMonthlyActivityByUserId(int Id, CancellationToken token)
        {
            var result = await _mediator.Send(new GetActivityRequest(Id), token);
            if (result.IsFailure) return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}
