using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces;

public interface IMovimentacaoCommandStore
{
    Task<ContaCorrente?> ObterContaEMovimentacoesAsync(string Id, CancellationToken cancellationToken);

    Task<bool> SaveMovimentacaoAsync(MovimentoConta movimentacaoEntity, CancellationToken cancellationToken);
}