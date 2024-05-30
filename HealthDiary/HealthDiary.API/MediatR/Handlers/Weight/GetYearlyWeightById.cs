using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Context.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static HealthDiary.API.MediatR.Handlers.Weight.GetWeightsByMonth;
using static HealthDiary.API.MediatR.Handlers.Weight.GetYearlyWeightById;

namespace HealthDiary.API.MediatR.Handlers.Weight
{
    public static class GetYearlyWeightById
    {
        public record GetYearlyWeightByIdRequest(int Id) : IRequest<OperationResult>;

        public sealed class Handler : IRequestHandler<GetYearlyWeightByIdRequest, OperationResult>
        {
            private readonly DataContext _context;
            private readonly IValidator<GetYearlyWeightByIdRequest> _requestValidator;

            public Handler(DataContext context, IValidator<GetYearlyWeightByIdRequest> requestValidator)
            {
                _context = context;
                _requestValidator = requestValidator;
            }

            public async Task<OperationResult> Handle(GetYearlyWeightByIdRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return OperationResultExtensions.Failure(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var currentMonth = DateTime.Now.Year;

                var weightsByMonth = await _context.Weights
                    .Where(x => x.UserId == request.Id && x.CreationDate.Month == currentMonth)
                    .OrderBy(x => x.CreationDate)
                    .ToListAsync(cancellationToken);

                return OperationResultExtensions.Success(weightsByMonth);
            }
        }
    }

    public sealed class Validator : AbstractValidator<GetYearlyWeightByIdRequest>
    {
        public const string UserIdValidation = "User Id must be greater than 0";

        public Validator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage(UserIdValidation);
        }
    }
}
