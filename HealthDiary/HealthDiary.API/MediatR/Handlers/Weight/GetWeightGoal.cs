using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Weight
{
    public class GetWeightGoal
    {
        public record GetWeightGoalRequest(int Id) : IRequest<Result<WeightGoalDto>>;

        public sealed class Handler : IRequestHandler<GetWeightGoalRequest, Result<WeightGoalDto>>
        {
            private readonly DataContext _context;
            private readonly IValidator<GetWeightGoalRequest> _requestValidator;

            public Handler(DataContext context, IValidator<GetWeightGoalRequest> validator)
            {
                _context = context;
                _requestValidator = validator;
            }

            public async Task<Result<WeightGoalDto>> Handle(GetWeightGoalRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure<WeightGoalDto>(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var goal = await _context.WeightGoals
                    .AsNoTracking()
                    .Where(x => x.UserId == request.Id) 
                    .Select(x => new WeightGoalDto
                    {
                        IsSet = x.IsSet,
                        CurrentWeight = x.CurrentWeight,
                        TargetWeight = x.TargetWeight,
                        CreationDate = x.CreationDate,
                        TargetDate = x.TargetDate
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                return Result.Success(goal ?? new WeightGoalDto());
            }
        }

        public sealed class Validator : AbstractValidator<GetWeightGoalRequest>
        {
            public const string UserIdValidation = "User Id must be greater than 0";

            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(UserIdValidation);
            }
        }
    }
}
