using CustomerValidation.ApplicationCore.Abstractions;

namespace CustomerValidation.ApplicationCore.ValueObjects;

public class Document : ValueObject
{
    public string Value { get; private set; }

    public Document(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException("Document number cannot be null or empty.", nameof(number));
        }

        Value = number;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
