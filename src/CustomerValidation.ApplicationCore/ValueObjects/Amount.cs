using CustomerValidation.ApplicationCore.Abstractions;

namespace CustomerValidation.ApplicationCore.ValueObjects;

public sealed class Amount : ValueObject
{
    public decimal Value { get; private set; }

    public Amount(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Transaction amount cannot be negative.", nameof(amount));
        }

        Value = amount;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
