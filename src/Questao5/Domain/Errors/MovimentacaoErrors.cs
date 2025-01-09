using Abstractions.Exceptions;
using Questao5.Domain.Language;

namespace Questao5.Domain.Errors;

public static class MovimentacaoErrors
{
    public static Error ContaCorrenteNaoEncontrada => new Error(nameof(Mensagens.INVALID_ACCOUNT)
        , Mensagens.INVALID_ACCOUNT_MOV);

    public static Error ContaCorrenteInativa => new Error(nameof(Mensagens.INACTIVE_ACCOUNT)
        , Mensagens.INACTIVE_ACCOUNT_MOV);

    public static Error ValorMovimentacaoInvalido => new Error(nameof(Mensagens.INVALID_VALUE)
        , Mensagens.INVALID_VALUE);

    public static Error TipoMovimentacaoInvalido => new Error(nameof(Mensagens.INVALID_TYPE)
        , Mensagens.INVALID_TYPE);

    public static Error UnknowError => new Error(nameof(Mensagens.UNKNOWN_ERROR)
        , Mensagens.UNKNOWN_ERROR);
}
