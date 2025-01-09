namespace Abstractions;

public interface IIdempotency
{
    public string RequestId { get; }
}