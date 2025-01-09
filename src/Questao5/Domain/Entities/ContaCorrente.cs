using Abstractions.Domain;
using Abstractions.Exceptions;
using Abstractions.ValueObjects;
using Questao5.Domain.DomainEvents;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Language;

namespace Questao5.Domain.Entities;

public sealed class ContaCorrente : EntityBase<Guid>
{
    private IList<MovimentoConta> _historico = new List<MovimentoConta>();

    public NumeroConta NumeroConta { get; private set; }

    public NomeCompleto NomeCompleto { get; private set; }

    public bool Ativo { get; private set; }

    private decimal Saldo => _historico.Sum(x => x.TipoMovimento == ETipoMovimento.Debito ? -x.Valor : x.Valor);

    public IReadOnlyCollection<MovimentoConta> Historico => (IReadOnlyCollection<MovimentoConta>)_historico;

    public bool Inativa => !Ativo;

    public static ContaCorrente Restore(Guid id
        , long numeroConta
        , string nomeCompleto
        , bool ativo
        , IEnumerable<MovimentoConta> historico)
    {
        var conta = new ContaCorrente()
        {
            Id = id,
            NomeCompleto = NomeCompleto.Create(nomeCompleto),
            NumeroConta = NumeroConta.Create(numeroConta),
            Ativo = ativo
        };

        if (historico is not null)
            foreach (var movimento in historico)
                conta._historico.Add(movimento);

        return conta;
    }

    public Guid FazerMovimentacao(ETipoMovimento tipoMovimento, decimal valor)
    {
        if(valor < 0)
            throw new BusinessException(new Error(nameof(Mensagens.INVALID_VALUE), Mensagens.INVALID_VALUE));

        var movimento = MovimentoConta.Create(Id, tipoMovimento, valor);

        _historico.Add(movimento);

        this.Publish(new MovimentacaoCriadaDomainEvent(movimento));

        return movimento.Id;
    }
}
