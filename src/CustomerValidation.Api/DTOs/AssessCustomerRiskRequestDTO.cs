using CustomerValidation.ApplicationCore.Features.Customers.Commands;
using CustomerValidation.ApplicationCore.ValueObjects;

namespace CustomerValidation.Api.DTOs;

public class AssessCustomerRiskRequestDTO
{
    public string Name { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public decimal TransactionAmount { get; set; }

    public AssessCustomerRiskCommand ToCommand()
    {
        return new AssessCustomerRiskCommand(
            Name,
            new Document(DocumentNumber),
            new Amount(TransactionAmount)
        );
    }
}
