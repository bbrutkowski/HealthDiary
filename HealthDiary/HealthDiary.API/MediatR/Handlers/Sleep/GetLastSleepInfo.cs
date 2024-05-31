using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Context.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Sleep
{
    public static class GetLastSleepInfo
    {
        public record GetLastSleepInfoByUserIdRequest(int Id) : IRequest<OperationResult>;

        public sealed class Handler : IRequestHandler<GetLastSleepInfoByUserIdRequest, OperationResult>
        {
            private readonly DataContext _context;

            public const string SleepInfoNotFoundError = "Sleep information not found";

            public Handler(DataContext context) => _context = context;

            public async Task<OperationResult> Handle(GetLastSleepInfoByUserIdRequest request, CancellationToken cancellationToken)
            {
                var sleepInformation = await _context.Sleeps
                    .Where(x => x.UserId == request.Id)
                    .OrderBy(x => x.CreationDate)
                    .LastOrDefaultAsync(cancellationToken);

                if (sleepInformation is null) return OperationResultExtensions.Failure(SleepInfoNotFoundError);

                return OperationResultExtensions.Success(sleepInformation);
            }
        }
    }
}
