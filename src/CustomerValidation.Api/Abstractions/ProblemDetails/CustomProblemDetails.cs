using MvcProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;
using System.Diagnostics;

namespace CustomerValidation.Api.Abstractions.ProblemDetails;

public record ValidationProblemDetail(string? Code, string? Detail);

internal class CustomProblemDetails : MvcProblemDetails
{
    protected CustomProblemDetails(
        string errorType,
        string requestPath,
        int status,
        string? code = null,
        string? detail = null)
    {
        Type = $"https://example.com/{errorType}";
        Instance = requestPath;
        Status = status;

        Extensions = new Dictionary<string, object?>
        {
            ["timestamp"] = DateTime.UtcNow,
            ["traceId"] = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString()
        };

        if (!string.IsNullOrEmpty(code))
        {
            Extensions["code"] = code;
            if (!string.IsNullOrEmpty(detail))
            {
                Detail = detail;
            }
        }
    }

    protected CustomProblemDetails(
        string errorType,
        string requestPath,
        int status,
        string? code = null,
        string? detail = null,
        Dictionary<string, ValidationProblemDetail[]>? errors = null)
        : this(errorType, requestPath, status, code, detail)
    {
        if (errors?.Count > 0)
        {
            Extensions["errors"] = errors;
        }
    }

    protected CustomProblemDetails(
        string errorType,
        string requestPath,
        int status,
        string? code = null,
        string? detail = null,
        ValidationProblemDetail[]? errors = null)
        : this(errorType, requestPath, status, code, detail)
    {
        if (errors?.Length > 0)
        {
            Extensions["errors"] = errors;
        }
    }
}
