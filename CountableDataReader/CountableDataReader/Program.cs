using System.Data.SqlClient;
using System;using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

using var source = OpenSourceConnection();
using var destination = OpenDestinationConnection();
// １回目はDropFKでエラーになるので２回投げる
TruncateDestinationTable(destination);
TruncateDestinationTable(destination);


using var command = source.CreateCommand();
command.CommandText = @"

select 
    * 
from 
    Purchasing.Vendor
order by
    BusinessEntityID";
command.Connection = source;

using var transaction = destination.BeginTransaction();

using var reader = new CountableSqlDataReader(command.ExecuteReader());
reader
    .Where(x => x % 10 == 0)
    .Subscribe(x =>
    {
        Console.WriteLine($"count:{x}");
    });

using var bulkCopy = new SqlBulkCopy(destination, SqlBulkCopyOptions.KeepIdentity, transaction);
bulkCopy.DestinationTableName = "Purchasing.Vendor";
bulkCopy.BulkCopyTimeout = 0;
bulkCopy.WriteToServer(reader);

transaction.Commit();





static IDbConnection OpenSourceConnection()
{
    var connectionString = new SqlConnectionStringBuilder
    {
        DataSource = "localhost",
        InitialCatalog = "AdventureWorks",
        UserID = "sa",
        Password = "P@ssw0rd!"
    }.ToString();

    var connection = new SqlConnection(connectionString);
    connection.Open();
    return connection;
}

static SqlConnection OpenDestinationConnection()
{
    var connectionString = new SqlConnectionStringBuilder
    {
        DataSource = "localhost, 1444",
        UserID = "sa",
        Password = "P@ssw0rd!"
    }.ToString();

    var connection = new SqlConnection(connectionString);
    connection.Open();
    return connection;
}

static void TruncateDestinationTable(IDbConnection connection)
{
    try
    {
        using var command = connection.CreateCommand();
        command.Connection = connection;
        command.CommandText = CountableDataReader.Properties.Resources.TruncateTable;
        command.ExecuteNonQuery();
    }
    catch
    {
        // ignore
    }
}