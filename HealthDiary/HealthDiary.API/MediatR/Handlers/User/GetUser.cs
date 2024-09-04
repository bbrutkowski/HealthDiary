using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.DTO;
using HealthDiary.API.Model.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.IO;

namespace HealthDiary.API.MediatR.Handlers.User
{ 
    public static class GetUser 
    {
        public record GetUserRequest(int Id) : IRequest<Result<UserInfoDto>>;

        public sealed class Handler : IRequestHandler<GetUserRequest, Result<UserInfoDto>>
        {
            private readonly DataContext _context;
            private readonly IValidator<GetUserRequest> _requestValidator;

            private const string UserNotFoundError = "User not found";

            public Handler(DataContext context, IValidator<GetUserRequest> validator)
            {
                _context = context;
                _requestValidator = validator;
            }

            public async Task<Result<UserInfoDto>> Handle(GetUserRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure<UserInfoDto>(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var userInfo = await _context.Users
                    .AsNoTracking()
                    .Where(x => x.Id == request.Id)
                    .Select(x => new UserInfoDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Surname = x.Surname,
                        Email = x.Email,
                        DateOfBirth = x.BirthDate,
                        PhoneNumber = x.PhoneNumber,
                        Gender = x.Gender,
                        Address = x.Address != null ? new AddressDto
                        {
                            Country = x.Address.Country,
                            City = x.Address.City,
                            Street = x.Address.Street,
                            BuildingNumber = x.Address.BuildingNumber,
                            ApartmentNumber = x.Address.ApartmentNumber,
                            PostalCode = x.Address.PostalCode
                        } : null
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (userInfo is null) return Result.Failure<UserInfoDto>(UserNotFoundError);

                return Result.Success(userInfo);
            }
        }

        public sealed class Validator : AbstractValidator<GetUserRequest>
        {
            public const string UserIdValidation = "User Id must be greater than 0";

            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(UserIdValidation);
            }
        }
    }
}
