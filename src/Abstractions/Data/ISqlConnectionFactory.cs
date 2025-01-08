using System.Data;

namespace Abstractions.Data;

public interface ISqlConnectionFactory : IDisposable
{
    IDbConnection CreateConnection();
}
