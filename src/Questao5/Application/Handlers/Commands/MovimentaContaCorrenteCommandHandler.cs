using Abstractions.Exceptions;
using FluentValidation;
using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Events;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Errors;
using Questao5.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace Questao5.Application.Handlers.Commands;

public sealed class MovimentaContaCorrenteCommandHandler : IRequestHandler<MovimentaContaCorrenteCommand, MovimentarContaCorrenteResponse>
{
    private readonly IMovimentacaoCommandStore _movimentacaoCommandStore;
    private readonly IEventPublisher _eventPublisher;

    public MovimentaContaCorrenteCommandHandler(IMovimentacaoCommandStore movimentacaoCommandStore, IEventPublisher eventPublisher)
    {
        _movimentacaoCommandStore = movimentacaoCommandStore;
        _eventPublisher = eventPublisher;
    }

    public async Task<MovimentarContaCorrenteResponse> Handle(MovimentaContaCorrenteCommand request, CancellationToken cancellationToken)
    {
        var conta = await _movimentacaoCommandStore.ObterContaEMovimentacoesAsync(request.Id, cancellationToken);

        if (conta is null)
            throw new BusinessException(MovimentacaoErrors.ContaCorrenteNaoEncontrada);

        if (conta.Inativa)
            throw new BusinessException(MovimentacaoErrors.ContaCorrenteInativa);

        var idMovimento = conta.FazerMovimentacao(request.TipoMovimentacao.ParseTipoMovimento()
            , request.Valor);

        await _eventPublisher.PublishAsync(conta);

        return MovimentarContaCorrenteResponse.Ok(idMovimento);
    }
}

internal sealed class MovimentaContaCorrenteCommandValidator : AbstractValidator<MovimentaContaCorrenteCommand>
{
    public MovimentaContaCorrenteCommandValidator()
    {
        RuleFor(x => x.RequestId)
            .NotEmpty();

        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Valor)
            .GreaterThan(0)
            .WithMessage(MovimentacaoErrors.ValorMovimentacaoInvalido.Detail);

        RuleFor(x => x.TipoMovimentacao)
            .Matches("^(c|d|cr[eé]dito|d[eé]bito)$", RegexOptions.IgnoreCase)
            .WithMessage(MovimentacaoErrors.TipoMovimentacaoInvalido.Detail);
    }
}
