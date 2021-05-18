using System;
using System.Linq;
using EFAdventureWorks.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace EFAdventureWorks
{
    class Program
    {
        static void Main(string[] args)
        {
            using var context = new AdventureWorksContext();
            var salesOrderDetail = context.SalesOrderDetails.First();
        }
    }

    //public class AdventureWorksContext : DbContext
    //{
    //    public DbSet<SalesOrderDetail> SalesOrderDetails { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        optionsBuilder.UseSqlServer(
    //            new SqlConnectionStringBuilder
    //            {
    //                DataSource = "localhost",
    //                UserID = "sa",
    //                Password = "P@ssw0rd!",
    //                InitialCatalog = "AdventureWorks"
    //            }.ToString());
    //        base.OnConfiguring(optionsBuilder);
    //    }
    //}

    //public class SalesOrderDetail
    //{
    //    public SalesOrderDetail(int salesOrderId, int salesOrderDetailId, string carrierTrackingNumber, short orderQty, int productId, int specialOfferId, decimal unitPrice, decimal unitPriceDiscount, decimal lineTotal, DateTime modifiedDate)
    //    {
    //        SalesOrderID = salesOrderId;
    //        SalesOrderDetailID = salesOrderDetailId;
    //        CarrierTrackingNumber = carrierTrackingNumber;
    //        OrderQty = orderQty;
    //        ProductId = productId;
    //        SpecialOfferId = specialOfferId;
    //        UnitPrice = unitPrice;
    //        UnitPriceDiscount = unitPriceDiscount;
    //        LineTotal = lineTotal;
    //        ModifiedDate = modifiedDate;
    //    }

    //    public int SalesOrderID { get; }
    //    public int SalesOrderDetailID { get; }
    //    public string CarrierTrackingNumber { get; }
    //    public short OrderQty { get; }
    //    public int ProductId { get; }
    //    public int SpecialOfferId { get; }
    //    public decimal UnitPrice { get; }
    //    public decimal UnitPriceDiscount { get; }
    //    public decimal LineTotal { get; }
    //    public DateTime ModifiedDate { get; }
    //}

}
