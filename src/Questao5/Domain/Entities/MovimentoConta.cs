using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities;

public sealed class MovimentoConta
{
    public Guid Id { get; private set; }

    public Guid IdContaCorrente { get; set; }

    public DateTime DataMovimento { get; private set; }

    public ETipoMovimento TipoMovimento { get; private set; }

    public decimal Valor { get; private set; }

    private MovimentoConta() { }

    public static MovimentoConta Create(Guid idContaCorrente, ETipoMovimento tipoMovimento, decimal valor)
    {
        return new MovimentoConta()
        {
            Id = Guid.NewGuid(),
            IdContaCorrente = idContaCorrente,
            DataMovimento = DateTime.UtcNow,
            TipoMovimento = tipoMovimento,
            Valor = valor
        };
    }
}
