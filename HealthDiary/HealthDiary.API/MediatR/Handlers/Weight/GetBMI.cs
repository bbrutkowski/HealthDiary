using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.DTO;
using HealthDiary.API.Model.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static HealthDiary.API.MediatR.Handlers.Weight.GetWeightGoal;

namespace HealthDiary.API.MediatR.Handlers.Weight
{
    public class GetBMI
    {
        public record GetBmiRequest(int Id) : IRequest<Result<BmiDto>>;

        public sealed class Handler : IRequestHandler<GetBmiRequest, Result<BmiDto>>
        {
            private readonly DataContext _context;
            private readonly IValidator<GetBmiRequest> _requestValidator;

            private const string UserHeightError = "No user height";
            private const string UserWeightError = "No user weight";
            private const string UserBmiNotFoundError = "BMI not found";

            public Handler(DataContext context, IValidator<GetBmiRequest> validator)
            {
                _context = context;
                _requestValidator = validator;
            }

            public async Task<Result<BmiDto>> Handle(GetBmiRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure<BmiDto>(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var userHeight = await _context.Users
                    .AsNoTracking()
                    .Where(x => x.Id == request.Id && x.IsActive)
                    .Select(x => x.Height)
                    .FirstOrDefaultAsync(cancellationToken);

                if (userHeight is default(decimal)) return Result.Failure<BmiDto>(UserHeightError);

                var lastWeight = await _context.Weights
                    .AsNoTracking()
                    .Where(x => x.UserId == request.Id)
                    .OrderByDescending(x => x.CreationDate)
                    .Select(x => x.Value)
                    .FirstOrDefaultAsync(cancellationToken);

                if (lastWeight is default(decimal)) return Result.Failure<BmiDto>(UserWeightError);

                var bmiValue = lastWeight / (userHeight * userHeight);

                var bmi = await _context.BMIs
                    .AsNoTracking()
                    .Where(x => bmiValue >= x.MinValue && bmiValue <= x.MaxValue)
                    .Select(x => new BmiDto
                    {
                        Value = bmiValue,
                        Description = x.Description,
                        IndexColor = x.IndexColor                       
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (bmi is null) return Result.Failure<BmiDto>(UserBmiNotFoundError);

                return Result.Success(bmi);
            }
        }

        public sealed class Validator : AbstractValidator<GetBmiRequest>
        {
            public const string UserIdValidation = "User Id must be greater than 0";

            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(UserIdValidation);
            }
        }
    }
}
