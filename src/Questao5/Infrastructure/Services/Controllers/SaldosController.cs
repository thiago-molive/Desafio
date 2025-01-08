using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Middleware;
using System.ComponentModel;
using System.Net;

namespace Questao5.Infrastructure.Services.Controllers;

[ApiController]
[Produces("application/json")]
[Description("Saldos controller")]
[Route("[controller]")]
public sealed class SaldosController : ControllerBase
{
    private readonly ISender _sender;

    public SaldosController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id}")]
    [Description("Obter saldos pela identificação da conta corrente")]
    [ProducesResponseType(typeof(ConsultaSaldoQueryResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ExceptionHandlingMiddleware.ExceptionDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ExceptionHandlingMiddleware.ExceptionDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<IActionResult> Get(string id)
    {
        var response = await _sender.Send(new ConsultaSaldoQuery { IdContaCorrente = id });

        if (response is null)
            return NotFound();

        return Ok(response);
    }
}
