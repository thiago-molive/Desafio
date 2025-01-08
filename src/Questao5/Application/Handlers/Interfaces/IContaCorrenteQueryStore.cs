using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Handlers.Interfaces;

public interface IContaCorrenteQueryStore
{
    Task<(bool, bool)> ContaCorrenteExisteEEstaAtivaAsync(string idContaCorrente, CancellationToken cancellationToken);

    Task<ConsultaSaldoQueryResponse> ObterSaldoContaCorrenteAsync(string idContaCorrente, CancellationToken cancellationToken);
}