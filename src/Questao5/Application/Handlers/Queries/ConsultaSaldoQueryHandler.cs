using Abstractions.Exceptions;
using FluentValidation;
using MediatR;
using Questao5.Application.Handlers.Queries.Interfaces;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Errors;

namespace Questao5.Application.Handlers.Queries;

public sealed class ConsultaSaldoQueryHandler : IRequestHandler<ConsultaSaldoQuery, ConsultaSaldoQueryResponse>
{
    private readonly IContaCorrenteQueryStore _contaCorrenteQueryStore;

    public ConsultaSaldoQueryHandler(IContaCorrenteQueryStore contaCorrenteQueryStore)
    {
        _contaCorrenteQueryStore = contaCorrenteQueryStore;
    }

    public async Task<ConsultaSaldoQueryResponse> Handle(ConsultaSaldoQuery request, CancellationToken cancellationToken)
    {
        var (contaCorrenteExiste, contaCorrenteEstaAtiva) = await _contaCorrenteQueryStore.ContaCorrenteExisteEEstaAtivaAsync(request.IdContaCorrente, cancellationToken);

        if (!contaCorrenteExiste)
            throw new BusinessException(ContaCorrenteErrors.ContaCorrenteNaoEncontrada);

        if (!contaCorrenteEstaAtiva)
            throw new BusinessException(ContaCorrenteErrors.ContaCorrenteInativa);

        return await _contaCorrenteQueryStore.ObterSaldoContaCorrenteAsync(request.IdContaCorrente, cancellationToken);
    }
}

internal sealed class AddCategoryCommandValidator : AbstractValidator<ConsultaSaldoQuery>
{
    public AddCategoryCommandValidator()
    {
        RuleFor(x => x.IdContaCorrente)
            .NotEmpty();
    }
}
