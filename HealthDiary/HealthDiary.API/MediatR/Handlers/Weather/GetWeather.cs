using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Model.DTO;
using HealthDiary.API.Model.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Runtime;

namespace HealthDiary.API.MediatR.Handlers.Weather
{
    public static class GetWeather 
    {
        public record GetWeatherRequest(decimal Latitude, decimal Longitude) : IRequest<Result<WeatherResponseDto>>;

        public sealed class Handler : IRequestHandler<GetWeatherRequest, Result<WeatherResponseDto>>
        {
            private readonly IHttpClientFactory _httpClientFactory;
            private readonly IValidator<GetWeatherRequest> _requestValidator;
            private readonly GeoSettingsDto _geoSettings;
            private readonly WeatherSettingsDto _weatherSettings;

            //private const string _geoUrl = "https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&key={2}";
            //private const string _geoKey = "AIzaSyAgo_hpgiUbiblsUvIBr-jAEDEsNUBB3tw";

            //private const string _weatherUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";
            //private const string _weatherApiKey = "d237302eb608500a27d0a2a5fa1e4a82";

            private const string _cityNameNotFoundError = "City name not found";
            private const string _deserializationError = "Error while deserializing data";

            public Handler(IHttpClientFactory httpClientFactory,
                           IValidator<GetWeatherRequest> validator,
                           IOptions<GeoSettingsDto> geoOptions,
                           IOptions<WeatherSettingsDto> weatherOptions)
            {
                _httpClientFactory = httpClientFactory;
                _requestValidator = validator;
                _geoSettings = geoOptions.Value;
                _weatherSettings = weatherOptions.Value;
            }

            public async Task<Result<WeatherResponseDto>> Handle(GetWeatherRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure<WeatherResponseDto>(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var geoRequest = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    _geoSettings.GeoUrl, request.Latitude.ToString("F6", System.Globalization.CultureInfo.InvariantCulture),
                    request.Longitude.ToString("F6", System.Globalization.CultureInfo.InvariantCulture),
                    _geoSettings.GeoApiKey);

                var client = _httpClientFactory.CreateClient();

                var response = await client.GetAsync(geoRequest, cancellationToken);
                if (!response.IsSuccessStatusCode) return Result.Failure<WeatherResponseDto>(response.StatusCode.ToString());

                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                var geocodeResponse = JsonConvert.DeserializeObject<GeocodeResponseDto>(content);
                if (geocodeResponse is null) return Result.Failure<WeatherResponseDto>(_deserializationError);

                if (geocodeResponse.Status is not "OK") return Result.Failure<WeatherResponseDto>(geocodeResponse.Status);

                var city = ExtractCityFromResults(geocodeResponse.Results);
                if (string.IsNullOrEmpty(city)) return Result.Failure<WeatherResponseDto>(_cityNameNotFoundError);

                var weatherResult = await GetWeatherConditions(city, cancellationToken);
                if (weatherResult.IsFailure) return Result.Failure<WeatherResponseDto>(weatherResult.Error);

                return Result.Success(weatherResult.Value);
            }

            private async Task<Result<WeatherResponseDto>> GetWeatherConditions(string city, CancellationToken cancellationToken)
            {
                var weatherRequest = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    _weatherSettings.WeatherUrl, city, _weatherSettings.WeatherApiKey);

                var client = _httpClientFactory.CreateClient();

                var weatherResponse = await client.GetAsync(weatherRequest, cancellationToken);
                if (!weatherResponse.IsSuccessStatusCode) return Result.Failure<WeatherResponseDto>(weatherResponse.StatusCode.ToString());

                var weatherContent = await weatherResponse.Content.ReadAsStringAsync(cancellationToken);

                var weatherData = JsonConvert.DeserializeObject<WeatherResponseDto>(weatherContent);
                if (weatherData is null) return Result.Failure<WeatherResponseDto>(_deserializationError);

                return Result.Success(weatherData);
            }

            private static string ExtractCityFromResults(GeocodeResultDto[] results)
            {
                foreach (var result in results)
                {
                    foreach (var component in result.AddressComponents)
                    {
                        if (component.Types.Contains("locality"))
                        {
                            return component.LongName;
                        }
                    }
                }
                return string.Empty;
            }
        }

        public sealed class Validator : AbstractValidator<GetWeatherRequest>
        {
            public const string LatitudeValidationError = "Latitude must be greater than 0";
            public const string LongitudeValidationError = "Longitude must be greater than 0";

            public Validator()
            {
                RuleFor(x => x.Latitude)
                    .GreaterThan(0)
                    .WithMessage(LatitudeValidationError);

                RuleFor(x => x.Longitude)
                    .GreaterThan(0)
                    .WithMessage(LongitudeValidationError);
            }
        }
    }
}
