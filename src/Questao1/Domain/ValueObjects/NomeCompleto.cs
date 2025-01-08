using Abstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Questao1.Domain.ValueObjects;

public sealed class NomeCompleto : ValueObject
{
    public string Nome { get; private set; }

    private NomeCompleto() { }

    public static NomeCompleto Create(string nomeCompleto)
    {
        var nome = new NomeCompleto()
        {
            Nome = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nomeCompleto.ToLower())
        };

        nome.Validate();

        return nome;
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Nome))
            throw new ArgumentException("Nome deve ser informado", nameof(Nome));

        if (!Regex.IsMatch(Nome, "^[A-Za-zÀ-ÖØ-öø-ÿ]+(?: [A-Za-zÀ-ÖØ-öø-ÿ]+)+$"))
            throw new ArgumentException("Nome informado não é valido", nameof(Nome));
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Nome;
    }
}

