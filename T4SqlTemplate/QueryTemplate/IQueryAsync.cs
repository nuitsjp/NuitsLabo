using System.Data;

namespace Dapper.QueryTemplate;

public interface IQueryAsync<T>
{
    public Task<IEnumerable<T>> QueryAsync(
        IDbConnection connection,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null);
}