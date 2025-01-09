using Questao5.Domain.DomainEvents;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Unit.Tests.Questao5.Domain.Events;

public sealed class MovimentacaoCriadaDomainEventTests
{
    [Fact]
    public void Create_ValidMovimentoConta_ShouldCreateDomainEvent()
    {
        // Arrange
        var idContaCorrente = Guid.NewGuid();
        var tipoMovimento = ETipoMovimento.Credito;
        var valor = 100.00m;
        var movimentoConta = MovimentoConta.Create(idContaCorrente, tipoMovimento, valor);

        // Act
        var domainEvent = new MovimentacaoCriadaDomainEvent(movimentoConta);

        // Assert
        Assert.NotNull(domainEvent);
        Assert.Equal(movimentoConta, domainEvent.Movimentacao);
    }
}

