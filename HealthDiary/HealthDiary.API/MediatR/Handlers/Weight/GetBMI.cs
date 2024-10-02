using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Weight
{
    public class GetBMI
    {
        public record GetBmiRequest(int Id) : IRequest<Result<BmiDto>>;

        public sealed class Handler : IRequestHandler<GetBmiRequest, Result<BmiDto>>
        {
            private readonly DataContext _context;

            private const string UserHeightError = "No user height";
            private const string UserWeightError = "No user weight";
            private const string UserBmiNotFoundError = "BMI not found";

            public Handler(DataContext context) => _context = context;

            public async Task<Result<BmiDto>> Handle(GetBmiRequest request, CancellationToken cancellationToken)
            {              
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
    }
}
