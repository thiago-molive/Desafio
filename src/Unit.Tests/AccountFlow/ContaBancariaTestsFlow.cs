using Questao1.Domain;

namespace Unit.Tests.AccountFlow;

public sealed class ContaBancariaTestsFlow
{
    [Fact]
    [Trait("Unit Tests", nameof(CreateContaBancaria_WithInitialDeposit))]
    public void CreateContaBancaria_WithInitialDeposit()
    {
        // Arrange
        long numeroConta = 5447;
        string titular = "Milton Gonçalves";
        decimal depositoInicial = 350.00m;

        // Act
        var conta = ContaBancaria.Create(numeroConta, titular, depositoInicial);

        // Assert
        Assert.NotNull(conta);
        Assert.Equal("Conta 5447, Titular: Milton Gonçalves, Saldo: $ 350.00", conta.ToString());
        Assert.Equal(numeroConta, conta.NumeroConta.Numero);
        Assert.Equal("Milton Gonçalves", conta.NomeCompleto.Nome);
        Assert.Equal(depositoInicial, conta.Historico.Sum());
    }

    [Fact]
    [Trait("Unit Tests", nameof(CreateContaBancaria_WithoutInitialDeposit))]
    public void CreateContaBancaria_WithoutInitialDeposit()
    {
        // Arrange
        long numeroConta = 5139;
        string titular = "Elza Soares";

        // Act
        var conta = ContaBancaria.Create(numeroConta, titular);

        // Assert
        Assert.NotNull(conta);
        Assert.Equal("Conta 5139, Titular: Elza Soares, Saldo: $ 0.00", conta.ToString());
        Assert.Equal(numeroConta, conta.NumeroConta.Numero);
        Assert.Equal("Elza Soares", conta.NomeCompleto.Nome);
        Assert.Equal(0, conta.Historico.Sum());
    }

    [Fact]
    [Trait("Unit Tests", nameof(Deposito_ValidAmount))]
    public void Deposito_ValidAmount()
    {
        // Arrange
        var conta = ContaBancaria.Create(5447, "Milton Gonçalves", 350.00m);
        decimal deposito = 200.00m;

        // Act
        conta.Deposito(deposito);

        // Assert
        Assert.Equal("Conta 5447, Titular: Milton Gonçalves, Saldo: $ 550.00", conta.ToString());
        Assert.Contains(deposito, conta.Historico);
        Assert.Equal(550.00m, conta.Historico.Sum());
    }

    [Fact]
    [Trait("Unit Tests", nameof(Saque_ValidAmount))]
    public void Saque_ValidAmount()
    {
        // Arrange
        var conta = ContaBancaria.Create(5447, "Milton Gonçalves", 550.00m);
        decimal saque = 199.00m;

        // Act
        conta.Saque(saque);

        // Assert
        Assert.Equal("Conta 5447, Titular: Milton Gonçalves, Saldo: $ 347.50", conta.ToString());
        Assert.Contains(-saque - ContaBancaria.TAXA_SAQUE, conta.Historico);
        Assert.Equal(347.50m, conta.Historico.Sum());
    }

    [Fact]
    [Trait("Unit Tests", nameof(FluxoCompleto_Milton))]
    public void FluxoCompleto_Milton()
    {
        // Arrange
        long numeroConta = 5447;
        string titular = "Milton Gonçalves";
        decimal depositoInicial = 350.00m;
        decimal deposito = 200.00m;
        decimal saque = 199.00m;

        // Act
        var conta = ContaBancaria.Create(numeroConta, titular, depositoInicial);
        conta.Deposito(deposito);
        conta.Saque(saque);

        // Assert
        Assert.NotNull(conta);
        Assert.Equal("Conta 5447, Titular: Milton Gonçalves, Saldo: $ 347.50", conta.ToString());
        Assert.Equal(numeroConta, conta.NumeroConta.Numero);
        Assert.Equal("Milton Gonçalves", conta.NomeCompleto.Nome);
        Assert.Equal(347.50m, conta.Historico.Sum());
    }

    [Fact]
    [Trait("Unit Tests", nameof(FluxoCompleto_Elza))]
    public void FluxoCompleto_Elza()
    {
        // Arrange
        long numeroConta = 5139;
        string titular = "Elza Soares";
        decimal depositoInicial = 0.00m;
        decimal deposito = 300.00m;
        decimal saque = 298.00m;

        // Act
        var conta = ContaBancaria.Create(numeroConta, titular, depositoInicial);
        conta.Deposito(deposito);
        conta.Saque(saque);

        // Assert
        Assert.NotNull(conta);
        Assert.Equal("Conta 5139, Titular: Elza Soares, Saldo: $ -1.50", conta.ToString());
        Assert.Equal(numeroConta, conta.NumeroConta.Numero);
        Assert.Equal("Elza Soares", conta.NomeCompleto.Nome);
        Assert.Equal(-1.50m, conta.Historico.Sum());
    }
}

