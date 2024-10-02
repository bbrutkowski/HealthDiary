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

            public Handler(DataContext context) => _context = context;

            public async Task<Result<WeightGoalDto>> Handle(GetWeightGoalRequest request, CancellationToken cancellationToken)
            {               
                var goal = await _context.WeightGoals
                    .AsNoTracking()
                    .Where(x => x.UserId == request.Id && x.IsSet) 
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
    }
}
