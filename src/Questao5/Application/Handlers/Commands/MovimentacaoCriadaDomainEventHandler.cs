using Abstractions.Exceptions;
using MediatR;
using Questao5.Domain.DomainEvents;
using Questao5.Domain.Errors;
using Questao5.Domain.Interfaces;

namespace Questao5.Application.Handlers.Commands;

public sealed class MovimentacaoCriadaDomainEventHandler : INotificationHandler<MovimentacaoCriadaDomainEvent>
{
    private readonly IMovimentacaoCommandStore _movimentacaoCommandStore;

    public MovimentacaoCriadaDomainEventHandler(IMovimentacaoCommandStore movimentacaoCommandStore)
    {
        _movimentacaoCommandStore = movimentacaoCommandStore;
    }

    public async Task Handle(MovimentacaoCriadaDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification?.Movimentacao is null)
            return;

        if (!await _movimentacaoCommandStore.SaveMovimentacaoAsync(notification.Movimentacao, cancellationToken))
            throw new BusinessException(MovimentacaoErrors.UnknowError);
    }
}
