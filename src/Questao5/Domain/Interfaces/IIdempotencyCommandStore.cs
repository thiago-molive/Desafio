using Questao5.Domain.Entities;

namespace Questao5.Domain.Interfaces;

public interface IIdempotencyCommandStore
{
    Task<IdempotencyEntity?> GetRequestByKeyAsync(string key, CancellationToken cancellationToken);

    Task SaveRequestAsync(IdempotencyEntity idempotencyEntity, CancellationToken cancellationToken);

    Task MarkRequestAsProcessedAsync(IdempotencyEntity entity, CancellationToken cancellationToken);
}