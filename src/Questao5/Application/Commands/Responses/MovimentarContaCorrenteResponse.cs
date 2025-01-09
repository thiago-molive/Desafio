namespace Questao5.Application.Commands.Responses;

public sealed class MovimentarContaCorrenteResponse
{
    public Guid IdMovimento { get; set; }

    public static MovimentarContaCorrenteResponse Ok(Guid idMovimento) =>
        new()
        {
            IdMovimento = idMovimento
        };
}