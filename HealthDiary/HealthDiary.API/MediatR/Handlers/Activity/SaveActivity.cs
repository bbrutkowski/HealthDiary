using CSharpFunctionalExtensions;
using HealthDiary.API.Context.DataContext;
using MediatR;
using ActivityAlias = HealthDiary.API.Model.Main.Activity;

namespace HealthDiary.API.MediatR.Handlers.Activity
{
    public class SaveActivity
    {
        public record SaveActivityRequest(
            string Name,
            int Calories,
            DateTime Date,
            decimal Distance,
            int Id,
            decimal Time) : IRequest<Result<bool>>;

        public sealed class Handler : IRequestHandler<SaveActivityRequest, Result<bool>>
        {
            private readonly DataContext _context;

            const string SaveActivityErrorMessage = "Error while saving activity";

            public Handler(DataContext dataContext) => _context = dataContext;

            public async Task<Result<bool>> Handle(SaveActivityRequest request, CancellationToken cancellationToken)
            {
                var activity = new ActivityAlias
                {
                    Name = request.Name,
                    TotalCalorieConsumption = request.Calories,
                    TotalDistance = request.Distance,
                    TotalExerciseTime = request.Time,
                    UserId = request.Id,
                };

                await _context.Activities.AddAsync(activity, cancellationToken);
                var changes = await _context.SaveChangesAsync(cancellationToken);

                return changes > 0 ? Result.Success(true) : Result.Failure<bool>(SaveActivityErrorMessage);
            }
        }
    }
}
