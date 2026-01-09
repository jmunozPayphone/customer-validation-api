using CustomerValidation.ApplicationCore.Abstractions;
using CustomerValidation.ApplicationCore.ValueObjects;
using MediatR;

namespace CustomerValidation.ApplicationCore.Features.Customers.Commands;

public record AssessCustomerRiskCommand(string Name, Document DocumentNumber, Amount TxAmount) : IRequest<Result<AssessCustomerRiskResponse>>;

public record AssessCustomerRiskResponse(string Status);