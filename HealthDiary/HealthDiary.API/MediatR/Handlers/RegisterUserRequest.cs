using CSharpFunctionalExtensions;
using HealthDiary.API.Context;
using HealthDiary.API.Context.Model;
using HealthDiary.API.Helpers;
using MediatR;

namespace HealthDiary.API.MediatR.Commands
{
    public record RegisterUserRequest : IRequest<Result>
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }

    public class RegisterUser : IRequestHandler<RegisterUserRequest, Result>
    {
        private readonly DataContext _context;

        public RegisterUser(DataContext context) => _context = context;

        public async Task<Result> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                Name = request.Name,
                Email = request.Email,
                Password = PasswordHasher.Hash(request.Password)
            }; 

            try
            {
                await _context.Users.AddAsync(user, cancellationToken);

                await _context.SaveChangesAsync();

                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure(e.Message);
            }
        }
    }
}
