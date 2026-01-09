using Finance.Api.Abstractions.ProblemDetails;

namespace CustomerValidation.Api.Abstractions.ProblemDetails;

internal sealed class AggregateValidatorProblemDetails : CustomProblemDetails
{
    public AggregateValidatorProblemDetails(string requestPath,
                                            string? code = null,
                                            string? detail = null,
                                            Dictionary<string, ValidationProblemDetail[]>? errors = null)
        : base("aggregate-validator-error", requestPath, StatusCodes.Status400BadRequest, code, detail, errors)
    {
        Title = Resources.AggregateProblemDetails_Title;
        ;
    }
}
