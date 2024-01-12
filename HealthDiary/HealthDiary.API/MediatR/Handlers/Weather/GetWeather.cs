using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Context.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Weather
{
    public record GetWeatherRequest() : IRequest<OperationResult>;

    public class GetWeather : IRequestHandler<GetWeatherRequest, OperationResult>
    {
        private readonly DataContext _context;

        private const string ContentNotFoundError = "No forecast with given Id";

        public GetWeather(DataContext context) => _context = context;

        public async Task<OperationResult> Handle(GetWeatherRequest request, CancellationToken cancellationToken)
        {
            var randomId = new Random().Next(1, 6);

            var weatherContent = await _context.WeatherInformations.Where(x => x.IsActive).Select(x => x.Content).FirstOrDefaultAsync(cancellationToken);

            if (weatherContent is null) return OperationResultExtensions.Failure(ContentNotFoundError + $"{randomId}");

            return OperationResultExtensions.Success(weatherContent);
        }
    }
}
