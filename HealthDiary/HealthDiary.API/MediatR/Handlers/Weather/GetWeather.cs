﻿using CSharpFunctionalExtensions;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Context.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Weather
{
    public static class GetWeather 
    {
        public record GetWeatherRequest() : IRequest<Result>;

        public sealed class Handler : IRequestHandler<GetWeatherRequest, Result>
        {
            private readonly DataContext _context;

            public Handler(DataContext context) => _context = context;

            private const string ContentNotFoundError = "No forecast with given Id";
            public async Task<Result> Handle(GetWeatherRequest request, CancellationToken cancellationToken)
            {
                var randomId = new Random().Next(1, 6);

                var weatherContent = await _context.WeatherInformations
                    .Where(x => x.Id == randomId && x.IsActive)
                    .Select(x => x.Content)
                    .FirstOrDefaultAsync(cancellationToken);

                if (weatherContent is null) return Result.Failure(ContentNotFoundError + $"{randomId}");

                return Result.Success(weatherContent);
            }
        }       
    }
}
