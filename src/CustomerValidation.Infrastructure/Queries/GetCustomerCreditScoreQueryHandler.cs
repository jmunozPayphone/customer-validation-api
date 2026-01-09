using CustomerValidation.ApplicationCore.Features.Customers.Queries;
using CustomerValidation.Infrastructure.Services;
using CustomerValidation.SharedKernel.Guards;
using MediatR;

namespace CustomerValidation.Infrastructure.Queries;

internal sealed class GetCustomerCreditScoreQueryHandler : IRequestHandler<GetCustomerCreditScoreQuery, GetCustomerCreditScoreResult>
{
    private readonly CreditScoreProviderService _scoreproviderService;

    public GetCustomerCreditScoreQueryHandler(CreditScoreProviderService scoreProviderService)
    {
        Guard.ThrowIfNull(scoreProviderService);
        _scoreproviderService = scoreProviderService;
    }

    public async Task<GetCustomerCreditScoreResult> Handle(GetCustomerCreditScoreQuery request, CancellationToken cancellationToken)
    {
        Guard.ThrowIfNull(request);
        var scoreRequest = new CreditScoreRequest(request.Name, request.Document);
        var scoreResult = await _scoreproviderService.GetAsync(scoreRequest);
        return new GetCustomerCreditScoreResult(scoreResult.Score);
    }
}
