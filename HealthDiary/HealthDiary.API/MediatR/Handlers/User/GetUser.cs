using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.DTO;
using HealthDiary.API.Model.Main;
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
                     .Join(_context.Avatars,
                         user => user.Id,
                         avatar => avatar.UserId,
                         (user, avatar) => new UserInfoDto
                         {
                             Id = user.Id,
                             Name = user.Name,
                             Surname = user.Surname,
                             Email = user.Email,
                             DateOfBirth = user.BirthDate,
                             PhoneNumber = user.PhoneNumber,
                             Gender = user.Gender,
                             Address = user.Address != null ? new AddressDto
                             {
                                 Country = user.Address.Country,
                                 City = user.Address.City,
                                 Street = user.Address.Street,
                                 BuildingNumber = user.Address.BuildingNumber,
                                 ApartmentNumber = user.Address.ApartmentNumber,
                                 PostalCode = user.Address.PostalCode
                             } : null,
                             Avatar = avatar.Picture 
                         })
                     .FirstOrDefaultAsync(cancellationToken);

                if (userInfo is null) return Result.Failure<UserInfoDto>(UserNotFoundError);

                return Result.Success(userInfo);
            }
        }
    }
}
