using FluentValidation;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Context.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthDiary.API.MediatR.Handlers.Weight
{
    public static class GetWeights
    {
        public record GetWeightsRequest(int Id) : IRequest<OperationResult>;

        public sealed class Handler : IRequestHandler<GetWeightsRequest, OperationResult>
        {
            private readonly DataContext _context;
            private readonly IValidator<GetWeightsRequest> _requestValidator;

            public Handler(DataContext context, IValidator<GetWeightsRequest> validator)
            {
                _context = context;
                _requestValidator = validator;
            }

            public async Task<OperationResult> Handle(GetWeightsRequest request, CancellationToken cancellationToken)
            {
                var requestValidationResult = await _requestValidator.ValidateAsync(request, cancellationToken);
                if (!requestValidationResult.IsValid) return OperationResultExtensions.Failure(string.Join(Environment.NewLine, requestValidationResult.Errors));

                var userWeights = await _context.Weights.Where(x => x.UserId == request.Id)
                    .ToListAsync(cancellationToken);

                return OperationResultExtensions.Success(userWeights);
            }
        }

        public sealed class Validator : AbstractValidator<GetWeightsRequest>
        {
            public const string UserIdValidation = "User Id must be greater than 0";

            public Validator()
            {
                RuleFor(x => x.Id).GreaterThan(0).WithMessage(UserIdValidation);
            }
        }
    }
}
