using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Context.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Weight
{
    public static class GetWeightsByMonth
    {
        public record GetWeightsByMonthRequest(int Id) : IRequest<OperationResult>;

        public sealed class Handler : IRequestHandler<GetWeightsByMonthRequest, OperationResult>
        {
            private readonly DataContext _context;

            public Handler(DataContext context) => _context = context;

            public async Task<OperationResult> Handle(GetWeightsByMonthRequest request, CancellationToken cancellationToken)
            {
                var currentMonth = DateTime.Now.Month;

                var weightsByMonth = await _context.Weights
                    .Where(x => x.UserId == request.Id && x.CreationDate.Month == currentMonth)
                    .ToListAsync(cancellationToken);

                return OperationResultExtensions.Success(weightsByMonth);
            }
        }
    }
}
