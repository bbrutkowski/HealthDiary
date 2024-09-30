using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.Main;
using MediatR;

namespace HealthDiary.API.MediatR.Handlers.Weight
{
    public class SaveWeightGoal
    {
        public record SaveWeightGoalRequest(int UserId, decimal CurrentWeight, decimal TargetWeight,
            DateTime CreationDate, DateTime TargetDate) : IRequest<Result<bool>>;

        public sealed class Handler : IRequestHandler<SaveWeightGoalRequest, Result<bool>>
        {
            private readonly DataContext _context;
            private readonly IValidator<SaveWeightGoalRequest> _requestValidator;

            private const string SaveWeightGoalError = "Error occurred while saving weight goal";

            public Handler(DataContext context, IValidator<SaveWeightGoalRequest> validator)
            {
                _context = context;
                _requestValidator = validator;
            }

            public async Task<Result<bool>> Handle(SaveWeightGoalRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure<bool>(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var weightGoal = new WeightGoal()
                {
                    IsSet = true,
                    UserId = request.UserId,
                    CreationDate = request.CreationDate == default ? DateTime.Now : request.CreationDate,
                    TargetDate = request.TargetDate,
                    CurrentWeight = request.CurrentWeight,
                    TargetWeight = request.TargetWeight
                };

                await _context.WeightGoals.AddAsync(weightGoal, cancellationToken);
                var changes = await _context.SaveChangesAsync(cancellationToken);

                return changes > 0 ? Result.Success(true) : Result.Failure<bool>(SaveWeightGoalError);
            }
        }

        public sealed class Validator : AbstractValidator<SaveWeightGoalRequest>
        {
            public const string UserIdValidation = "User Id must be greater than 0";

            public Validator()
            {
                RuleFor(x => x.UserId).GreaterThan(0).WithMessage(UserIdValidation);
            }
        }
    }
}
