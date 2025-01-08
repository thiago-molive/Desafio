using Abstractions.ValueObjects;

namespace Unit.Tests.Domain;

public sealed class NumeroContaTests
{
    [Fact]
    public void Create_ValidNumero_ReturnsNumeroConta()
    {
        // Arrange
        long validNumero = 123456;

        // Act
        var result = NumeroConta.Create(validNumero);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(validNumero, result.Numero);
    }

    [Fact]
    public void Create_NegativeNumero_ThrowsArgumentException()
    {
        // Arrange
        long invalidNumero = -1;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => NumeroConta.Create(invalidNumero));
        Assert.Equal("Número da conta deve ser maior que zero (Parameter 'numero')", exception.Message);
    }

    [Fact]
    public void Create_ZeroNumero_ThrowsArgumentException()
    {
        // Arrange
        long invalidNumero = 0;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => NumeroConta.Create(invalidNumero));
        Assert.Equal("Número da conta deve ser maior que zero (Parameter 'numero')", exception.Message);
    }
}

