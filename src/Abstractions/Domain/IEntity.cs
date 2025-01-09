namespace Abstractions.Domain;

public interface IEntity
{
    IReadOnlyCollection<IDomainEvent> GetEvents();

    void ClearEvents();
}

public interface IEntity<TIdType> : IEntity
{
    TIdType Id { get; }

    void SetId(TIdType id);
}