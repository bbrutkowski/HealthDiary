using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Weight
{
    public static class GetWeightsByMonth
    {
        public record GetWeightsByMonthRequest(int Id) : IRequest<Result<List<WeightDto>>>;

        public sealed class Handler : IRequestHandler<GetWeightsByMonthRequest, Result<List<WeightDto>>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context) => _context = context;

            public async Task<Result<List<WeightDto>>> Handle(GetWeightsByMonthRequest request, CancellationToken cancellationToken)
            {               
                var currentMonth = DateTime.Now.Month;
                var startOfMonth = new DateTime(DateTime.Now.Year, currentMonth, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);

                var weightsByMonth = await _context.Weights
                    .AsNoTracking()
                    .Where(x => x.UserId == request.Id && x.CreationDate >= startOfMonth && x.CreationDate <= endOfMonth)
                    .OrderBy(x => x.CreationDate)
                    .Select(x => new WeightDto
                    {
                        Value = x.Value,
                        CreationDate = x.CreationDate,
                    })
                    .ToListAsync(cancellationToken);

                return Result.Success(weightsByMonth);
            }
        }
    }
}
