using Abstractions.Exceptions;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Errors;

namespace Unit.Tests.Questao5.Domain;

public sealed class MovimentoContaTests
{
    [Fact]
    public void Create_ValidInputs_ShouldCreateMovimentoConta()
    {
        // Arrange
        var idContaCorrente = Guid.NewGuid();
        var tipoMovimento = ETipoMovimento.Credito;
        var valor = 100.00m;

        // Act
        var movimentoConta = MovimentoConta.Create(idContaCorrente, tipoMovimento, valor);

        // Assert
        Assert.NotNull(movimentoConta);
        Assert.Equal(idContaCorrente, movimentoConta.IdContaCorrente);
        Assert.Equal(tipoMovimento, movimentoConta.TipoMovimento);
        Assert.Equal(valor, movimentoConta.Valor);
        Assert.Equal(DateTime.UtcNow.Date, movimentoConta.DataMovimento.Date);
    }

    [Fact]
    public void Restore_ValidInputs_ShouldRestoreMovimentoConta()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tipoMovimento = "C";
        var valor = 100.00m;
        var dataMovimento = DateTime.UtcNow;

        // Act
        var movimentoConta = MovimentoConta.Restore(id, tipoMovimento, valor, dataMovimento);

        // Assert
        Assert.NotNull(movimentoConta);
        Assert.Equal(id, movimentoConta.Id);
        Assert.Equal(ETipoMovimento.Credito, movimentoConta.TipoMovimento);
        Assert.Equal(valor, movimentoConta.Valor);
        Assert.Equal(dataMovimento, movimentoConta.DataMovimento);
    }

    [Fact]
    public void Restore_InvalidTipoMovimento_ShouldThrowBusinessException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tipoMovimento = "X";
        var valor = 100.00m;
        var dataMovimento = DateTime.UtcNow;

        // Act & Assert
        var exception = Assert.Throws<BusinessException>(() => MovimentoConta.Restore(id, tipoMovimento, valor, dataMovimento));
        Assert.Equal(MovimentacaoErrors.TipoMovimentacaoInvalido.Detail, exception.Message);
    }
}

