using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using HealthDiary.API.Context.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Weight
{
    public class GetWeightGoalProgress
    {
        public record GetWeightGoalProgressRequest(int Id) : IRequest<Result<decimal>>;

        public sealed class Handler : IRequestHandler<GetWeightGoalProgressRequest, Result<decimal>>
        {
            private readonly DataContext _context;

            private const string WeightGoalNotFoundError = "Weight goal not found";

            public Handler(DataContext context) => _context = context;

            public async Task<Result<decimal>> Handle(GetWeightGoalProgressRequest request, CancellationToken cancellationToken)
            {
                var goal = await _context.WeightGoals
                    .AsNoTracking()
                    .Where(x => x.UserId == request.Id && x.IsSet)
                    .FirstOrDefaultAsync(cancellationToken);

                if (goal is null) return Result.Failure<decimal>(WeightGoalNotFoundError);

                var totalDays = (goal.TargetDate - goal.CreationDate).TotalDays;
                var daysPassed = (DateTime.Now - goal.CreationDate).TotalDays;
                var progressPercentage = (daysPassed / totalDays) * 100;

                return Result.Success((decimal)progressPercentage);
            }
        }
    }
}
