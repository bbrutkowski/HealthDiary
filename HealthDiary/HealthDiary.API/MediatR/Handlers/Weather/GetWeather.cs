using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Context.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Weather
{
    public static class GetWeather 
    {
        public record GetWeatherRequest() : IRequest<OperationResult>;

        public sealed class Handler : IRequestHandler<GetWeatherRequest, OperationResult>
        {
            private readonly DataContext _context;

            public Handler(DataContext context) => _context = context;

            private const string ContentNotFoundError = "No forecast with given Id";
            public async Task<OperationResult> Handle(GetWeatherRequest request, CancellationToken cancellationToken)
            {
                var randomId = new Random().Next(1, 6);

                var weatherContent = await _context.WeatherInformations
                    .Where(x => x.Id == randomId && x.IsActive)
                    .Select(x => x.Content)
                    .FirstOrDefaultAsync(cancellationToken);

                if (weatherContent is null) return OperationResultExtensions.Failure(ContentNotFoundError + $"{randomId}");

                return OperationResultExtensions.Success(weatherContent);
            }
        }       
    }
}
