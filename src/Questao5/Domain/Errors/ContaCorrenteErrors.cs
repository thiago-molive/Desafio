using Abstractions.Exceptions;
using Questao5.Domain.Language;

namespace Questao5.Domain.Errors;

public static class ContaCorrenteErrors
{
    public static Error ContaCorrenteNaoEncontrada => new Error(
        nameof(Mensagens.INVALID_ACCOUNT)
        , Mensagens.INVALID_ACCOUNT);

    public static Error ContaCorrenteInativa => new Error(
        nameof(Mensagens.INACTIVE_ACCOUNT)
        , Mensagens.INACTIVE_ACCOUNT);
}
