using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Food
{
    public static class GetLastMealInformation
    {
        public record GetMealInfoRequest(int Id) : IRequest<Result>;

        public sealed class Handler : IRequestHandler<GetMealInfoRequest, Result>
        {
            private readonly DataContext _context;

            public const string MealInformationNotFound = "Meal information not found";

            public Handler(DataContext context) => _context = context;

            public async Task<Result> Handle(GetMealInfoRequest request, CancellationToken cancellationToken)
            {            
                var lastMeal = await _context.Foods
                    .AsNoTracking()
                    .Where(x => x.UserId == request.Id)
                    .OrderByDescending(x => x.CreationDate)
                    .Select(x => new MealDto
                    {
                        Name = x.Name,
                        Protein = x.Protein,
                        Fat = x.Fat,
                        Carbohydrates = x.Carbohydrates,
                        Kcal = x.Kcal,
                        LastUpdate = x.CreationDate
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (lastMeal is null) return Result.Failure(MealInformationNotFound);

                return Result.Success(lastMeal);
            }
        }      
    }
}
