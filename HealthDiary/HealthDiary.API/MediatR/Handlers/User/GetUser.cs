using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.User
{
    public static class GetUser 
    {
        public record GetUserRequest(int Id) : IRequest<Result<UserInfoDto>>;

        public sealed class Handler : IRequestHandler<GetUserRequest, Result<UserInfoDto>>
        {
            private readonly DataContext _context;

            private const string UserNotFoundError = "User not found";

            public Handler(DataContext context) => _context = context;

            public async Task<Result<UserInfoDto>> Handle(GetUserRequest request, CancellationToken cancellationToken)
            {              
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
    }
}
