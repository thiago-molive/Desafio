using Abstractions.ValueObjects;
using Questao5.Domain.Enumerators;
using System.Globalization;

namespace Questao5.Domain.Entities;

public sealed class ContaCorrente
{
    private IList<MovimentoConta> _historico = new List<MovimentoConta>();

    public Guid Id { get; private set; }

    public NumeroConta NumeroConta { get; private set; }

    public NomeCompleto NomeCompleto { get; private set; }

    public bool Ativo { get; private set; }

    private decimal Saldo => _historico.Sum(x => x.Valor);

    public IReadOnlyCollection<MovimentoConta> Historico => (IReadOnlyCollection<MovimentoConta>)_historico;

    public static ContaCorrente Create(long numeroConta
        , string nomeCompleto
        , bool ativo = false)
    {
        var conta = new ContaCorrente()
        {
            Id = Guid.NewGuid(),
            NomeCompleto = NomeCompleto.Create(nomeCompleto),
            NumeroConta = NumeroConta.Create(numeroConta),
            Ativo = ativo
        };

        return conta;
    }

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

        foreach (var movimento in historico)
            conta._historico.Add(movimento);

        return conta;
    }

    public void Deposito(decimal valor)
    {
        if (valor <= 0)
            return;

        _historico.Add(MovimentoConta.Create(Id, ETipoMovimento.Credito, valor));
    }

    public void Saque(decimal quantia)
    {
        if (quantia <= 0)
            throw new ArgumentException("Valor do saque deve ser maior que zero", nameof(quantia));

        if (Saldo < quantia)
            throw new InvalidOperationException("Saldo insuficiente");

        _historico.Add(MovimentoConta.Create(Id, ETipoMovimento.Debito, -quantia));
    }
}
