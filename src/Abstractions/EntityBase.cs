namespace Abstractions;

public interface IEntity<TIdType> : IEntity
{
    TIdType Id { get; }

    void SetId(TIdType id);
}

public interface IEntity
{
    IReadOnlyCollection<DomainEventBase> GetEvents();

    void ClearEvents();
}

public interface IDomainEvent
{
}

public abstract class DomainEventBase : IDomainEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

public abstract class EntityBase<TIdType> : IEntity<TIdType>
{
    private readonly List<DomainEventBase> _domainEvents = new();

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

    protected abstract void Validate();

    public virtual IReadOnlyCollection<DomainEventBase> GetEvents() => _domainEvents.ToList();

    public virtual void ClearEvents() => _domainEvents.Clear();

    protected virtual void Publish(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);

    public override bool Equals(object obj) => Equals(obj as EntityBase<TIdType>);

    protected bool Equals(EntityBase<TIdType> other) => other is not null && Id.Equals(other.Id);

    public override int GetHashCode() => (object)Id != (object)default(TIdType) ? Id.GetHashCode() : Guid.NewGuid().GetHashCode();
}