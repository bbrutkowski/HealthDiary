using CSharpFunctionalExtensions;
using FluentValidation;
using HealthDiary.API.Context.DataContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Weight
{
    public static class GetWeightsByMonth
    {
        public record GetWeightsByMonthRequest(int Id) : IRequest<Result>;

        public sealed class Handler : IRequestHandler<GetWeightsByMonthRequest, Result>
        {
            private readonly DataContext _context;
            private readonly IValidator<GetWeightsByMonthRequest> _requestValidator;

            public Handler(DataContext context, IValidator<GetWeightsByMonthRequest> requestValidator)
            {
                _context = context;
                _requestValidator = requestValidator;
            } 

            public async Task<Result> Handle(GetWeightsByMonthRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return Result.Failure(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var currentMonth = DateTime.Now.Month;

                var weightsByMonth = await _context.Weights
                    .Where(x => x.UserId == request.Id && x.CreationDate.Month == currentMonth)
                    .OrderBy(x => x.CreationDate)
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
