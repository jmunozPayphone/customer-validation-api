namespace CustomerValidation.Infrastructure.Options;

internal sealed class CreditScoreProvider
{
    public string BaseAddress { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}
