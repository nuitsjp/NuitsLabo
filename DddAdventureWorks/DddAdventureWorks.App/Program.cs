using System;
using System.ComponentModel;
using System.Drawing;
using DddAdventureWorks.Repository;
using Microsoft.Data.SqlClient;

namespace DddAdventureWorks.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var salesOrderId = (ISalesOrderID)TypeDescriptor.GetConverter(typeof(ISalesOrderID)).ConvertFromString("1");

            var connectionString = new SqlConnectionStringBuilder
            {
                DataSource = "localhost, 1444",
                UserID = "sa",
                Password = "P@ssw0rd!",
                InitialCatalog = "AdventureWorks"
            }.ToString();
            var repository = new SalesOrderDetailRepository(new SqlConnectionFactory(connectionString));
            var salesOrderDetail = repository.GetSalesOrderDetail(43659, 1);
            Console.WriteLine(salesOrderDetail);
        }
    }
}
