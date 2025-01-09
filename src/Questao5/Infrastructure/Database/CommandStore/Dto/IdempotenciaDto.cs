using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.CommandStore.Dto;

internal sealed class IdempotenciaDto
{
    public string Idempotencia { get; set; }
    public string Request { get; set; }
    public string Response { get; set; }

    public static IdempotencyEntity MapToEntity(IdempotenciaDto dto) =>
        IdempotencyEntity.Create(dto.Idempotencia, dto.Request, dto.Response);
}
