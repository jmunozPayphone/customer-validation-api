using CustomerValidation.ApplicationCore.Abstractions;
using CustomerValidation.ApplicationCore.Enums;
using CustomerValidation.ApplicationCore.Features.Customers.Queries;
using CustomerValidation.ApplicationCore.ValueObjects;
using CustomerValidation.SharedKernel.Guards;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerValidation.ApplicationCore.Features.Customers.Commands;

internal sealed class AssessCustomerRiskCommandHandler : IRequestHandler<AssessCustomerRiskCommand, Result<AssessCustomerRiskResponse>>
{
    private readonly IMediator _mediator;
    private readonly ILogger<AssessCustomerRiskCommandHandler> _logger;

    public AssessCustomerRiskCommandHandler(
        IMediator mediator,
        ILogger<AssessCustomerRiskCommandHandler> logger)
    {
        Guard.ThrowIfNull(mediator);
        Guard.ThrowIfNull(logger);
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<Result<AssessCustomerRiskResponse>> Handle(AssessCustomerRiskCommand request, CancellationToken cancellationToken)
    {
        var customerScore = await _mediator.Send(new GetCustomerCreditScoreQuery(
            request.Name,
            request.DocumentNumber.Value), cancellationToken);
        _logger.LogWarning("Assessed risk for customer {Name} with document {Document}. Credit Score: {Score}, Transaction Amount: {Amount}",
            request.Name,
            request.DocumentNumber.Value,
            customerScore,
            request.TxAmount.Value);

        if (customerScore == null)
        {
            return Result.Success(new AssessCustomerRiskResponse(nameof(CustomerScoreStatus.Rejected)));
        }

        var score = customerScore.Score;

        if (score < 500)
        {
            _logger.LogWarning("Customer {Name} with document {Document} has a low credit score of {Score}. Automatically rejected.",
                request.Name,
                request.DocumentNumber.Value,
                score);
            return Result.Success(new AssessCustomerRiskResponse(nameof(CustomerScoreStatus.Rejected)));
        }

        if (score <= 699)
        {
            _logger.LogWarning("Customer {Name} with document {Document} has a medium credit score of {Score}. Evaluating transaction amount {Amount}.",
                request.Name,
                request.DocumentNumber.Value,
                score,
                request.TxAmount.Value);
            return request.TxAmount.Value < 1000m
                ? Result.Success(new AssessCustomerRiskResponse(nameof(CustomerScoreStatus.Approved)))
                : Result.Success(new AssessCustomerRiskResponse(nameof(CustomerScoreStatus.Rejected)));
        }

        _logger.LogInformation("Customer {Name} with document {Document} has a high credit score of {Score}. Automatically approved.",
            request.Name,
            request.DocumentNumber.Value,
            score);
        return Result.Success(new AssessCustomerRiskResponse(nameof(CustomerScoreStatus.Approved)));
    }
}
