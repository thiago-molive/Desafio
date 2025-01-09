using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.CommandStore.Dto;

internal sealed class ContaCorrenteDto
{
    public string IdContaCorrente { get; set; }
    public long Numero { get; set; }
    public string Nome { get; set; }
    public int Ativo { get; set; }

    public IList<MovimentacaoDto> Movimentos { get; set; } = new List<MovimentacaoDto>();

    public ContaCorrente? MapToEntity()
    {
        return ContaCorrente.Restore(Guid.Parse(IdContaCorrente)
        , Numero
        , Nome
        , Ativo == 1
        , Movimentos?.Select(MovimentacaoDto.MapToEntity) ?? Enumerable.Empty<MovimentoConta>());
    }
}

internal sealed class MovimentacaoDto
{
    public string IdMovimento { get; set; }
    public string TipoMovimento { get; set; }
    public decimal Valor { get; set; }
    public DateTime Data { get; set; }

    public static MovimentoConta MapToEntity(MovimentacaoDto dto)
    {
        if (dto is null)
            return default;
        return MovimentoConta.Restore(Guid.Parse(dto.IdMovimento)
        , dto.TipoMovimento
        , dto.Valor
        , dto.Data);
    }
}

