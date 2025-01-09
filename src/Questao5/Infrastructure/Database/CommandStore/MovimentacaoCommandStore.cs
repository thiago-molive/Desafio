using Abstractions.Data;
using Dapper;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Dto;
using System.Data;
using Abstractions.Extensions;

namespace Questao5.Infrastructure.Database.CommandStore;

public class MovimentacaoCommandStore : IMovimentacaoCommandStore
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public MovimentacaoCommandStore(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<ContaCorrente?> ObterContaEMovimentacoesAsync(string Id, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var param = new DynamicParameters();
        param.Add("@Id", Id.ToUpper(), direction: ParameterDirection.Input);

        const string sql = $@"
            SELECT
                c.idcontacorrente {nameof(ContaCorrenteDto.IdContaCorrente)},
                c.numero {nameof(ContaCorrenteDto.Numero)},
                c.nome {nameof(ContaCorrenteDto.Nome)},
                c.ativo {nameof(ContaCorrenteDto.Ativo)},
                m.idmovimento {nameof(MovimentacaoDto.IdMovimento)},
                m.tipomovimento {nameof(MovimentacaoDto.TipoMovimento)},
                m.valor {nameof(MovimentacaoDto.Valor)},
                m.datamovimento {nameof(MovimentacaoDto.Data)}
            FROM
                contacorrente c
            LEFT JOIN movimento m ON m.idcontacorrente = c.idcontacorrente
            WHERE
                c.idcontacorrente = @Id";

        var result = await connection.QueryAsync<ContaCorrenteDto, MovimentacaoDto, ContaCorrenteDto>(
            sql,
            (contaCorrente, movimento) =>
            {
                contaCorrente.Movimentos.Add(movimento);
                return contaCorrente;
            },
            param,
            splitOn: $"{nameof(MovimentacaoDto.IdMovimento)}");

        return result?.FirstOrDefault()?.MapToEntity();
    }

    public async Task<bool> SaveMovimentacaoAsync(MovimentoConta movimentacaoEntity, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        var param = new DynamicParameters();
        param.Add("@idmovimento", movimentacaoEntity.Id.ToString().ToUpper(), direction: ParameterDirection.Input);
        param.Add("@IdContaCorrente", movimentacaoEntity.IdContaCorrente.ToString().ToUpper(), direction: ParameterDirection.Input);
        param.Add("@DataMovimento", movimentacaoEntity.DataMovimento, direction: ParameterDirection.Input);
        param.Add("@TipoMovimento", movimentacaoEntity.TipoMovimento.GetName(), direction: ParameterDirection.Input);
        param.Add("@Valor", movimentacaoEntity.Valor, direction: ParameterDirection.Input);

        const string sql = @"
            INSERT INTO movimento
            (
                idmovimento,
                idcontacorrente,
                datamovimento,
                tipomovimento,
                valor
            )
            VALUES
            (
                @idmovimento,
                @IdContaCorrente,
                @DataMovimento,
                @TipoMovimento,
                @Valor
            )";

        var result = await connection.ExecuteAsync(sql, param);

        return result == 1;
    }
}
