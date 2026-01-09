namespace CustomerValidation.ApplicationCore.Abstractions;

public abstract class Entity<TEntityId> : IEntity<TEntityId> where TEntityId : IEquatable<TEntityId>
{
    protected Entity()
    {
    }

    protected Entity(TEntityId id) => Id = id;

    public TEntityId Id { get; init; } = default!;

}
