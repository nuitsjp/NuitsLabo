using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions;

namespace DapperStudy.Extensions
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var connection = new SqlConnection(GetConnectionString()))
            {
                connection.Open();

                var purchaseOrderDetail = connection.Get<PurchaseOrderDetail>(new PurchaseOrderDetail { PurchaseOrderID = 1, PurchaseOrderDetailID = 1 });
                Console.WriteLine($"PurchaseOrderID:{purchaseOrderDetail.PurchaseOrderID} PurchaseOrderDetailID:{purchaseOrderDetail.PurchaseOrderDetailID}");
                Console.WriteLine();

                var predicateGroup = new PredicateGroup {Operator = GroupOperator.And, Predicates = new List<IPredicate>()};
                predicateGroup.Predicates.Add(Predicates.Field<PurchaseOrderDetail>(f => f.ProductID, Operator.Eq, 1));
                predicateGroup.Predicates.Add(Predicates.Field<PurchaseOrderDetail>(f => f.PurchaseOrderID, Operator.Ge, 1000));
                var sort = new List<ISort>
                {
                    Predicates.Sort<PurchaseOrderDetail>(x => x.PurchaseOrderID),
                    Predicates.Sort<PurchaseOrderDetail>(x => x.PurchaseOrderDetailID),
                };

                foreach (var detail in
                    connection.GetList<PurchaseOrderDetail>(predicateGroup, sort))
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
