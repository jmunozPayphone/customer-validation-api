using MediatR;

namespace CustomerValidation.ApplicationCore.Features.Customers.Queries;

public record GetCustomerCreditScoreQuery(string Name, string Document): IRequest<GetCustomerCreditScoreResult>;

public record GetCustomerCreditScoreResult(int Score);