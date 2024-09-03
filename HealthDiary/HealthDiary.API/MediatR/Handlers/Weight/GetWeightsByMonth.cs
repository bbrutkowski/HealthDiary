using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WeightAlias = HealthDiary.API.Model.Main.Weight;

namespace HealthDiary.API.MediatR.Handlers.Weight
{
    public static class GetWeightsByMonth
    {
        public record GetWeightsByMonthRequest(int Id) : IRequest<Result<List<WeightDto>>>;

        public sealed class Handler : IRequestHandler<GetWeightsByMonthRequest, Result<List<WeightDto>>>
        {
            private readonly DataContext _context;
            private readonly IValidator<GetWeightsByMonthRequest> _requestValidator;

            public Handler(DataContext context, IValidator<GetWeightsByMonthRequest> requestValidator)
            {
                _context = context;
                _requestValidator = requestValidator;
            } 

            public async Task<Result<List<WeightDto>>> Handle(GetWeightsByMonthRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure<List<WeightDto>>(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var currentMonth = DateTime.Now.Month;
                var startOfMonth = new DateTime(DateTime.Now.Year, currentMonth, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);

                var weightsByMonth = await _context.Weights
                    .AsNoTracking()
                    .Where(x => x.UserId == request.Id && x.CreationDate >= startOfMonth && x.CreationDate <= endOfMonth)
                    .OrderBy(x => x.CreationDate)
                    .Select(x => new WeightDto
                    {
                        Value = x.Value,
                        CreationDate = x.CreationDate,
                    })
                    .ToListAsync(cancellationToken);

                return Result.Success(weightsByMonth);
            }
        }

        public sealed class Validator : AbstractValidator<GetWeightsByMonthRequest>
        {
            public const string UserIdValidation = "User Id must be greater than 0";

            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(UserIdValidation);
            }
        }
    }
}
