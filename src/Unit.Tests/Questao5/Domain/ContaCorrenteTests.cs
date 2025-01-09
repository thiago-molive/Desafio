using Abstractions.Exceptions;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Language;

namespace Unit.Tests.Questao5.Domain;

public sealed class ContaCorrenteTests
{
    [Fact]
    public void Restore_ValidInputs_ShouldRestoreContaCorrente()
    {
        // Arrange
        var id = Guid.NewGuid();
        long numeroConta = 123456;
        string nomeCompleto = "João da Silva";
        bool ativo = true;
        var historico = new List<MovimentoConta>
            {
                MovimentoConta.Create(id, ETipoMovimento.Credito, 100.00m),
                MovimentoConta.Create(id, ETipoMovimento.Debito, 50.00m)
            };

        // Act
        var contaCorrente = ContaCorrente.Restore(id, numeroConta, nomeCompleto, ativo, historico);

        // Assert
        Assert.NotNull(contaCorrente);
        Assert.Equal(id, contaCorrente.Id);
        Assert.Equal(numeroConta, contaCorrente.NumeroConta.Numero);
        Assert.Equal("João Da Silva", contaCorrente.NomeCompleto.Nome);
        Assert.Equal(ativo, contaCorrente.Ativo);
        Assert.Equal(historico.Count, contaCorrente.Historico.Count);
    }

    [Fact]
    public void FazerMovimentacao_ValidCredito_ShouldAddToHistorico()
    {
        // Arrange
        var contaCorrente = ContaCorrente.Restore(Guid.NewGuid(), 123456, "João da Silva", true, null);
        decimal valor = 100.00m;

        // Act
        var movimentoId = contaCorrente.FazerMovimentacao(ETipoMovimento.Credito, valor);

        // Assert
        var movimento = contaCorrente.Historico.FirstOrDefault(m => m.Id == movimentoId);
        Assert.NotNull(movimento);
        Assert.Equal(ETipoMovimento.Credito, movimento.TipoMovimento);
        Assert.Equal(valor, movimento.Valor);
    }

    [Fact]
    public void FazerMovimentacao_ValidDebito_ShouldAddToHistorico()
    {
        // Arrange
        var contaCorrente = ContaCorrente.Restore(Guid.NewGuid(), 123456, "João da Silva", true, null);
        decimal valor = 50.00m;

        // Act
        var movimentoId = contaCorrente.FazerMovimentacao(ETipoMovimento.Debito, valor);

        // Assert
        var movimento = contaCorrente.Historico.FirstOrDefault(m => m.Id == movimentoId);
        Assert.NotNull(movimento);
        Assert.Equal(ETipoMovimento.Debito, movimento.TipoMovimento);
        Assert.Equal(valor, movimento.Valor);
    }

    [Fact]
    public void FazerMovimentacao_NegativeAmount_ShouldThrowArgumentException()
    {
        // Arrange
        var contaCorrente = ContaCorrente.Restore(Guid.NewGuid(), 123456, "João da Silva", true, null);
        decimal valor = -100.00m;

        // Act & Assert
        var exception = Assert.Throws<BusinessException>(() => contaCorrente.FazerMovimentacao(ETipoMovimento.Credito, valor));
        Assert.Equal(Mensagens.INVALID_VALUE, exception.Message);
    }
}

