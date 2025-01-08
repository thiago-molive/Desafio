using Abstractions.Data;
using Questao5.Infrastructure.Sqlite;
using System.Data;
using System.Data.SQLite;

namespace Questao5.Infrastructure.Providers.DapperProvider;

internal sealed class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
{
    private readonly DatabaseConfig databaseConfig;
    private IDbConnection _connection;

    public SqlConnectionFactory(DatabaseConfig databaseConfig)
    {
        this.databaseConfig = databaseConfig;
    }

    public IDbConnection CreateConnection()
    {
        _connection = new SQLiteConnection(databaseConfig.Name);
        _connection.Open();

        return _connection;
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}
