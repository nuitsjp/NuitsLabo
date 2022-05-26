using System.Data;

namespace Dapper.QueryTemplate;

public abstract class QuerySingleTemplate<T> : QueryTemplateBase, IQuerySingleAsync<T>
{
    public Task<T> QuerySingleAsync(
        IDbConnection connection, 
        IDbTransaction? transaction = null,
        int? commandTimeout = null, 
        CommandType? commandType = null)
        => connection.QuerySingleAsync<T>(TransformText(), this, transaction, commandTimeout, commandType);
}