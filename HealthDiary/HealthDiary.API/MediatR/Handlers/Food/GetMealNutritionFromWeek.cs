using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Context.Model;
using HealthDiary.API.Context.Model.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Food
{
    public static class GetMealNutritionFromWeek
    {
        public record GetMealNutritionFromWeekByUserIdRequest(int Id) : IRequest<OperationResult>;

        public sealed class Handler : IRequestHandler<GetMealNutritionFromWeekByUserIdRequest, OperationResult>
        {
            private readonly DataContext _context;

            public const string MealsNotFoundError = "No meals found";

            public Handler(DataContext context) => _context = context;

            public async Task<OperationResult> Handle(GetMealNutritionFromWeekByUserIdRequest request, CancellationToken cancellationToken)
            {
                DateTime today = DateTime.Now;;

                var weeklyMeals = await _context.Foods.Where(x => x.UserId == request.Id && x.CreationDate >= today.AddDays(-7) && x.CreationDate <= today)
                    .ToArrayAsync(cancellationToken);

                if(!weeklyMeals.Any()) return OperationResultExtensions.Failure(MealsNotFoundError);

                var weeklyNutritionInfo = new WeeklyNutritionDto()
                {
                    Kcal = weeklyMeals.Sum(x => x.Kcal),
                    Protein = weeklyMeals.Sum(x => x.Protein),
                    Fat = weeklyMeals.Sum(x => x.Fat),
                    Carbohydrates = weeklyMeals.Sum(x => x.Carbohydrates)
                };

                return OperationResultExtensions.Success(weeklyNutritionInfo);
            }
        }
    }
}
