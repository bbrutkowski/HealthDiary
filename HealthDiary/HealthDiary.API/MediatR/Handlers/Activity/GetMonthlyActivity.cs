using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Activity
{
    public static class GetMonthlyActivity
    {
        public record GetActivityRequest(int Id) : IRequest<Result>;

        public sealed class Handler : IRequestHandler<GetActivityRequest, Result>
        {
            private readonly DataContext _context;
            private readonly IValidator<GetActivityRequest> _requestValidator;

            const string ActivitiesNotFoundErrorMessage = "Activities not found";

            public Handler(DataContext dataContext, IValidator<GetActivityRequest> validator)
            {
                _context = dataContext;
                _requestValidator = validator;
            }

            public async Task<Result> Handle(GetActivityRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var today = DateTime.Now;
                var currentMonth = today.Month;

                var activities = await _context.Activities
                    .Where(x => x.UserId == request.Id && x.CreationDate.Month == currentMonth)
                    .OrderBy(x => x.CreationDate)
                    .ToListAsync(cancellationToken);

                if (!activities.Any()) return Result.Failure<TotalMonthlyActivityDto>(ActivitiesNotFoundErrorMessage);

                var totalActivity = new TotalMonthlyActivityDto()
                {
                    TotalCalorieConsumption = activities.Sum(x => x.TotalCalorieConsumption),
                    TotalDistance = activities.Sum(x => x.TotalDistance),
                    TotalExerciseTime = activities.Sum(x => x.TotalExerciseTime),
                    LastUpdate = activities.Select(x => x.CreationDate).LastOrDefault()
                };

                return Result.Success<TotalMonthlyActivityDto>(totalActivity);
            }
        }

        public sealed class Validator : AbstractValidator<GetActivityRequest>
        {
            public const string UserIdValidation = "User Id must be greater than 0";

            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(UserIdValidation);
            }
        }
    }
}
