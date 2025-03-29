using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace IntegrationTestStudy.Test;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    {
        await new QueryService().DeleteCustomerAsync();

        // Assert
        // Customerテーブルが9件になっていることを確認する
    }
}

public class TestContextProvider<TDbContext> : IDbContextProvider<TDbContext> where TDbContext : DbContext, new()
{

    public TDbContext Provide()
    {
        return new TDbContext();
        //var proxy = _proxyGenerator.CreateClassProxyWithTarget(dbContext, _interceptor);
        //return proxy;
    }
}

public class DbContextTransactionProxy(IDbContextTransaction transaction) : IDbContextTransaction
{
    public void Dispose()
    {
        transaction.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        return transaction.DisposeAsync();
    }

    public void Commit()
    {
        transaction.Commit();
    }

    public Task CommitAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return transaction.CommitAsync(cancellationToken);
    }

    public void Rollback()
    {
        transaction.Rollback();
    }

    public Task RollbackAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        return transaction.RollbackAsync(cancellationToken);
    }

    public Guid TransactionId => transaction.TransactionId;
}
