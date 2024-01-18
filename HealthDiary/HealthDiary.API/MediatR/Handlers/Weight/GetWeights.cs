using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Context.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Weight
{
    public static class GetWeights
    {
        public record GetWeightsRequest(int Id) : IRequest<OperationResult>;

        public sealed class Handler : IRequestHandler<GetWeightsRequest, OperationResult>
        {
            private readonly DataContext _context;

            public Handler(DataContext context) => _context = context;

            public const string UserIdError = "User id must be greater than 0";

            public async Task<OperationResult> Handle(GetWeightsRequest request, CancellationToken cancellationToken)
            {
                if (request.Id <= 0) return OperationResultExtensions.Failure(UserIdError);

                var userWeights = await _context.Weights.Where(x => x.UserId == request.Id)
                    .ToListAsync(cancellationToken);

                return OperationResultExtensions.Success(userWeights);
            }
        }
    }
}
