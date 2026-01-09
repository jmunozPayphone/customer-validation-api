using CustomerValidation.Api.Abstractions.ProblemDetails;

namespace CustomerValidation.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError, new InternalServerErrorProblemDetails(context.Request.Path));
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, int statusCode, object problemDetails)
    {
        _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}
