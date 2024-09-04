using CSharpFunctionalExtensions;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Food
{
    public static class GetMealNutritionFromWeek
    {
        public record GetNutritionInfoRequest(int Id) : IRequest<Result<WeeklyNutritionDto>>;

        public sealed class Handler : IRequestHandler<GetNutritionInfoRequest, Result<WeeklyNutritionDto>>
        {
            private readonly DataContext _context;

            public const string MealsNotFoundError = "No meals found";

            public Handler(DataContext context) => _context = context;

            public async Task<Result<WeeklyNutritionDto>> Handle(GetNutritionInfoRequest request, CancellationToken cancellationToken)
            {
                var today = DateTime.Now;
                var sevenDaysAgo = today.AddDays(-7); 

                var weeklyNutritionInfo = await _context.Foods
                    .AsNoTracking() 
                    .Where(x => x.UserId == request.Id && x.CreationDate >= sevenDaysAgo && x.CreationDate <= today)
                    .Select(x => new WeeklyNutritionDto
                    {
                        Kcal = x.Kcal,
                        Protein = x.Protein,
                        Fat = x.Fat,
                        Carbohydrates = x.Carbohydrates
                    })
                    .ToListAsync(cancellationToken);

                if (!weeklyNutritionInfo.Any()) return Result.Failure<WeeklyNutritionDto>(MealsNotFoundError);

                var totalNutritionInfo = new WeeklyNutritionDto
                {
                    Kcal = weeklyNutritionInfo.Sum(x => x.Kcal),
                    Protein = weeklyNutritionInfo.Sum(x => x.Protein),
                    Fat = weeklyNutritionInfo.Sum(x => x.Fat),
                    Carbohydrates = weeklyNutritionInfo.Sum(x => x.Carbohydrates)
                };

                return Result.Success(totalNutritionInfo);
            }
        }
    }
}
