using System.Globalization;
using System.Text.RegularExpressions;
using Abstractions.Domain;
using Abstractions.Exceptions;

namespace Abstractions.ValueObjects;

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
            throw new BusinessException(new Error("INVALID_NAME", "Nome deve ser informado"));

        if (!Regex.IsMatch(Nome, "^[A-Za-zÀ-ÖØ-öø-ÿ]+(?: [A-Za-zÀ-ÖØ-öø-ÿ]+)+$"))
            throw new BusinessException(new Error("INVALID_NAME", "Nome informado não é valido"));
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Nome;
    }
}

