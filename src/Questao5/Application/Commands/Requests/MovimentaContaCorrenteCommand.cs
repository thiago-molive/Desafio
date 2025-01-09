using Questao5.Application.Commands.Responses;
using Questao5.MediatrAbstractions;

namespace Questao5.Application.Commands.Requests;

public sealed class MovimentaContaCorrenteCommand : IdempotentCommand<MovimentarContaCorrenteResponse>
{
    public string Id { get; set; }

    public decimal Valor { get; set; }

    public string TipoMovimentacao { get; set; }
}