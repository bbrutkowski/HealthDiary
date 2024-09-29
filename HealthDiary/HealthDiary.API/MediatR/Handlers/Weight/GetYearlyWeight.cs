using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static HealthDiary.API.MediatR.Handlers.Weight.GetYearlyWeight;

namespace HealthDiary.API.MediatR.Handlers.Weight
{
    public static class GetYearlyWeight
    {
        public record GetYearlyWeightRequest(int Id) : IRequest<Result<List<WeightDto>>>;

        public sealed class Handler : IRequestHandler<GetYearlyWeightRequest, Result<List<WeightDto>>>
        {
            private readonly DataContext _context;
            private readonly IValidator<GetYearlyWeightRequest> _requestValidator;

            public Handler(DataContext context, IValidator<GetYearlyWeightRequest> requestValidator)
            {
                _context = context;
                _requestValidator = requestValidator;
            }

            public async Task<Result<List<WeightDto>>> Handle(GetYearlyWeightRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure<List<WeightDto>>(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var currentYear = DateTime.Now.Year;

                var weightsByMonth = await _context.Weights
                    .Where(x => x.UserId == request.Id && x.CreationDate.Year == currentYear)
                    .OrderBy(x => x.CreationDate)
                    .Select(x => new WeightDto
                    {
                        CreationDate = x.CreationDate,
                        Value = x.Value
                    })
                    .ToListAsync(cancellationToken);

                return Result.Success(weightsByMonth);
            }
        }
    }

    public sealed class Validator : AbstractValidator<GetYearlyWeightRequest>
    {
        public const string UserIdValidation = "User Id must be greater than 0";

        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage(UserIdValidation);
        }
    }
}
