using HealthDiary.BusinessLogic.Models.Main;
using HealthDiary.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HealthDiary.BusinessLogic.Helpers
{
    internal class UserValidator
    {
        private readonly DataContext _context;

        private const string ValidationError = "Error occurred during validation. User not found";
        private const string UserNameValidationError = "User name already exists";
        private const string EmailValidationError = "Email adress is taken";

        public UserValidator(DataContext context) => _context = context;

        public async Task<string> ValidateUser(User newUser, CancellationToken token)
        {
            var user = await _context.Users.Where(x => x.Name == newUser.Name && x.IsActive)
                                           .FirstOrDefaultAsync(token);

            if (user is null) return ValidationError;

            if (user.Name == newUser.Name) return UserNameValidationError;
            if (user.Email == newUser.Email) return EmailValidationError;

            return string.Empty;
        }
    }
}
