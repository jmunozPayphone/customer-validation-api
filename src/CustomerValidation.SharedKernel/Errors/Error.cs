using System.Collections.Immutable;
using System.Globalization;

namespace CustomerValidation.SharedKernel.Errors;

public record Error(string Code, string MessageTemplate)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "A null value was provided");

    public ImmutableArray<object> Parameters { get; protected set; } = [];

    public Error WithParameters(params object[] parameters)
    {
        Parameters = [.. parameters];
        return this;
    }

    public string Format(params object[] args) =>
        string.Format(CultureInfo.InvariantCulture, MessageTemplate, args);

    public override string ToString()
    {
        if (Parameters.Length == 0)
        {
            return $"[{Code}] {MessageTemplate}";
        }

        var formattedMessage = string.Format(CultureInfo.InvariantCulture, MessageTemplate, [.. Parameters]);
        return $"[{Code}] {formattedMessage}";
    }
}
