using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Weight
{
    public static class GetWeights
    {
        public record GetWeightsRequest(int Id) : IRequest<Result>;

        public sealed class Handler : IRequestHandler<GetWeightsRequest, Result>
        {
            private readonly DataContext _context;

            public Handler(DataContext context) => _context = context;

            public async Task<Result> Handle(GetWeightsRequest request, CancellationToken cancellationToken)
            {             
                var userWeights = await _context.Weights
                    .Where(x => x.UserId == request.Id)
                    .ToListAsync(cancellationToken);

                return Result.Success(userWeights);
            }
        }
    }
}
