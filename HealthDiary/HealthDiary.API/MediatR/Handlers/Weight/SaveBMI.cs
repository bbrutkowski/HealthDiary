using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeightAlias = HealthDiary.API.Model.Main.Weight;

namespace HealthDiary.API.MediatR.Handlers.Weight
{
    public class SaveBMI
    {
        public record SaveBmiRequest(decimal Height, int UserId) : IRequest<Result<bool>>;

        public sealed class Handler : IRequestHandler<SaveBmiRequest, Result<bool>>
        {
            private readonly DataContext _context;
            private readonly IValidator<SaveBmiRequest> _requestValidator;

            private const string SaveBmiError = "Error occurred while saving BMI";
            private const string UserNotFoundError = "User not found";

            public Handler(DataContext context, IValidator<SaveBmiRequest> validator)
            {
                _context = context;
                _requestValidator = validator;
            }

            public async Task<Result<bool>> Handle(SaveBmiRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure<bool>(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var user = await _context.Users
                    .Where(x => x.Id == request.UserId)
                    .FirstOrDefaultAsync(cancellationToken);

                if (user is null) return Result.Failure<bool>(UserNotFoundError);

                user.Height = request.Height;
          
                var changes = await _context.SaveChangesAsync(cancellationToken);

                return changes > 0 ? Result.Success(true) : Result.Failure<bool>(SaveBmiError);
            }
        }

        public sealed class Validator : AbstractValidator<SaveBmiRequest>
        {
            public const string UserIdValidation = "User Id must be greater than 0";
            public const string HeightValidation = "Incorrect height value";

            public Validator()
            {
                RuleFor(x => x.UserId).GreaterThan(0).WithMessage(UserIdValidation);
                RuleFor(x => x.Height).GreaterThan(0).WithMessage(HeightValidation);
            }
        }
    }
}
