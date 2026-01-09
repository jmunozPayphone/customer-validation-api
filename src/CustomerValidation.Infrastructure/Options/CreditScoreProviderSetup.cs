using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CustomerValidation.Infrastructure.Options;

internal sealed class CreditScoreProviderSetup : IConfigureOptions<CreditScoreProvider>
{
    private const string CreditScoreProviderSection = "CreditScoreProvider";
    private readonly IConfiguration _configuration;
    public CreditScoreProviderSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void Configure(CreditScoreProvider options)
    {
        _configuration.GetSection(CreditScoreProviderSection).Bind(options);
    }
}
