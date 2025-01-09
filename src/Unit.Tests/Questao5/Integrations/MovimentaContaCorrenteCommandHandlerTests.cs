using Abstractions.Exceptions;
using FluentAssertions;
using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Events;
using Questao5.Application.Handlers.Commands;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Errors;
using Questao5.Domain.Interfaces;

namespace Unit.Tests.Questao5.Integrations;

public sealed class MovimentaContaCorrenteCommandHandlerTests
{
    private readonly Mock<IMovimentacaoCommandStore> _movimentacaoCommandStoreMock;
    private readonly Mock<IEventPublisher> _eventPublisherMock;
    private readonly MovimentaContaCorrenteCommandHandler _handler;

    public MovimentaContaCorrenteCommandHandlerTests()
    {
        _movimentacaoCommandStoreMock = new Mock<IMovimentacaoCommandStore>();
        _eventPublisherMock = new Mock<IEventPublisher>();
        _handler = new MovimentaContaCorrenteCommandHandler(_movimentacaoCommandStoreMock.Object, _eventPublisherMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldReturnMovimentarContaCorrenteResponse()
    {
        // Arrange
        var request = new MovimentaContaCorrenteCommand { Id = Guid.NewGuid().ToString(), TipoMovimentacao = "C", Valor = 100.00m };
        var conta = ContaCorrente.Restore(Guid.Parse(request.Id), 4545, "Nome Valido", true, null);
        Guid idMovimento;

        _movimentacaoCommandStoreMock
            .Setup(x => x.ObterContaEMovimentacoesAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(conta);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IdMovimento.Should().NotBeEmpty();

        _movimentacaoCommandStoreMock.Verify(x => x.ObterContaEMovimentacoesAsync(request.Id, It.IsAny<CancellationToken>()), Times.Once);
        _eventPublisherMock.Verify(x => x.PublishAsync(conta), Times.Once);
    }

    [Fact]
    public async Task Handle_ContaCorrenteNaoExiste_ShouldThrowBusinessException()
    {
        // Arrange
        var request = new MovimentaContaCorrenteCommand { Id = Guid.NewGuid().ToString(), TipoMovimentacao = "C", Valor = 100.00m };

        _movimentacaoCommandStoreMock
            .Setup(x => x.ObterContaEMovimentacoesAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ContaCorrente)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<BusinessException>())
            .And
            .Message.Should().Be(MovimentacaoErrors.ContaCorrenteNaoEncontrada.Detail);

        _movimentacaoCommandStoreMock.Verify(x => x.ObterContaEMovimentacoesAsync(request.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ContaCorrenteInativa_ShouldThrowBusinessException()
    {
        // Arrange
        var request = new MovimentaContaCorrenteCommand { Id = Guid.NewGuid().ToString(), TipoMovimentacao = "C", Valor = 100.00m };
        var conta = ContaCorrente.Restore(Guid.Parse(request.Id), 4545, "Nome Valido", false, null);

        _movimentacaoCommandStoreMock
            .Setup(x => x.ObterContaEMovimentacoesAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(conta);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        (await act.Should().ThrowAsync<BusinessException>())
            .And
            .Message.Should().Be(MovimentacaoErrors.ContaCorrenteInativa.Detail);

        _movimentacaoCommandStoreMock.Verify(x => x.ObterContaEMovimentacoesAsync(request.Id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void MovimentaContaCorrenteCommandValidator_ValidRequest_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var validator = new MovimentaContaCorrenteCommandValidator();
        var request = new MovimentaContaCorrenteCommand { Id = Guid.NewGuid().ToString(), TipoMovimentacao = "C", Valor = 100.00m };
        request.SetRequestId(Guid.NewGuid().ToString());

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void MovimentaContaCorrenteCommandValidator_InvalidRequest_ShouldHaveValidationErrors()
    {
        // Arrange
        var validator = new MovimentaContaCorrenteCommandValidator();
        var request = new MovimentaContaCorrenteCommand { Id = Guid.Empty.ToString(), TipoMovimentacao = "X", Valor = -100.00m };

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(MovimentaContaCorrenteCommand.Valor));
        result.Errors.Should().Contain(e => e.ErrorMessage == MovimentacaoErrors.ValorMovimentacaoInvalido.Detail);

        result.Errors.Should().Contain(e => e.PropertyName == nameof(MovimentaContaCorrenteCommand.TipoMovimentacao));
        result.Errors.Should().Contain(e => e.ErrorMessage == MovimentacaoErrors.TipoMovimentacaoInvalido.Detail);
    }
}

