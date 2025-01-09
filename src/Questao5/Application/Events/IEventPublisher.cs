using Abstractions.Domain;
using MediatR;

namespace Questao5.Application.Events;

public interface IEventPublisher
{
    Task PublishAsync<T>(EntityBase<T> dominio);
}

public sealed class EventPublisher : IEventPublisher
{
    private readonly IPublisher _publisher;

    public EventPublisher(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task PublishAsync<T>(EntityBase<T> dominio)
    {
        if (dominio.GetEvents() is { Count: 0 })
            return;

        foreach (var evento in dominio.GetEvents())
            await _publisher.Publish(evento);
    }
}