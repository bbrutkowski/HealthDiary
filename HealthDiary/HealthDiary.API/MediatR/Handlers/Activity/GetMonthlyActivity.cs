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
        public record GetActivityRequest(int UserId) : IRequest<Result<TotalMonthlyActivityDto>>;

        public sealed class Handler : IRequestHandler<GetActivityRequest, Result<TotalMonthlyActivityDto>>
        {
            private readonly DataContext _context;

            const string ActivitiesNotFoundErrorMessage = "Activities not found";

            public Handler(DataContext dataContext) => _context = dataContext;

            public async Task<Result<TotalMonthlyActivityDto>> Handle(GetActivityRequest request, CancellationToken cancellationToken)
            {            
                var today = DateTime.Now;
                var currentMonth = today.Month;

                var activities = await _context.Activities
                    .AsNoTracking()
                    .Where(x => x.UserId == request.UserId && x.CreationDate.Month == currentMonth)
                    .OrderByDescending(x => x.CreationDate)
                    .ToListAsync(cancellationToken);

                if (!activities.Any()) return Result.Failure<TotalMonthlyActivityDto>(ActivitiesNotFoundErrorMessage);

                var totalActivity = new TotalMonthlyActivityDto()
                {
                    TotalCalorieConsumption = activities.Sum(x => x.TotalCalorieConsumption),
                    TotalDistance = activities.Sum(x => x.TotalDistance),
                    TotalExerciseTime = activities.Sum(x => x.TotalExerciseTime),
                    LastUpdate = activities.Select(x => x.CreationDate).FirstOrDefault()
                };

                return Result.Success(totalActivity);
            }
        }
    }
}
