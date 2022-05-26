using System.Data;
using System.Text;
using Dapper;

namespace T4SqlTemplate;

public abstract class QueryAsyncBase<T> : IQueryAsync<T>
{
    protected StringBuilder GenerationEnvironment { get; } = new();

    public abstract string TransformText();

    public void Write(string text)
    {
        GenerationEnvironment.Append(text);
    }

    public class ToStringInstanceHelper
    {
        public string ToStringWithCulture(object objectToConvert)
        {
            return (string)objectToConvert;
        }
    }

    public ToStringInstanceHelper ToStringHelper { get; } = new();

    public Task<IEnumerable<T>> QueryAsync(
        IDbConnection cnn, 
        object? param = null, 
        IDbTransaction? transaction = null, 
        int? commandTimeout = null, 
        CommandType? commandType = null)
    {
        return cnn.QueryAsync<T>(TransformText(), this, transaction, commandTimeout, commandType);
    }
}

public interface IQueryAsync<T>
{
    public Task<IEnumerable<T>> QueryAsync(
        IDbConnection cnn,
        object? param = null,
        IDbTransaction? transaction = null,
        int? commandTimeout = null,
        CommandType? commandType = null);
}