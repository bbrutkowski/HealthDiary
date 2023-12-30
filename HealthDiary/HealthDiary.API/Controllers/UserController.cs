﻿using HealthDiary.API.Context;
using HealthDiary.API.Context.Model;
using HealthDiary.API.Context.Model.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly string UserNotFoundError = "User not found";

        public UserController(DataContext dataContext)
        {
            _context = dataContext;
        }

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] UserDto userDto, CancellationToken token)
        {
            if (userDto is null) return BadRequest();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.IsActive && x.Name == userDto.UserName && x.Password == userDto.Password, token);

            if (user is null) return NotFound(UserNotFoundError);

            var role = user.Role;

            return Ok(new {Role = role});
        }

        [HttpPost(nameof(Delete))]
        public async Task<IActionResult> Delete([FromBody] UserDto userDto, CancellationToken token)
        {
            if (userDto is null) return BadRequest();

            var user = await _context.Users.Where(x => x.IsActive)
                                           .FirstOrDefaultAsync(x => x.Name == userDto.UserName, token);

            if (user == null) return NotFound(UserNotFoundError);

            user.IsActive = false;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route(nameof(Register))]
        public async Task<IActionResult> Register([FromBody] User user, CancellationToken token)
        {
            if (user is null) return BadRequest();

            try
            {
                await _context.Users.AddAsync(user, token);

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }                              
        }
    }
}