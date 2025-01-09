using System.Text.Json.Serialization;
using Abstractions;
using MediatR;

namespace Questao5.MediatrAbstractions;

public abstract class IdempotentCommand<TResponse> : IRequest<TResponse>, IIdempotency
{
    [JsonIgnore]
    public string RequestId { get; private set; }

    public void SetRequestId(string requestId)
    {
        RequestId = requestId;
    }
}
