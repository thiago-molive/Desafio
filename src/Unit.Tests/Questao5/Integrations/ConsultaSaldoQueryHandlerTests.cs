using Abstractions.Exceptions;
using FluentAssertions;
using Moq;
using Questao5.Application.Handlers.Queries;
using Questao5.Application.Handlers.Queries.Interfaces;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Errors;

namespace Unit.Tests.Questao5.Integrations;

public sealed class ConsultaSaldoQueryHandlerTests
{
    private readonly Mock<IContaCorrenteQueryStore> _contaCorrenteQueryStoreMock;
    private readonly ConsultaSaldoQueryHandler _handler;

    public ConsultaSaldoQueryHandlerTests()
    {
        _contaCorrenteQueryStoreMock = new Mock<IContaCorrenteQueryStore>();
        _handler = new ConsultaSaldoQueryHandler(_contaCorrenteQueryStoreMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldReturnConsultaSaldoQueryResponse()
    {
        // Arrange
        var request = new ConsultaSaldoQuery { IdContaCorrente = Guid.NewGuid().ToString() };
        var response = new ConsultaSaldoQueryResponse { Saldo = 100.00m };

        _contaCorrenteQueryStoreMock
            .Setup(x => x.ContaCorrenteExisteEEstaAtivaAsync(request.IdContaCorrente, It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, true));

        _contaCorrenteQueryStoreMock
            .Setup(x => x.ObterSaldoContaCorrenteAsync(request.IdContaCorrente, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(response.Saldo, result.Saldo);

        _contaCorrenteQueryStoreMock.Verify(x => x.ContaCorrenteExisteEEstaAtivaAsync(request.IdContaCorrente, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ContaCorrenteNaoExiste_ShouldThrowBusinessException()
    {
        // Arrange
        var request = new ConsultaSaldoQuery { IdContaCorrente = Guid.NewGuid().ToString() };

        _contaCorrenteQueryStoreMock
            .Setup(x => x.ContaCorrenteExisteEEstaAtivaAsync(request.IdContaCorrente, It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, false));

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        //  Assert
        (await act.Should().ThrowAsync<BusinessException>())
            .And
            .Message.Should().Be(ContaCorrenteErrors.ContaCorrenteNaoEncontrada.Detail);

        _contaCorrenteQueryStoreMock.Verify(x => x.ContaCorrenteExisteEEstaAtivaAsync(request.IdContaCorrente, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ContaCorrenteInativa_ShouldThrowBusinessException()
    {
        // Arrange
        var request = new ConsultaSaldoQuery { IdContaCorrente = Guid.NewGuid().ToString() };

        _contaCorrenteQueryStoreMock
            .Setup(x => x.ContaCorrenteExisteEEstaAtivaAsync(request.IdContaCorrente, It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, false));

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        //  & Assert
        (await act.Should().ThrowAsync<BusinessException>())
            .And
            .Message.Should().Be(ContaCorrenteErrors.ContaCorrenteInativa.Detail);

        _contaCorrenteQueryStoreMock.Verify(x => x.ContaCorrenteExisteEEstaAtivaAsync(request.IdContaCorrente, It.IsAny<CancellationToken>()), Times.Once);
    }
}

