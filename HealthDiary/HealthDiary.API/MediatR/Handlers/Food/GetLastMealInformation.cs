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
            private readonly IValidator<GetMealInfoRequest> _requestValidator;

            public const string MealInformationNotFound = "Meal information not found";

            public Handler(DataContext context, IValidator<GetMealInfoRequest> requestValidator)
            {
                _context = context;
                _requestValidator = requestValidator;
            }

            public async Task<Result> Handle(GetMealInfoRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var lastMeal = await _context.Foods
                    .Where(x => x.UserId == request.Id)
                    .OrderByDescending(x => x.CreationDate)
                    .FirstOrDefaultAsync(cancellationToken);

                if (lastMeal is null) return Result.Failure(MealInformationNotFound);

                var lastMealInfo = new MealDto()
                {
                    Name = lastMeal.Name,
                    Protein = lastMeal.Protein,
                    Fat = lastMeal.Fat,
                    Carbohydrates = lastMeal.Carbohydrates,
                    Kcal = lastMeal.Kcal,
                    LastUpdate = lastMeal.CreationDate
                };

                return Result.Success(lastMealInfo);
            }
        }

        public sealed class Validator : AbstractValidator<GetMealInfoRequest>
        {
            public const string UserIdValidation = "User Id must be greater than 0";

            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(UserIdValidation);
            }
        }
    }
}
