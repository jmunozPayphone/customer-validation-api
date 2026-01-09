using CustomerValidation.SharedKernel.Guards;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerValidation.ApplicationCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationCore(this IServiceCollection services)
    {
        Guard.ThrowIfNull(services);
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
        return services;
    }
}
