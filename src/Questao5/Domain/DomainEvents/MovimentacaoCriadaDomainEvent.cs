using Abstractions.Domain;
using MediatR;
using Questao5.Domain.Entities;

namespace Questao5.Domain.DomainEvents;

public class MovimentacaoCriadaDomainEvent : INotification, IDomainEvent
{
    public MovimentoConta Movimentacao { get; init; }

    public MovimentacaoCriadaDomainEvent(MovimentoConta movimentacao)
    {
        Movimentacao = movimentacao;
    }
}
