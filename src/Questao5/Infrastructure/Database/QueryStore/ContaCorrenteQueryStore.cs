using Abstractions.Data;
using Dapper;
using Questao5.Application.Handlers.Interfaces;
using Questao5.Application.Queries.Responses;
using System.Data;

namespace Questao5.Infrastructure.Database.QueryStore;

public sealed class ContaCorrenteQueryStore : IContaCorrenteQueryStore
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public ContaCorrenteQueryStore(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<(bool, bool)> ContaCorrenteExisteEEstaAtivaAsync(string idContaCorrente, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = @"
            SELECT
                ativo
            FROM
                contacorrente
            WHERE
                upper(idcontacorrente) = @Id";

        var param = new DynamicParameters();
        param.Add("@Id", idContaCorrente.ToUpper(), direction: ParameterDirection.Input);

        var result = await connection.QueryFirstOrDefaultAsync<int?>(new CommandDefinition(sql, param, cancellationToken: cancellationToken));

        return (result is not null, result == 1);
    }

    public async Task<ConsultaSaldoQueryResponse> ObterSaldoContaCorrenteAsync(string idContaCorrente, CancellationToken cancellationToken)
    {
        var connection = _sqlConnectionFactory.CreateConnection();

        var param = new DynamicParameters();
        param.Add("@Id", idContaCorrente.ToUpper(), direction: ParameterDirection.Input);

        const string sql = $@"
            SELECT c.numero {nameof(ConsultaSaldoQueryResponse.ContaCorrente)}
                , c.nome {nameof(ConsultaSaldoQueryResponse.Titular)}
                , Sum(case when upper(m.tipomovimento) = 'C' then ifnull(m.Valor, 0) else ifnull(m.Valor, 0) * -1 end) {nameof(ConsultaSaldoQueryResponse.Saldo)}
            FROM
                contacorrente c
            LEFT join movimento m on m.idmovimento = c.idcontacorrente
            WHERE
                c.idcontacorrente = @Id
            GROUP BY c.numero, c.nome";

        var result = await connection.QueryFirstOrDefaultAsync<ConsultaSaldoQueryResponse>(new CommandDefinition(sql, param, cancellationToken: cancellationToken));

        return result ?? new ConsultaSaldoQueryResponse();
    }
}
