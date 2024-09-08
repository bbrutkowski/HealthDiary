using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Model.Main;
using MediatR;
using Newtonsoft.Json;

namespace HealthDiary.API.MediatR.Handlers.Localization
{
    public static class GetCity
    {
        public record GetCityRequest(decimal Latitude, decimal Longitude) : IRequest<Result<string>>;

        public sealed class Handler : IRequestHandler<GetCityRequest, Result<string>>
        {
            private readonly IHttpClientFactory _httpClientFactory;
            private readonly IValidator<GetCityRequest> _requestValidator;
            private HttpClient _httpClient;

            private const string _geoUrl = "https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&key={2}"; 
            private const string _geoKey = ""; 
            private const string _cityNameNotFoundError = "City name not found"; 
            private const string _deserializationError = "Error while deserializing data"; 

            public Handler(IHttpClientFactory httpClientFactory, IValidator<GetCityRequest> validator)
            {
                _httpClientFactory = httpClientFactory;
                _requestValidator = validator;
            }

            public async Task<Result<string>> Handle(GetCityRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure<string>(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var geoRequest = string.Format(_geoUrl, request.Latitude, request.Longitude, _geoKey);

                var response = await _httpClient.GetAsync(geoRequest, cancellationToken);
                if (!response.IsSuccessStatusCode) return Result.Failure<string>(response.StatusCode.ToString());

                var content = await response.Content.ReadAsStringAsync(cancellationToken);

                var geocodeResponse = JsonConvert.DeserializeObject<GeocodeResponseDto>(content);
                if (geocodeResponse is null) return Result.Failure<string>(_deserializationError);

                if (geocodeResponse.Status != "OK") return Result.Failure<string>(geocodeResponse.Status);

                var city = ExtractCityFromResults(geocodeResponse.Results);
                if (string.IsNullOrEmpty(city)) return Result.Failure<string>(_cityNameNotFoundError);

                return Result.Success(city);
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

        public sealed class Validator : AbstractValidator<GetCityRequest>
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
