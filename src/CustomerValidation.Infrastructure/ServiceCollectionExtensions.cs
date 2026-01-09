using CustomerValidation.Infrastructure.Options;
using CustomerValidation.Infrastructure.Services;
using CustomerValidation.Infrastructure.Utils;
using CustomerValidation.SharedKernel.Guards;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CustomerValidation.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        Guard.ThrowIfNull(services);
        Guard.ThrowIfNull(configuration);

        return services
            .AddMediatR()
            .AddOptions()
            .AddHttpClientFactory(configuration)
            .AddServices();
    }

    private static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
        return services;
    }

    private static IServiceCollection AddHttpClientFactory(this IServiceCollection services, IConfiguration configuration)
    {
        Guard.ThrowIfNull(services);
        Guard.ThrowIfNull(configuration);

        var clientName = Constants.CreditScoreHttpClient;
        services.AddHttpClient(clientName, (sp, http) =>
        {
            var options = sp.GetRequiredService<IOptions<CreditScoreProvider>>().Value;
            var url = options.BaseAddress;
            Guard.ThrowIfNull(url, $"{clientName} URL");
            http.BaseAddress = new Uri(url);
            http.DefaultRequestHeaders.Add("x-api-key", options.ApiKey);
            http.Timeout = TimeSpan.FromSeconds(10);
        });

        return services;
    }

    private static IServiceCollection AddOptions(this IServiceCollection services)
    {
        Guard.ThrowIfNull(services);
        services.ConfigureOptions<CreditScoreProviderSetup>();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<CreditScoreProviderService>();
        return services;
    }
}
