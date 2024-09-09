﻿using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Model.DTO;
using HealthDiary.API.Model.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HealthDiary.API.MediatR.Handlers.Weather
{
    public static class GetWeather 
    {
        public record GetWeatherRequest(decimal Latitude, decimal Longitude) : IRequest<Result<string>>;

        public sealed class Handler : IRequestHandler<GetWeatherRequest, Result<string>>
        {
            private readonly IHttpClientFactory _httpClientFactory;
            private readonly IValidator<GetWeatherRequest> _requestValidator;
            private HttpClient _httpClient;

            private const string _geoUrl = "https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&key={2}";
            private const string _geoKey = "AIzaSyAgo_hpgiUbiblsUvIBr-jAEDEsNUBB3tw";

            private const string _weatherUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";
            private const string _weatherApiKey = "d237302eb608500a27d0a2a5fa1e4a82";

            private const string _cityNameNotFoundError = "City name not found";
            private const string _deserializationError = "Error while deserializing data";

            public Handler(IHttpClientFactory httpClientFactory, IValidator<GetWeatherRequest> validator)
            {
                _httpClientFactory = httpClientFactory;
                _requestValidator = validator;
            }

            public async Task<Result<string>> Handle(GetWeatherRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure<string>(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var geoRequest = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    _geoUrl, request.Latitude.ToString("F6", System.Globalization.CultureInfo.InvariantCulture),
                    request.Longitude.ToString("F6", System.Globalization.CultureInfo.InvariantCulture),
                    _geoKey);

                CreateClient();

                var response = await _httpClient.GetAsync(geoRequest, cancellationToken);
                if (!response.IsSuccessStatusCode) return Result.Failure<string>(response.StatusCode.ToString());

                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                var geocodeResponse = JsonConvert.DeserializeObject<GeocodeResponseDto>(content);
                if (geocodeResponse is null) return Result.Failure<string>(_deserializationError);

                if (geocodeResponse.Status != "OK") return Result.Failure<string>(geocodeResponse.Status);

                var city = ExtractCityFromResults(geocodeResponse.Results);
                if (string.IsNullOrEmpty(city)) return Result.Failure<string>(_cityNameNotFoundError);

                var weatherResult = await GetWeatherConditions(city, cancellationToken);
                if (weatherResult.IsFailure) return Result.Failure<string>(weatherResult.Error);

                return Result.Success(weatherResult.Value.ToString());
            }

            private async Task<Result<WeatherResponseDto>> GetWeatherConditions(string city, CancellationToken cancellationToken)
            {
                var weatherRequest = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    _weatherUrl, city, _weatherApiKey);

                var weatherResponse = await _httpClient.GetAsync(weatherRequest, cancellationToken);
                if (!weatherResponse.IsSuccessStatusCode) return Result.Failure<WeatherResponseDto>(weatherResponse.StatusCode.ToString());

                var weatherContent = await weatherResponse.Content.ReadAsStringAsync(cancellationToken);

                var weatherData = JsonConvert.DeserializeObject<WeatherResponseDto>(weatherContent);
                if (weatherData is null) return Result.Failure<WeatherResponseDto>(_deserializationError);

                return Result.Success(weatherData);
            }

            private HttpClient CreateClient()
            {
                _httpClient ??= _httpClientFactory.CreateClient();

                return _httpClient;
            }

            private string ExtractCityFromResults(GeocodeResultDto[] results)
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
