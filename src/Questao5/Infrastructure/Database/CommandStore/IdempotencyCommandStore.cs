using Abstractions.Data;
using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Dto;
using System.Data;

namespace Questao5.Infrastructure.Database.CommandStore;

public sealed class IdempotencyCommandStore : IIdempotencyCommandStore
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public IdempotencyCommandStore(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<IdempotencyEntity?> GetRequestByKeyAsync(string key, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var param = new DynamicParameters();
        param.Add("@Key", key.ToUpper(), direction: ParameterDirection.Input);

        const string sql = $@"
            SELECT
                chave_idempotencia {nameof(IdempotenciaDto.Idempotencia)},
                requisicao {nameof(IdempotenciaDto.Request)},
                resultado {nameof(IdempotenciaDto.Response)}
            FROM
                idempotencia
            WHERE
                chave_idempotencia = @Key";

        var idempotencia = await connection.QueryFirstOrDefaultAsync<IdempotenciaDto>(sql, param);

        return idempotencia is null ? null : IdempotenciaDto.MapToEntity(idempotencia);
    }

    public async Task SaveRequestAsync(IdempotencyEntity idempotencyEntity, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var param = new DynamicParameters();
        param.Add("@Id", idempotencyEntity.Id, direction: ParameterDirection.Input);
        param.Add("@Request", idempotencyEntity.Requisicao, direction: ParameterDirection.Input);

        const string sql = @"
            INSERT INTO idempotencia
                (chave_idempotencia, requisicao)
            VALUES
                (@Id, @Request)";

        await connection.ExecuteAsync(sql, param);
    }

    public async Task MarkRequestAsProcessedAsync(IdempotencyEntity entity, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var param = new DynamicParameters();
        param.Add("@Key", entity.Id.ToString().ToUpper(), direction: ParameterDirection.Input);
        param.Add("@Response", entity.Resultado, direction: ParameterDirection.Input);

        const string sql = @"
            UPDATE idempotencia
            SET
                resultado = @Response
            WHERE
                chave_idempotencia = @Key";

        await connection.ExecuteAsync(sql, param);
    }
}
