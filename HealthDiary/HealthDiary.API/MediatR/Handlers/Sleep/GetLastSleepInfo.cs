using CSharpFunctionalExtensions;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Sleep
{
    public static class GetLastSleepInfo
    {
        public record GetSleepInfoRequest(int Id) : IRequest<Result<SleepInfoDto>>;

        public sealed class Handler : IRequestHandler<GetSleepInfoRequest, Result<SleepInfoDto>>
        {
            private readonly DataContext _context;

            public const string SleepInfoNotFoundError = "Sleep information not found";

            public Handler(DataContext context) => _context = context;

            public async Task<Result<SleepInfoDto>> Handle(GetSleepInfoRequest request, CancellationToken cancellationToken)
            {
                var sleepInformation = await _context.Sleeps
                    .AsNoTracking()
                    .Where(x => x.UserId == request.Id)
                    .OrderByDescending(x => x.CreationDate)
                    .Select(x => new SleepInfoDto
                    {
                        SleepTime = x.SleepTime,
                        CreationDate = x.CreationDate,
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (sleepInformation is null) return Result.Failure<SleepInfoDto>(SleepInfoNotFoundError);

                return Result.Success(sleepInformation);
            }
        }
    }
}
