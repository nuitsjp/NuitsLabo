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
    //                DataSource = "localhost, 1444",
    //                UserID = "sa",
    //                Password = "P@ssw0rd!",
    //                InitialCatalog = "AdventureWorks"
    //            }.ToString());
    //        base.OnConfiguring(optionsBuilder);
    //    }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<SalesOrderDetail>(entity =>
    //        {
    //            entity.HasKey(e => new { e.SalesOrderId, e.SalesOrderDetailId })
    //                .HasName("PK_SalesOrderDetail_SalesOrderID_SalesOrderDetailID");

    //            entity.ToTable("SalesOrderDetail", "Sales");

    //            entity.HasComment("Individual products associated with a specific sales order. See SalesOrderHeader.");

    //            entity.HasIndex(e => e.Rowguid, "AK_SalesOrderDetail_rowguid")
    //                .IsUnique();

    //            entity.HasIndex(e => e.ProductId, "IX_SalesOrderDetail_ProductID");

    //            entity.Property(e => e.SalesOrderId)
    //                .HasColumnName("SalesOrderID")
    //                .HasComment("Primary key. Foreign key to SalesOrderHeader.SalesOrderID.");

    //            entity.Property(e => e.SalesOrderDetailId)
    //                .ValueGeneratedOnAdd()
    //                .HasColumnName("SalesOrderDetailID")
    //                .HasComment("Primary key. One incremental unique number per product sold.");

    //            entity.Property(e => e.CarrierTrackingNumber)
    //                .HasMaxLength(25)
    //                .HasComment("Shipment tracking number supplied by the shipper.");

    //            entity.Property(e => e.LineTotal)
    //                .HasColumnType("numeric(38, 6)")
    //                .HasComputedColumnSql("(isnull(([UnitPrice]*((1.0)-[UnitPriceDiscount]))*[OrderQty],(0.0)))", false)
    //                .HasComment("Per product subtotal. Computed as UnitPrice * (1 - UnitPriceDiscount) * OrderQty.");

    //            entity.Property(e => e.ModifiedDate)
    //                .HasColumnType("datetime")
    //                .HasDefaultValueSql("(getdate())")
    //                .HasComment("Date and time the record was last updated.");

    //            entity.Property(e => e.OrderQty).HasComment("Quantity ordered per product.");

    //            entity.Property(e => e.ProductId)
    //                .HasColumnName("ProductID")
    //                .HasComment("Product sold to customer. Foreign key to Product.ProductID.");

    //            entity.Property(e => e.Rowguid)
    //                .HasColumnName("rowguid")
    //                .HasDefaultValueSql("(newid())")
    //                .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.");

    //            entity.Property(e => e.SpecialOfferId)
    //                .HasColumnName("SpecialOfferID")
    //                .HasComment("Promotional code. Foreign key to SpecialOffer.SpecialOfferID.");

    //            entity.Property(e => e.UnitPrice)
    //                .HasColumnType("money")
    //                .HasComment("Selling price of a single product.");

    //            entity.Property(e => e.UnitPriceDiscount)
    //                .HasColumnType("money")
    //                .HasComment("Discount amount.");

    //            entity.HasOne(d => d.SalesOrder)
    //                .WithMany(p => p.SalesOrderDetails)
    //                .HasForeignKey(d => d.SalesOrderId);

    //            entity.HasOne(d => d.SpecialOfferProduct)
    //                .WithMany(p => p.SalesOrderDetails)
    //                .HasForeignKey(d => new { d.SpecialOfferId, d.ProductId })
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK_SalesOrderDetail_SpecialOfferProduct_SpecialOfferIDProductID");
    //        });
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
