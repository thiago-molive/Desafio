using Questao1.Domain;

namespace Unit.Tests.Questao1.Domain;

public sealed class ContaBancariaTests
{
    [Fact]
    public void Create_ValidContaBancaria_ReturnsContaBancaria()
    {
        // Arrange
        long numeroConta = 123456;
        string nomeCompleto = "joão da silva";
        decimal depositoInicial = 100;

        // Act
        var result = ContaBancaria.Create(numeroConta, nomeCompleto, depositoInicial);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(numeroConta, result.NumeroConta.Numero);
        Assert.Equal("João Da Silva", result.NomeCompleto.Nome);
        Assert.Equal(depositoInicial, result.Historico.Sum());
    }

    [Fact]
    public void Create_InvalidNumeroConta_ThrowsArgumentException()
    {
        // Arrange
        long invalidNumeroConta = -1;
        string nomeCompleto = "joão da silva";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => ContaBancaria.Create(invalidNumeroConta, nomeCompleto));
        Assert.Equal("Número da conta deve ser maior que zero (Parameter 'numero')", exception.Message);
    }

    [Fact]
    public void Create_InvalidNomeCompleto_ThrowsArgumentException()
    {
        // Arrange
        long numeroConta = 123456;
        string invalidNomeCompleto = "joão123";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => ContaBancaria.Create(numeroConta, invalidNomeCompleto));
        Assert.Equal("Nome informado não é valido (Parameter 'Nome')", exception.Message);
    }

    [Fact]
    public void Deposito_ValidAmount_AddsToHistorico()
    {
        // Arrange
        var conta = ContaBancaria.Create(123456, "joão da silva");
        decimal deposito = 50;

        // Act
        conta.Deposito(deposito);

        // Assert
        Assert.Contains(deposito, conta.Historico);
    }

    [Fact]
    public void Deposito_InvalidAmount_DoesNotAddToHistorico()
    {
        // Arrange
        var conta = ContaBancaria.Create(123456, "joão da silva");
        decimal deposito = -50;

        // Act
        conta.Deposito(deposito);

        // Assert
        Assert.DoesNotContain(deposito, conta.Historico);
    }

    [Fact]
    public void Saque_ValidAmount_SubtractsFromHistorico()
    {
        // Arrange
        var conta = ContaBancaria.Create(123456, "joão da silva", 100);
        decimal saque = 50;

        // Act
        conta.Saque(saque);

        // Assert
        Assert.Contains(-saque - ContaBancaria.TAXA_SAQUE, conta.Historico);
    }

    [Fact]
    public void Saque_AmountGreaterThanSaldo_ThrowsInvalidOperationException()
    {
        // Arrange
        var conta = ContaBancaria.Create(123456, "joão da silva", 50);
        decimal saque = 100;

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => conta.Saque(saque));
        Assert.Equal("Saldo insuficiente", exception.Message);
    }

    [Fact]
    public void Saque_InvalidAmount_ThrowsArgumentException()
    {
        // Arrange
        var conta = ContaBancaria.Create(123456, "joão da silva");
        decimal saque = -50;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => conta.Saque(saque));
        Assert.Equal("Valor do saque deve ser maior que zero (Parameter 'quantia')", exception.Message);
    }
}

