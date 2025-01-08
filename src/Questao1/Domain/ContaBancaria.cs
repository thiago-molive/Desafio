using Abstractions.ValueObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Questao1.Domain;

public sealed class ContaBancaria
{
    public static readonly decimal TAXA_SAQUE = 3.5m;

    private IList<decimal> _historico = new List<decimal>();

    public NumeroConta NumeroConta { get; private set; }

    public NomeCompleto NomeCompleto { get; private set; }

    private decimal Saldo => _historico.Sum();

    public IReadOnlyCollection<decimal> Historico => (IReadOnlyCollection<decimal>)_historico;

    public static ContaBancaria Create(long numeroConta, string nomeCompleto, decimal depositoInicial = 0)
    {
        var conta = new ContaBancaria()
        {
            NomeCompleto = NomeCompleto.Create(nomeCompleto),
            NumeroConta = NumeroConta.Create(numeroConta)
        };

        conta.Deposito(depositoInicial);

        return conta;
    }

    public override string ToString() =>
        $"Conta {NumeroConta.Numero}, Titular: {NomeCompleto.Nome}, Saldo: $ {Saldo.ToString("N2", new CultureInfo("en-US"))}";

    public void Deposito(decimal valor)
    {
        if (valor <= 0)
            return;

        _historico.Add(valor);
    }

    public void Saque(decimal quantia)
    {
        if (quantia <= 0)
            throw new ArgumentException("Valor do saque deve ser maior que zero", nameof(quantia));

        if (Saldo < quantia)
            throw new InvalidOperationException("Saldo insuficiente");

        quantia += TAXA_SAQUE;

        _historico.Add(-quantia);
    }
}