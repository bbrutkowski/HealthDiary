﻿using HealthDiary.API.Context.Model.Dto;
using HealthDiary.API.Context.Model;
using HealthDiary.API.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HealthDiary.API.Helpers;

namespace HealthDiary.API.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly string UserNotFoundError = "User not found";
        private readonly string UserCredentialsError = "User name or password are wrong";

        public UserController(DataContext dataContext) => _context = dataContext;

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] UserDto userDto, CancellationToken token)
        {
            if (userDto is null) return BadRequest();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.IsActive && x.Name == userDto.UserName, token);

            if (user is null) return NotFound(UserNotFoundError);

            if (!PasswordHasher.Varify(userDto.Password, user.Password)) return NotFound(UserCredentialsError);

            var role = user.Role;

            return Ok(new { Role = role });
        }

        [HttpPost]
        [Route(nameof(Register))]
        public async Task<IActionResult> Register([FromBody] User user, CancellationToken token)
        {
            if (user is null) return BadRequest();

            user.Password = PasswordHasher.Hash(user.Password);

            var validationResult = await new UserValidator(_context).ValidateUser(user, token);
            if (validationResult != string.Empty) return ValidationProblem(validationResult);

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
