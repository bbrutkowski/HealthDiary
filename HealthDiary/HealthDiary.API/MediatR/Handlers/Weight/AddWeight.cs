using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using MediatR;
using WeightAlias = HealthDiary.API.Model.Main.Weight;

namespace HealthDiary.API.MediatR.Handlers.Weight
{
    public class AddWeight
    {
        public record AddWeightRequest(int Id, decimal Weight, DateTime Date) : IRequest<Result<bool>>;

        public sealed class Handler : IRequestHandler<AddWeightRequest, Result<bool>>
        {
            private readonly DataContext _context;
            private readonly IValidator<AddWeightRequest> _requestValidator;

            public const string AddWeightError = "Error occurred while saving the weight";

            public Handler(DataContext context, IValidator<AddWeightRequest> validator)
            {
                _context = context;
                _requestValidator = validator;
            }

            public async Task<Result<bool>> Handle(AddWeightRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure<bool>(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var newWeight = new WeightAlias
                {
                    UserId = request.Id,
                    Value = request.Weight,
                    CreationDate = request.Date
                };

                await _context.Weights.AddAsync(newWeight, cancellationToken);
                var changes = await _context.SaveChangesAsync(cancellationToken);

                return changes > 0 ? Result.Success(true) : Result.Failure<bool>(AddWeightError);
            }
        }

        public sealed class Validator : AbstractValidator<AddWeightRequest>
        {
            public const string UserIdValidationError = "User Id must be greater than 0";
            public const string WeightValidationError = "Incorrect height value";
            public const string CreationDateValidationError = "Incorrect date value";

            public Validator()
            {
                RuleFor(x => x.Weight).GreaterThan(0).WithMessage(WeightValidationError);
                RuleFor(x => x.Date).GreaterThan(default(DateTime)).WithMessage(CreationDateValidationError);
            }
        }
    }
}
