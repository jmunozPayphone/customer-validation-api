using CustomerValidation.ApplicationCore.Repositories;

namespace CustomerValidation.ApplicationCore.Abstractions;

public interface IUnitOfWork : IDisposable
{
    ICustomerRepository CustomerRepository { get; }
    Task<int> SaveAsync();
}
