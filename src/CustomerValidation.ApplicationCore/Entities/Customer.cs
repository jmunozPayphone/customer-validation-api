using CustomerValidation.ApplicationCore.Abstractions;

namespace CustomerValidation.ApplicationCore.Entities;

public class Customer : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Identification { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}
