using CSharpFunctionalExtensions;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HealthDiary.API.MediatR.Handlers.Activity
{
    public class GetYearlyActivity
    {
        public record GetWeeklyActivityRequest(int UserId) : IRequest<Result<List<WeaklyActivityDto>>>;

        public sealed class Handler : IRequestHandler<GetWeeklyActivityRequest, Result<List<WeaklyActivityDto>>>
        {
            private readonly DataContext _context;

            const string ActivitiesNotFoundErrorMessage = "Activities not found";

            public Handler(DataContext dataContext) => _context = dataContext;

            public async Task<Result<List<WeaklyActivityDto>>> Handle(GetWeeklyActivityRequest request, CancellationToken cancellationToken)
            {
                var activities = await _context.Activities
                    .AsNoTracking()
                    .Where(x => x.UserId == request.UserId)
                    .OrderBy(x => x.CreationDate)
                    .ToListAsync(cancellationToken);

                if (!activities.Any()) return Result.Failure<List<WeaklyActivityDto>>(ActivitiesNotFoundErrorMessage);

                var weeklyActivities = activities
                    .GroupBy(x => GetWeekStartAndEnd(x.CreationDate))
                    .Select(a => new WeaklyActivityDto
                    {
                        WeekRange = $"{a.Key.start:dd.MM} - {a.Key.end:dd.MM}",
                        Year = a.First().CreationDate.Year,
                        TotalCalorieConsumption = a.Where(b => b.CreationDate >= a.Key.start && b.CreationDate <= a.Key.end)
                                                   .Sum(b => b.TotalCalorieConsumption),
                        TotalDistance = a.Where(b => b.CreationDate >= a.Key.start && b.CreationDate <= a.Key.end)
                                         .Sum(b => b.TotalDistance),
                        TotalExerciseTime = a.Where(b => b.CreationDate >= a.Key.start && b.CreationDate <= a.Key.end)
                                             .Sum(b => b.TotalExerciseTime),
                        LastUpdate = a.Where(b => b.CreationDate >= a.Key.start && b.CreationDate <= a.Key.end)
                                      .Max(b => b.CreationDate)
                    })
                    .ToList();

                return Result.Success(weeklyActivities);

            }

            private (DateTime start, DateTime end) GetWeekStartAndEnd(DateTime date)
            {
                var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
                var startOfWeek = date.AddDays(-diff).Date;
                var endOfWeek = startOfWeek.AddDays(6).Date;
                return (startOfWeek, endOfWeek);
            }
        }
    }
}
