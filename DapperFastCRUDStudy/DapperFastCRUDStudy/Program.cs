using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();
                var purchaseOrderDetail = connection.Get(new PurchaseOrderDetail { PurchaseOrderID = 1, PurchaseOrderDetailID = 1 });

                foreach (var detail in
                    connection.Find<PurchaseOrderDetail>(statement => statement
                        .Where($"{nameof(PurchaseOrderDetail.ProductID):C}=@ProductID")
                        .OrderBy($"{nameof(PurchaseOrderDetail.PurchaseOrderID)}, {nameof(PurchaseOrderDetail.PurchaseOrderDetailID)}")
                        .WithParameters(new { ProductID = 1 })))
                {
                    Console.WriteLine($"PurchaseOrderID:{detail.PurchaseOrderID} PurchaseOrderDetailID:{detail.PurchaseOrderDetailID}");
                }
            }
            Console.ReadLine();
        }

        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["AdventureWorks2017"].ConnectionString;
        }

    }
}
