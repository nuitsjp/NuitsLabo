using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace IntegrationTestStudy;

public class QueryService(IDbContextProvider<SqlBulkCopierDbContext> dbContextProvider)
{
    public async Task DeleteCustomerAsync()
    {
        await using SqlBulkCopierDbContext context = dbContextProvider.Provide();
        var transaction = await context.Database.BeginTransactionAsync();

        context.Customers.Remove(context.Customers.First());
        await context.SaveChangesAsync();

        await transaction.CommitAsync();
    }
}

public interface IDbContextProvider<out TDbContext> where TDbContext : DbContext, new()
{
    TDbContext Provide();
}

public class DbContextProvider<TDbContext> : IDbContextProvider<TDbContext> where TDbContext : DbContext, new()
{
    public TDbContext Provide() => new();
}

public class SqlBulkCopierDbContext : DbContextBase
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = new SqlConnectionStringBuilder
        {
            DataSource = ".",
            InitialCatalog = "SqlBulkCopier",
            IntegratedSecurity = true,
            TrustServerCertificate = true
        }.ToString();
        optionsBuilder.UseSqlServer(connectionString);
    }

    public DbSet<Customer> Customers { get; set; }
}

public class DatabaseProxy(DbContext context, IDbContextTransaction? baseTransaction) : DatabaseFacade(context)
{
    public override async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = new())
    {
        if (baseTransaction is not null)
        {
            return baseTransaction;
        }
        return await base.BeginTransactionAsync(cancellationToken);
    }
}

public abstract class DbContextBase : DbContext
{
    private DatabaseFacade? _databaseFacade;
    public override DatabaseFacade Database => _databaseFacade ??= new DatabaseProxy(this, null);
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
        // transaction.Commit();
    }

    public Task CommitAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        // return transaction.CommitAsync(cancellationToken);
        return Task.CompletedTask;
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
