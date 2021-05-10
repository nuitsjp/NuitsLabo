namespace DddAdventureWorks
{
    public readonly struct SalesOrderDetailKey
    {
        private readonly SalesOrderID _salesOrderId;

        private readonly SalesOrderDetailID _salesOrderDetailId;

        public SalesOrderDetailKey(SalesOrderID salesOrderId, SalesOrderDetailID salesOrderDetailId)
        {
            _salesOrderId = salesOrderId;
            _salesOrderDetailId = salesOrderDetailId;
        }

        public static SalesOrderID GetSalesOrderID(SalesOrderDetailKey salesOrderDetailKey) => salesOrderDetailKey._salesOrderId;
        public static SalesOrderDetailID GetSalesOrderDetailID(SalesOrderDetailKey salesOrderDetailKey) => salesOrderDetailKey._salesOrderDetailId;
    }
}