using Abstractions.Domain;

namespace Questao5.Domain.Entities;

public class IdempotencyEntity : EntityBase<string>
{
    public string Requisicao { get; private set; }

    public string Resultado { get; private set; }

    public static IdempotencyEntity Create(string id, string requisicao, string resultado)
    {
        return new IdempotencyEntity
        {
            Id = id,
            Requisicao = requisicao,
            Resultado = resultado
        };
    }

    public void DefinirResultado(string resposta)
    {
        if (!string.IsNullOrWhiteSpace(Resultado))
            return;

        Resultado = resposta;
    }
}
