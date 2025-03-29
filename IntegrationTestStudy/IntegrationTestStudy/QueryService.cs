using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace IntegrationTestStudy;

public class QueryService
{
    public async Task DeleteCustomerAsync()
    {
        await using SqlBulkCopierDbContext context = new();
        var transaction = await context.Database.BeginTransactionAsync();

        context.Customers.RemoveRange(context.Customers);
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

public class DatabaseProxy(DbContext context) : DatabaseFacade(context)
{
    public override async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var transaction = await base.BeginTransactionAsync(cancellationToken);
        return new DbContextTransactionProxy(transaction);
    }
}

public abstract class DbContextBase : DbContext
{
    private DatabaseFacade? _databaseFacade;
    public override DatabaseFacade Database => _databaseFacade ??= new DatabaseProxy(this);
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
