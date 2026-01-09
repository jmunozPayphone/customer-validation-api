using CustomerValidation.ApplicationCore.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CustomerValidation.Api.Extensions;

public static class ControllerResultExtensions
{
    /// <summary>
    /// Returns an OK (200) result if the result is successful, or handles the error by returning a BadRequest if necessary.
    /// </summary>
    /// <typeparam name="T">The type of the resulting entity.</typeparam>
    /// <param name="controller">The controller from which the method is invoked.</param>
    /// <param name="result">The result containing the value or error.</param>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    public static IActionResult OkFromResult<T>(this ControllerBase controller, Result<T> result)
    {
        // Validate that parameters are not null
        ArgumentNullException.ThrowIfNull(controller);
        ArgumentNullException.ThrowIfNull(result);
        return result.IsSuccess
            ? new OkObjectResult(result.Value) // OK if operation was successful
            : controller.ToProblemDetails(result.Error); // BadRequest if there was an error
    }

    /// <summary>
    /// Returns an OK (200) result with the mapped value if the operation is successful, or handles the error by returning a BadRequest if necessary.
    /// </summary>
    /// <typeparam name="TResult">The type of the original result.</typeparam>
    /// <typeparam name="TValue">The type of the mapped value.</typeparam>
    /// <param name="controller">The controller from which the method is invoked.</param>
    /// <param name="result">The result containing the value or error.</param>
    /// <param name="mapper">Function to map the result value to a new value.</param>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    public static IActionResult OkFromResult<TResult, TValue>(this ControllerBase controller, Result<TResult> result, Func<TResult, TValue> mapper)
    {
        // Validate that parameters are not null
        ArgumentNullException.ThrowIfNull(controller);
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(mapper);
        return result.IsSuccess
            ? new OkObjectResult(mapper(result.Value)) // OK with mapped value if operation was successful
            : controller.ToProblemDetails(result.Error); // BadRequest if there was an error
    }

    /// <summary>
    /// Returns a NoContent (204) result if the operation is successful, or handles the error by returning a BadRequest if necessary.
    /// </summary>
    /// <param name="controller">The controller from which the method is invoked.</param>
    /// <param name="result">The result containing the status of the operation.</param>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    public static IActionResult NoContentFromResult(this ControllerBase controller, Result result)
    {
        // Validate that parameters are not null
        ArgumentNullException.ThrowIfNull(controller);
        ArgumentNullException.ThrowIfNull(result);
        return result.IsSuccess
            ? controller.NoContent() // NoContent if operation was successful
            : controller.ToProblemDetails(result.Error); // BadRequest if there was an error
    }

    /// <summary>
    /// Returns a Created (201) result if the operation is successful, or handles the error by returning a BadRequest if necessary.
    /// </summary>
    /// <typeparam name="T">The type of the resulting entity.</typeparam>
    /// <param name="controller">The controller from which the method is invoked.</param>
    /// <param name="result">The result containing the value or error.</param>
    /// <returns>An IActionResult representing the result of the operation.</returns>
    public static IActionResult CreatedFromResult<T>(this ControllerBase controller, Result<T> result)
    {
        // Validate that parameters are not null
        ArgumentNullException.ThrowIfNull(controller);
        ArgumentNullException.ThrowIfNull(result);
        return result.IsSuccess
            ? controller.StatusCode(201, result.Value) // Created (201) if operation was successful
            : controller.ToProblemDetails(result.Error); // BadRequest if there was an error
    }
}
