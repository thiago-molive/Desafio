using Abstractions.Exceptions;
using Questao5.Domain.Errors;
using System.ComponentModel.DataAnnotations;

namespace Questao5.Domain.Enumerators;

public enum ETipoMovimento
{
    [Display(Name = "C")]
    Credito,

    [Display(Name = "D")]
    Debito
}

public static class TipoMovimentoExtensions
{
    public static ETipoMovimento ParseTipoMovimento(this string tipoMovimento)
    {
        if (string.IsNullOrWhiteSpace(tipoMovimento))
            throw new BusinessException(MovimentacaoErrors.TipoMovimentacaoInvalido);

        return GetEnumValue(tipoMovimento[0].ToString());
    }

    private static ETipoMovimento GetEnumValue(string fistLetter) =>
        fistLetter.ToUpper() switch
        {
            "C" => ETipoMovimento.Credito,
            "D" => ETipoMovimento.Debito,
            _ => throw new BusinessException(MovimentacaoErrors.TipoMovimentacaoInvalido)
        };
}