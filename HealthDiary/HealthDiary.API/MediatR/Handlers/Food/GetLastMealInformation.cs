using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Context.Model;
using HealthDiary.API.Context.Model.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Food
{
    public static class GetLastMealInformation
    {
        public record GetLastMealInformationByUserIdRequest(int Id) : IRequest<OperationResult>;

        public sealed class Handler : IRequestHandler<GetLastMealInformationByUserIdRequest, OperationResult>
        {
            private readonly DataContext _context;
            private readonly IValidator<GetLastMealInformationByUserIdRequest> _requestValidator;

            public const string MealInformationNotFound = "Meal information not found";

            public Handler(DataContext context, IValidator<GetLastMealInformationByUserIdRequest> requestValidator)
            {
                _context = context;
                _requestValidator = requestValidator;
            }

            public async Task<OperationResult> Handle(GetLastMealInformationByUserIdRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return OperationResultExtensions.Failure(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var lastMeal = await _context.Foods
                    .Where(x => x.UserId == request.Id)
                    .OrderByDescending(x => x.CreationDate)
                    .FirstOrDefaultAsync(cancellationToken);

                if (lastMeal is null) return OperationResultExtensions.Failure(MealInformationNotFound);

                var lastMealInfo = new MealDto()
                {
                    Name = lastMeal.Name,
                    Protein = lastMeal.Protein,
                    Fat = lastMeal.Fat,
                    Carbohydrates = lastMeal.Carbohydrates,
                    Kcal = lastMeal.Kcal,
                    LastUpdate = lastMeal.CreationDate
                };

                return OperationResultExtensions.Success(lastMealInfo);
            }
        }

        public sealed class Validator : AbstractValidator<GetLastMealInformationByUserIdRequest>
        {
            public const string UserIdValidation = "User Id must be greater than 0";

            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(UserIdValidation);
            }
        }
    }
}
