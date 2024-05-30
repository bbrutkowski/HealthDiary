using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Context.Model;
using HealthDiary.API.Context.Model.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserAlias = HealthDiary.API.Context.Model.Main.User;

namespace HealthDiary.API.MediatR.Handlers.User
{
    public static class UpdateUser
    {
        public record UpdateUserRequest(
            int Id, string Name, string Surname, string Email, DateTime BirthDate, string PhoneNumber, Gender Gender,
            string Country, string City, string Street, int BuildingNumber, int ApartmentNumber, string PostalCode) : IRequest<OperationResult>;

        public sealed class Handler : IRequestHandler<UpdateUserRequest, OperationResult>
        {
            private readonly DataContext _context;
            private readonly IValidator<UpdateUserRequest> _requestValidator;

            private const string UserNotFoundError = "User with given Id not found";

            public Handler(DataContext context, IValidator<UpdateUserRequest> validator)
            {
                _context = context;
                _requestValidator = validator;
            }

            public async Task<OperationResult> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return OperationResultExtensions.Failure(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var userToUpdate = await _context.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (userToUpdate == null) return OperationResultExtensions.Failure(UserNotFoundError);

                UpdateUserInformation(userToUpdate, request);

                await _context.SaveChangesAsync(cancellationToken);
                return OperationResultExtensions.Success();
            }

            private static void UpdateUserInformation(UserAlias user, UpdateUserRequest updateRequest)
            {
                user.Name = updateRequest.Name;
                user.Surname = updateRequest.Surname;
                user.Email = updateRequest.Email;
                user.BirthDate = updateRequest.BirthDate;
                user.PhoneNumber = updateRequest.PhoneNumber;
                user.Gender = updateRequest.Gender;

                user.Address ??= new Address();

                user.Address.Country = updateRequest.Country;
                user.Address.City = updateRequest.City;
                user.Address.Street = updateRequest.Street;
                user.Address.BuildingNumber = updateRequest.BuildingNumber;
                user.Address.ApartmentNumber = updateRequest.ApartmentNumber;
                user.Address.PostalCode = updateRequest.PostalCode;
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
