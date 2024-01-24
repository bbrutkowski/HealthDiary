using FluentValidation;
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
            private readonly IValidator<GetUserRequest> _requestValidator;

            private const string UserNotFoundError = "User not found";

            public Handler(DataContext context, IValidator<GetUserRequest> validator)
            {
                _context = context;
                _requestValidator = validator;
            }

            public async Task<OperationResult> Handle(GetUserRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return OperationResultExtensions.Failure(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var user = await _context.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                if (user is null) return OperationResultExtensions.Failure(UserNotFoundError);

                return OperationResultExtensions.Success(user);
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
