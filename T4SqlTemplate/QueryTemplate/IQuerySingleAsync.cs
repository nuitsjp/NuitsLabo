using System.Data;

namespace Dapper.QueryTemplate;

public interface IQuerySingleAsync<T>
{
    public Task<T> QuerySingleAsync(
        IDbConnection connection,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null);
}