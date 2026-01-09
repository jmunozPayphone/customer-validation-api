using Finance.Api.Abstractions.ProblemDetails;

namespace CustomerValidation.Api.Abstractions.ProblemDetails;

internal sealed class CommonProblemDetails : CustomProblemDetails
{
    public CommonProblemDetails(string requestPath,
                                string? code = null,
                                string? detail = null)
        : base("common-error", requestPath, StatusCodes.Status400BadRequest, code, detail)
    {
        Title = Resources.CommonProblemDetails_Title;
    }
}
