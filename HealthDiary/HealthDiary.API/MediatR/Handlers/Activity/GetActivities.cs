using CSharpFunctionalExtensions;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Activity
{
    public class GetActivities
    {
        public record GetActivitiesRequest(int UserId) : IRequest<Result<ActivityCatalog>>;

        public sealed class Handler : IRequestHandler<GetActivitiesRequest, Result<ActivityCatalog>>
        {
            private readonly DataContext _context;

            const string ActivitiesNotFoundErrorMessage = "Activities not found";

            public Handler(DataContext dataContext) => _context = dataContext;

            public async Task<Result<ActivityCatalog>> Handle(GetActivitiesRequest request, CancellationToken cancellationToken)
            {
                var activities = await _context.PhysicalActivities
                    .AsNoTracking()
                    .Select(x => new ActivityDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        MET = x.MET,
                        AverageSpeed = x.AverageSpeed                   
                    })
                    .ToListAsync(cancellationToken);

                var lastUserWeight = await _context.Weights
                    .AsNoTracking()
                    .Where(x => x.UserId == request.UserId)
                    .OrderByDescending(x => x.CreationDate)
                    .Select(x => x.Value)
                    .FirstOrDefaultAsync(cancellationToken);

                var activitiesCatalog = new ActivityCatalog
                {
                    Activities = activities,
                    LastUserWeight = lastUserWeight
                };

                return Result.Success(activitiesCatalog);
            }
        }
    }
}
