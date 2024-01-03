using HealthDiary.API.Context;
using HealthDiary.API.Context.Model;
using HealthDiary.API.Context.Model.Dto;
using HealthDiary.API.Helpers;
using HealthDiary.API.MediatR.Commands;
using HealthDiary.API.MediatR.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator) => _mediator = mediator;

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(request, token);
            if (result.IsFailure) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost]
        [Route(nameof(Register))]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken token)
        {
            var result = await _mediator.Send(request, token);
            if (result.IsFailure) return BadRequest(result);
            return Ok(result);
        }
    }
}