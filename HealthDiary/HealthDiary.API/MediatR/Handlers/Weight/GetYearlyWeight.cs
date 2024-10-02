using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Weight
{
    public static class GetYearlyWeight
    {
        public record GetYearlyWeightRequest(int Id) : IRequest<Result<List<WeightDto>>>;

        public sealed class Handler : IRequestHandler<GetYearlyWeightRequest, Result<List<WeightDto>>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context) => _context = context;

            public async Task<Result<List<WeightDto>>> Handle(GetYearlyWeightRequest request, CancellationToken cancellationToken)
            {                
                var currentYear = DateTime.Now.Year;

                var weightsByMonth = await _context.Weights
                    .Where(x => x.UserId == request.Id && x.CreationDate.Year == currentYear)
                    .OrderBy(x => x.CreationDate)
                    .Select(x => new WeightDto
                    {
                        CreationDate = x.CreationDate,
                        Value = x.Value
                    })
                    .ToListAsync(cancellationToken);

                return Result.Success(weightsByMonth);
            }
        }
    }
}
