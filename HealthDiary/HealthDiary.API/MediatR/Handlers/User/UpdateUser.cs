using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserAlias = HealthDiary.API.Model.Main.User;

namespace HealthDiary.API.MediatR.Handlers.User
{
    public static class UpdateUser
    {
        public record UpdateUserRequest(
            int Id, string Name, string Surname, string Email, DateTime BirthDate, string PhoneNumber, Gender Gender,
            string Country, string City, string Street, int BuildingNumber, int ApartmentNumber, string PostalCode) : IRequest<Result<bool>>;

        public sealed class Handler : IRequestHandler<UpdateUserRequest, Result<bool>>
        {
            private readonly DataContext _context;
            private readonly IValidator<UpdateUserRequest> _requestValidator;

            private const string UserNotFoundError = "User with given Id not found";
            private const string UpdateUserError = "Error occurred while updating user";

            public Handler(DataContext context, IValidator<UpdateUserRequest> validator)
            {
                _context = context;
                _requestValidator = validator;
            }

            public async Task<Result<bool>> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
            {
                var validationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid) return Result.Failure<bool>(string.Join(Environment.NewLine, validationResult.Errors));

                var userToUpdate = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (userToUpdate is null) return Result.Failure<bool>(UserNotFoundError);

                if (NeedsAddressUpdate(request))
                {
                    await _context.Entry(userToUpdate).Reference(u => u.Address).LoadAsync(cancellationToken);
                    userToUpdate.Address ??= new Address(); 
                }

                UpdateUserInformation(userToUpdate, request);

                var changes = await _context.SaveChangesAsync(cancellationToken);
                return changes > 0 ? Result.Success(true) : Result.Failure<bool>(UpdateUserError);
            }

            private bool NeedsAddressUpdate(UpdateUserRequest request)
            {
                return !string.IsNullOrEmpty(request.Country) ||
                       !string.IsNullOrEmpty(request.City) ||
                       !string.IsNullOrEmpty(request.Street) ||
                       request.BuildingNumber != 0 ||  
                       request.ApartmentNumber != 0 || 
                       !string.IsNullOrEmpty(request.PostalCode);
            }

            private static void UpdateUserInformation(UserAlias user, UpdateUserRequest request)
            {
                user.Name = request.Name;
                user.Surname = request.Surname;
                user.Email = request.Email;
                user.BirthDate = request.BirthDate;
                user.PhoneNumber = request.PhoneNumber;
                user.Gender = request.Gender;

                if (user.Address is null) return;

                user.Address.Country = request.Country;
                user.Address.City = request.City;
                user.Address.Street = request.Street;
                user.Address.BuildingNumber = request.BuildingNumber;
                user.Address.ApartmentNumber = request.ApartmentNumber;
                user.Address.PostalCode = request.PostalCode;
            }
        }

        public sealed class Validator : AbstractValidator<UpdateUserRequest>
        {
            public const string UserIdValidation = "User Id must be greater than 0";

            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(UserIdValidation);
            }
        }
    }
}
