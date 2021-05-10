using System;

namespace DddAdventureWorks
{
    public class SalesOrderDetail
    {
        public SalesOrderDetail(
            SalesOrderID salesOrderId,
            SalesOrderDetailID salesOrderDetailId,
            string carrierTrackingNumber, 
            short orderQty, 
            int productId, 
            int specialOfferId,
            decimal unitPrice,
            decimal unitPriceDiscount,
            decimal lineTotal, 
            DateTime modifiedDate)
        {
            SalesOrderID = salesOrderId;
            SalesOrderDetailID = salesOrderDetailId;
            CarrierTrackingNumber = carrierTrackingNumber;
            OrderQty = orderQty;
            ProductID = productId;
            SpecialOfferID = specialOfferId;
            UnitPrice = unitPrice;
            UnitPriceDiscount = unitPriceDiscount;
            LineTotal = lineTotal;
            ModifiedDate = modifiedDate;
        }

        public SalesOrderID SalesOrderID { get; }
        public SalesOrderDetailID SalesOrderDetailID { get; }
        //public SalesOrderDetailKey Key { get; }
        public string CarrierTrackingNumber { get; }
        public short OrderQty { get; }
        public int ProductID { get; }
        public int SpecialOfferID { get; }
        public decimal UnitPrice { get; }
        public decimal UnitPriceDiscount { get; }
        public decimal LineTotal { get; }
        public DateTime ModifiedDate { get; }
    }
}
