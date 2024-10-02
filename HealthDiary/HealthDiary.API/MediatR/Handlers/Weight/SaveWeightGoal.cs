using CSharpFunctionalExtensions;
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

            private const string SaveWeightGoalError = "Error occurred while saving weight goal";

            public Handler(DataContext context) => _context = context;

            public async Task<Result<bool>> Handle(SaveWeightGoalRequest request, CancellationToken cancellationToken)
            {
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
    }
}
