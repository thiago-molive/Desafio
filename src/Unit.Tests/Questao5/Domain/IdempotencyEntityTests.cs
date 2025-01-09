using Questao5.Domain.Entities;

namespace Unit.Tests.Questao5.Domain;

public sealed class IdempotencyEntityTests
{
    [Fact]
    public void Create_ValidInputs_ShouldCreateIdempotencyEntity()
    {
        // Arrange
        string id = Guid.NewGuid().ToString();
        string requisicao = "Sample Request";
        string resultado = "Sample Result";

        // Act
        var idempotencyEntity = IdempotencyEntity.Create(id, requisicao, resultado);

        // Assert
        Assert.NotNull(idempotencyEntity);
        Assert.Equal(id, idempotencyEntity.Id);
        Assert.Equal(requisicao, idempotencyEntity.Requisicao);
        Assert.Equal(resultado, idempotencyEntity.Resultado);
    }

    [Fact]
    public void DefinirResultado_ValidResponse_ShouldSetResultado()
    {
        // Arrange
        string id = Guid.NewGuid().ToString();
        string requisicao = "Sample Request";
        string resultado = "Sample Result";
        var idempotencyEntity = IdempotencyEntity.Create(id, requisicao, null);

        // Act
        idempotencyEntity.DefinirResultado(resultado);

        // Assert
        Assert.Equal(resultado, idempotencyEntity.Resultado);
    }

    [Fact]
    public void DefinirResultado_AlreadySet_ShouldNotChangeResultado()
    {
        // Arrange
        string id = Guid.NewGuid().ToString();
        string requisicao = "Sample Request";
        string initialResultado = "Initial Result";
        string newResultado = "New Result";
        var idempotencyEntity = IdempotencyEntity.Create(id, requisicao, initialResultado);

        // Act
        idempotencyEntity.DefinirResultado(newResultado);

        // Assert
        Assert.Equal(initialResultado, idempotencyEntity.Resultado);
    }
}

