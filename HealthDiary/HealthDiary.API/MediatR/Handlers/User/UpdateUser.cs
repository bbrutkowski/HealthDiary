using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Context.Model;
using HealthDiary.API.Context.Model.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

            private const string UserNotFoundError = "User with given Id not found";
            private const string UserIdError = "User Id must be greater than 0";

            public Handler(DataContext context) => _context = context;

            public async Task<OperationResult> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
            {
                if(request.Id == 0) return OperationResultExtensions.Failure(UserIdError);

                var userToUpdate = await _context.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (userToUpdate == null) return OperationResultExtensions.Failure(UserNotFoundError);

                userToUpdate.Name = request.Name;
                userToUpdate.Surname = request.Surname;
                userToUpdate.Email = request.Email;
                userToUpdate.BirthDate = request.BirthDate;
                userToUpdate.PhoneNumber = request.PhoneNumber;
                userToUpdate.Gender = request.Gender;

                userToUpdate.Address ??= new Address();

                userToUpdate.Address.Country = request.Country;
                userToUpdate.Address.City = request.City;
                userToUpdate.Address.Street = request.Street;
                userToUpdate.Address.BuildingNumber = request.BuildingNumber;
                userToUpdate.Address.ApartmentNumber = request.ApartmentNumber;
                userToUpdate.Address.PostalCode = request.PostalCode;

                await _context.SaveChangesAsync(cancellationToken);
                return OperationResultExtensions.Success();
            }       
        }
    }
}
