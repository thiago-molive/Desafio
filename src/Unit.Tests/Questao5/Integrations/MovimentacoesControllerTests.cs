using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Infrastructure.Services.Controllers;

namespace Unit.Tests.Questao5.Integrations;

public sealed class MovimentacoesControllerTests
{
    private readonly Mock<ISender> _senderMock;
    private readonly MovimentacoesController _controller;

    public MovimentacoesControllerTests()
    {
        _senderMock = new Mock<ISender>();
        _controller = new MovimentacoesController(_senderMock.Object);
    }

    [Fact]
    public async Task MovimentarContaCorrente_ValidRequest_ShouldReturnOk()
    {
        // Arrange
        var requestId = Guid.NewGuid().ToString();
        var command = new MovimentaContaCorrenteCommand { Id = Guid.NewGuid().ToString(), TipoMovimentacao = "C", Valor = 100.00m };
        var response = new MovimentarContaCorrenteResponse { IdMovimento = Guid.NewGuid() };

        _senderMock
            .Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.MovimentarContaCorrente(requestId, command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        Assert.Equal(response, okResult.Value);

        _senderMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task MovimentarContaCorrente_NullResponse_ShouldReturnNotFound()
    {
        // Arrange
        var requestId = Guid.NewGuid().ToString();
        var command = new MovimentaContaCorrenteCommand { Id = Guid.NewGuid().ToString(), TipoMovimentacao = "C", Valor = 100.00m };

        _senderMock
            .Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync((MovimentarContaCorrenteResponse)null);

        // Act
        var result = await _controller.MovimentarContaCorrente(requestId, command);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);

        _senderMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task MovimentarContaCorrente_MissingRequestId_ShouldReturnBadRequest()
    {
        // Arrange
        var command = new MovimentaContaCorrenteCommand { Id = Guid.NewGuid().ToString(), TipoMovimentacao = "C", Valor = 100.00m };

        // Act
        var result = await _controller.MovimentarContaCorrente(null, command);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);

        _senderMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Never);
    }
}

