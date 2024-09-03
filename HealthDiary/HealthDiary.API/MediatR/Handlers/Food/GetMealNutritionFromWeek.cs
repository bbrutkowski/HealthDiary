using CSharpFunctionalExtensions;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Food
{
    public static class GetMealNutritionFromWeek
    {
        public record GetNutritionInfoRequest(int Id) : IRequest<Result>;

        public sealed class Handler : IRequestHandler<GetNutritionInfoRequest, Result>
        {
            private readonly DataContext _context;

            public const string MealsNotFoundError = "No meals found";

            public Handler(DataContext context) => _context = context;

            public async Task<Result> Handle(GetNutritionInfoRequest request, CancellationToken cancellationToken)
            {
                DateTime today = DateTime.Now;;

                var weeklyMeals = await _context.Foods.Where(x => x.UserId == request.Id && x.CreationDate >= today.AddDays(-7) && x.CreationDate <= today)
                    .ToArrayAsync(cancellationToken);

                if(!weeklyMeals.Any()) return Result.Failure(MealsNotFoundError);

                var weeklyNutritionInfo = new WeeklyNutritionDto()
                {
                    Kcal = weeklyMeals.Sum(x => x.Kcal),
                    Protein = weeklyMeals.Sum(x => x.Protein),
                    Fat = weeklyMeals.Sum(x => x.Fat),
                    Carbohydrates = weeklyMeals.Sum(x => x.Carbohydrates)
                };

                return Result.Success(weeklyNutritionInfo);
            }
        }
    }
}
