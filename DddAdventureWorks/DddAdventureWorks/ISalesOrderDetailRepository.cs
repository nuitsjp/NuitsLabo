namespace DddAdventureWorks
{
    public interface ISalesOrderDetailRepository
    {
        SalesOrderDetail GetSalesOrderDetail(int salesOrderID, int salesOrderDetailID);
    }
}