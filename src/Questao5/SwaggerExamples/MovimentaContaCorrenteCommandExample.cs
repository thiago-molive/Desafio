using Questao5.Application.Commands.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace Questao5.SwaggerExamples;

public class MovimentaContaCorrenteCommandExample : IExamplesProvider<MovimentaContaCorrenteCommand>
{
    public MovimentaContaCorrenteCommand GetExamples()
    {
        var result = new MovimentaContaCorrenteCommand
        {
            Id = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9",
            TipoMovimentacao = "C",
            Valor = 200
        };

        return result;
    }
}