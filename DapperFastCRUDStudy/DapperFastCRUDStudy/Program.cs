using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.FastCrud;
using DapperFastCRUDStudy.Models;

namespace DapperFastCRUDStudy
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = ConfigurationManager.ConnectionStrings["AdventureWorks2017"];
            var factory = DbProviderFactories.GetFactory(settings.ProviderName);
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = settings.ConnectionString;
                connection.Open();

                //var purchaseOrderDetail = connection.Get(new PurchaseOrderDetail { PurchaseOrderID = 1, PurchaseOrderDetailID = 1 });

                //foreach (var detail in
                //    connection.Find<PurchaseOrderDetail>(statement => statement
                //        .Where($"{nameof(PurchaseOrderDetail.ProductID):C}=@ProductID")
                //        .OrderBy($"{nameof(PurchaseOrderDetail.PurchaseOrderID)}, {nameof(PurchaseOrderDetail.PurchaseOrderDetailID)}")
                //        .WithParameters(new { ProductID = 1 })))
                //{
                //    Console.WriteLine($"PurchaseOrderID:{detail.PurchaseOrderID} PurchaseOrderDetailID:{detail.PurchaseOrderDetailID}");
                //}
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var values = connection.Find<SalesOrderHeaderDetail>(statement => statement
                    .OrderBy($"{nameof(SalesOrderHeaderDetail.SalesOrderID)}")
                    .Top(10000)).ToList();
                stopwatch.Stop();
                Console.WriteLine($"Elapsed:{stopwatch.Elapsed}");
            }
            Console.ReadLine();
        }
    }
}
