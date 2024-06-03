using CSharpFunctionalExtensions;
using HealthDiary.API.Context.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Sleep
{
    public static class GetLastSleepInfo
    {
        public record GetLastSleepInfoByUserIdRequest(int Id) : IRequest<Result>;

        public sealed class Handler : IRequestHandler<GetLastSleepInfoByUserIdRequest, Result>
        {
            private readonly DataContext _context;

            public const string SleepInfoNotFoundError = "Sleep information not found";

            public Handler(DataContext context) => _context = context;

            public async Task<Result> Handle(GetLastSleepInfoByUserIdRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    var sleepInformation = await _context.Sleeps
                   .Where(x => x.UserId == request.Id)
                   .OrderBy(x => x.CreationDate)
                   .LastOrDefaultAsync(cancellationToken);

                    if (sleepInformation is null) return Result.Failure(SleepInfoNotFoundError);

                    return Result.Success(sleepInformation);
                }
                catch (Exception e)
                {
                    return Result.Failure(e.Message);
                }
            }
        }
    }
}
