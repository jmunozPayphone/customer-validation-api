using CustomerValidation.Infrastructure.Utils;
using CustomerValidation.SharedKernel.Guards;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace CustomerValidation.Infrastructure.Services;

public record class CreditScoreRequest(string Name, string Document);
public record CreditScoreResponse(int Score);

internal sealed class CreditScoreProviderService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CreditScoreProviderService> _logger;

    public CreditScoreProviderService(IHttpClientFactory httpClientFactory, ILogger<CreditScoreProviderService> logger)
    {
        Guard.ThrowIfNull(httpClientFactory);
        Guard.ThrowIfNull(logger);
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<CreditScoreResponse> GetAsync(CreditScoreRequest request)
    {
        Guard.ThrowIfNull(request);
        var url = "/api/users";
        var httpRequest = new
        {
            Name = request.Name,
            Job = request.Document
        };

        try
        {
            var jsonContent = JsonConvert.SerializeObject(httpRequest);
            using var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var httpClient = _httpClientFactory.CreateClient(Constants.CreditScoreHttpClient);
            var httpResponse = await httpClient.PostAsync(url, httpContent);
            if (!httpResponse.IsSuccessStatusCode)
            {
                return new CreditScoreResponse(0);
            }

            var responseContent = await httpResponse.Content.ReadAsStringAsync();
            var responseModel = JsonConvert.DeserializeObject<UserData>(responseContent);
            Guard.ThrowIfNull(responseModel);
            return new CreditScoreResponse(GenerateScore(responseModel));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting credit score for Name: {Name}, Document: {Document}", request.Name, request.Document);
            return new CreditScoreResponse(0);
        }
    }

    private static int GenerateScore(UserData data)
    {
        // Crear string único del usuario
        var userString = $"{data.Id}|{data.Email}|{data.FirstName}|{data.LastName}";

        // Generar hash SHA256
        using var sha256 = SHA256.Create();
        Guard.ThrowIfNull(sha256);
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(userString));

        // Convertir primeros 4 bytes a int
        var hashInt = Math.Abs(BitConverter.ToInt32(hashBytes, 0));

        const int minScore = 400;
        const int maxScore = 900;

        // Mapear al rango deseado
        return (hashInt % (maxScore - minScore + 1)) + minScore;
    }

    private record UserData(int Id, string Email, string FirstName, string LastName, string Avatar);
}
