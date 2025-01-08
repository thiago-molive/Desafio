using System.ComponentModel.DataAnnotations;

namespace Questao5.Domain.Enumerators;

public enum ETipoMovimento
{
    [Display(Name = "C")]
    Credito,

    [Display(Name = "D")]
    Debito
}