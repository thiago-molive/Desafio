using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Middleware;
using System.ComponentModel;
using System.Net;
using Questao5.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;

namespace Questao5.Infrastructure.Services.Controllers;

[ApiController]
[Produces("application/json")]
[Description("Movimentacoes controller")]
[Route("[controller]")]
public sealed class MovimentacoesController : ControllerBase
{
    private readonly ISender _sender;

    public MovimentacoesController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Realiza uma movimentação na conta corrente.
    /// </summary>
    /// <remarks>
    /// - Este endpoint realiza movimentações de crédito ou débito na conta corrente. O cabeçalho `requestId` é obrigatório e usado para rastreamento.
    /// - Identificador da conta corrente é obrigatório e deve ser uma conta corrente cadastrada e ativa.
    /// - Valor a ser movimentado deve ser maior do que zero
    /// - "Tipo de movimentação valores validos: C = Credito, D = Débito
    /// </remarks>
    /// <param name="requestId">ID único para rastreamento da requisição.</param>
    /// <param name="command">Dados da movimentação a ser realizada.</param>
    /// <returns>Retorna o resultado da movimentação.</returns>
    [HttpPost]
    [Description("Realizar movimentações na conta corrente")]
    [ProducesResponseType(typeof(MovimentarContaCorrenteResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ExceptionHandlingMiddleware.ExceptionDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ExceptionHandlingMiddleware.ExceptionDetails), (int)HttpStatusCode.InternalServerError)]
    [SwaggerRequestExample(typeof(MovimentaContaCorrenteCommand), typeof(MovimentaContaCorrenteCommandExample))]
    public async Task<IActionResult> MovimentarContaCorrente([FromHeader(Name = "x-requestId")] string requestId, [FromBody] MovimentaContaCorrenteCommand command)
    {
        if(string.IsNullOrWhiteSpace(requestId))
            return BadRequest("RequestId não informado");

        command.SetRequestId(requestId);
        var response = await _sender.Send(command);

        if (response is null)
            return NotFound();

        return Ok(response);
    }
}
