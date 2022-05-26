using System.Data;

namespace Dapper.QueryTemplate;

public abstract class QueryAsyncTemplate<T> : QueryTemplateBase, IQueryAsync<T>
{
    public Task<IEnumerable<T>> QueryAsync(
        IDbConnection connection, 
        object? param = null, 
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null)
        => connection.QueryAsync<T>(TransformText(), this, transaction, commandTimeout, commandType);
}