using CustomerValidation.Api.Abstractions.ProblemDetails;
using CustomerValidation.SharedKernel.Errors;
using CustomerValidation.SharedKernel.Guards;
using Microsoft.AspNetCore.Mvc;

namespace CustomerValidation.Api.Extensions;

public static class ControllerProblemDetailsExtensions
{
    public static ActionResult ToProblemDetails<T>(this T controller, Error error) where T : ControllerBase
    {
        Guard.ThrowIfNull(controller);
        Guard.ThrowIfNull(error);
        if (error is AggregateValidatorError aggregateValidatorError)
        {
            var errors = aggregateValidatorError.InnerErrors
                .GroupBy(ie => ie.Field)
                .ToDictionary(g => g.Key, g => g.Select(ie => new ValidationProblemDetail(ie.Code, ie.Message)).ToArray());
            var customProblem = new AggregateValidatorProblemDetails(controller.Request.Path, errors: errors);
            return controller.BadRequest(customProblem);
        }
        var problem = new CommonProblemDetails(controller.Request.Path, error.Code, error.MessageTemplate);
        return controller.BadRequest(problem);
    }
}
