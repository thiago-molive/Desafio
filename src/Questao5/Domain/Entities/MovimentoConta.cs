using Abstractions.Domain;
using Abstractions.Exceptions;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Errors;

namespace Questao5.Domain.Entities;

public sealed class MovimentoConta : EntityBase<Guid>
{
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

    public static MovimentoConta Restore(Guid id, string tipoMovimento, decimal valor, DateTime dataMovimento)
    {
        return new MovimentoConta()
        {
            Id = id,
            DataMovimento = dataMovimento,
            TipoMovimento = tipoMovimento switch
            {
                "C" => ETipoMovimento.Credito,
                "D" => ETipoMovimento.Debito,
                _ => throw new BusinessException(MovimentacaoErrors.TipoMovimentacaoInvalido)
            },
            Valor = valor
        };
    }
}
