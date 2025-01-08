namespace Abstractions.ValueObjects;

public sealed class NumeroConta : ValueObject
{
    public long Numero { get; private set; }

    private NumeroConta() { }

    public static NumeroConta Create(long numero)
    {
        if (numero <= 0)
            throw new ArgumentException("Número da conta deve ser maior que zero", nameof(numero));

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