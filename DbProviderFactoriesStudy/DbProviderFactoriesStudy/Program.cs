// See https://aka.ms/new-console-template for more information
using System.Data.Common;
using Microsoft.Data.SqlClient;

Console.WriteLine("Hello, World!");

DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
var factory = DbProviderFactories.GetFactory("Microsoft.Data.SqlClient");
using var connection = factory.CreateConnection()!;

connection.ConnectionString = "settings.ConnectionString";
connection.Open();