using Abstractions.Domain;
using Abstractions.Exceptions;

namespace Abstractions.ValueObjects;

public sealed class NumeroConta : ValueObject
{
    public long Numero { get; private set; }

    private NumeroConta() { }

    public static NumeroConta Create(long numero)
    {
        if (numero <= 0)
            throw new BusinessException(new Error("INVALID_ACCOUNT", "Número da conta deve ser maior que zero"));

        return new NumeroConta()
        {
            Numero = numero
        };
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Numero;
    }
}