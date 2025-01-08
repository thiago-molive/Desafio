using Questao1.Domain.ValueObjects;

namespace Unit.Tests.Domain;

public sealed class NomeCompletoTests
{
    [Fact]
    public void Create_ValidNomeCompleto_ReturnsNomeCompleto()
    {
        // Arrange
        string validNomeCompleto = "joão da silva";

        // Act
        var result = NomeCompleto.Create(validNomeCompleto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("João Da Silva", result.Nome);
    }

    [Fact]
    public void Create_EmptyNomeCompleto_ThrowsArgumentException()
    {
        // Arrange
        string invalidNomeCompleto = "";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => NomeCompleto.Create(invalidNomeCompleto));
        Assert.Equal("Nome deve ser informado (Parameter 'Nome')", exception.Message);
    }

    [Fact]
    public void Create_NullNomeCompleto_ThrowsArgumentException()
    {
        // Arrange
        string invalidNomeCompleto = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => NomeCompleto.Create(invalidNomeCompleto));
        Assert.Equal("Nome deve ser informado (Parameter 'Nome')", exception.Message);
    }

    [Fact]
    public void Create_InvalidNomeCompletoFormat_ThrowsArgumentException()
    {
        // Arrange
        string invalidNomeCompleto = "joão123";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => NomeCompleto.Create(invalidNomeCompleto));
        Assert.Equal("Nome informado não é valido (Parameter 'Nome')", exception.Message);
    }
}

