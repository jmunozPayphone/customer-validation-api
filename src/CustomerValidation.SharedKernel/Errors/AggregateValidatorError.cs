namespace CustomerValidation.SharedKernel.Errors;

public record FieldError(string Field, string Code, string Message) : Error(Code, Message)
{
    public override string ToString() => $"{Field}: {Message}";
}

public record AggregateValidatorError(string Code = nameof(AggregateValidatorError), string Message = "") : Error(Code, Message)
{
    public IReadOnlyList<FieldError> InnerErrors => _innerErrors;
    private readonly List<FieldError> _innerErrors = [];

    public void AddError(FieldError error)
    {
        ArgumentNullException.ThrowIfNull(error);
        _innerErrors.Add(error);
    }

    public void AddErrors(IEnumerable<FieldError> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);
        _innerErrors.AddRange(errors);
    }

    public override string ToString()
    {
        if (_innerErrors.Count == 0)
        {
            return $"[{Code}] {Message}";
        }

        var errors = string.Join("; ", _innerErrors.Select(e => e.ToString()));
        return $"[{Code}] {errors}";
    }
}
