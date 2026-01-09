using Finance.Api.Abstractions.ProblemDetails;

namespace CustomerValidation.Api.Abstractions.ProblemDetails;

internal sealed class InternalServerErrorProblemDetails : CustomProblemDetails
{
    public InternalServerErrorProblemDetails(string requestPath, string? code = null, string? detail = null)
        : base("internal-server-error", requestPath, StatusCodes.Status500InternalServerError, code, detail)
    {
        Title = Resources.InternalServerErrorProblemDetails_Title;
    }
}
