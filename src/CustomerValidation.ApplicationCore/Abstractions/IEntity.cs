namespace CustomerValidation.ApplicationCore.Abstractions;

public interface IEntity<TEntityId> where TEntityId : IEquatable<TEntityId>
{
    TEntityId Id { get; }
}
