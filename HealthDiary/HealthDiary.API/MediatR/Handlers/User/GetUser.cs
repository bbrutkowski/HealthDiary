using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Context.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.User
{ 
    public static class GetUser 
    {
        public record GetUserRequest(int Id) : IRequest<OperationResult>;

        public sealed class Handler : IRequestHandler<GetUserRequest, OperationResult>
        {
            private readonly DataContext _context;

            public Handler(DataContext context) => _context = context;

            private const string UserNotFoundError = "User not found";
            private const string UserIdError = "User id must be greater than 0";

            public async Task<OperationResult> Handle(GetUserRequest request, CancellationToken cancellationToken)
            {
                if (request.Id <= 0) return OperationResultExtensions.Failure(UserIdError);

                var user = await _context.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (user is null) return OperationResultExtensions.Failure(UserNotFoundError);

                return OperationResultExtensions.Success(user);
            }
        }   
    }
}
