namespace Abstractions.Domain;

public abstract class EntityBase<TIdType> : IEntity<TIdType>
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public virtual TIdType Id { get; protected set; }

    protected EntityBase(TIdType id)
    {
        Id = id;
    }

    protected EntityBase()
    {
    }

    public virtual void SetId(TIdType id)
    {
        ArgumentNullException.ThrowIfNull(id, nameof(id));
        Id = id;
    }

    public virtual IReadOnlyCollection<IDomainEvent> GetEvents() => _domainEvents.ToList();

    public virtual void ClearEvents() => _domainEvents.Clear();

    protected virtual void Publish(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public override bool Equals(object obj) => Equals(obj as EntityBase<TIdType>);

    protected bool Equals(EntityBase<TIdType> other) => other is not null && Id.Equals(other.Id);

    public override int GetHashCode() => (object)Id != (object)default(TIdType) ? Id.GetHashCode() : Guid.NewGuid().GetHashCode();

}