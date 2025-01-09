using MediatR;
using Newtonsoft.Json;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using Questao5.MediatrAbstractions;

namespace Questao5.Behaviors;

internal sealed class IdempotencyBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IdempotentCommand<TResponse>
{
    private readonly IIdempotencyCommandStore _idempotencyCommandStore;

    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
    };

    public IdempotencyBehavior(IIdempotencyCommandStore idempotencyCommandStore)
    {
        _idempotencyCommandStore = idempotencyCommandStore;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RequestId))
            return await next();

        var idempotencyEntity = await _idempotencyCommandStore.GetRequestByKeyAsync(request.RequestId, cancellationToken);

        if (idempotencyEntity is not null)
        {
            if (!string.IsNullOrWhiteSpace(idempotencyEntity.Resultado))
                return JsonConvert.DeserializeObject<TResponse>(idempotencyEntity.Resultado, JsonSerializerSettings)!;
        }
        else
        {
            idempotencyEntity = IdempotencyEntity.Create(request.RequestId, JsonConvert.SerializeObject(request, JsonSerializerSettings), null);
            await _idempotencyCommandStore.SaveRequestAsync(idempotencyEntity, cancellationToken);
        }

        var response = await next();

        idempotencyEntity.DefinirResultado(JsonConvert.SerializeObject(response, JsonSerializerSettings));

        await _idempotencyCommandStore.MarkRequestAsProcessedAsync(idempotencyEntity, cancellationToken);

        return response;
    }
}