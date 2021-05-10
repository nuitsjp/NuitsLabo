using System;
using Dapper;

namespace DddAdventureWorks.Repository
{
    public class SalesOrderDetailRepository : ISalesOrderDetailRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public SalesOrderDetailRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;

        }

        public SalesOrderDetail GetSalesOrderDetail(int salesOrderID, int salesOrderDetailID)
        {
            using var connection = _dbConnectionFactory.Open();

            SqlMapper.AddTypeHandler(new SalesOrderIDTypeHandler());
            SqlMapper.AddTypeHandler(new SalesOrderDetailIDTypeHandler());

            return connection.QuerySingle<SalesOrderDetail>(@"
select 
	[SalesOrderID],
	[SalesOrderDetailID],
	[CarrierTrackingNumber],
	[OrderQty],
	[ProductID],
	[SpecialOfferID],
	[UnitPrice],
	[UnitPriceDiscount],
	[LineTotal],
	[ModifiedDate]
from 
	[AdventureWorks].[Sales].[SalesOrderDetail]
where
    [SalesOrderID] = @SalesOrderID
    and [SalesOrderDetailID] = @SalesOrderDetailID
",
                new
                {
                    SalesOrderID = salesOrderID,
                    SalesOrderDetailID = salesOrderDetailID
                });
        }
    }

    public class SalesOrderIDTypeHandler : SqlMapper.TypeHandler<SalesOrderID>
    {
        public override SalesOrderID Parse(object value)
        {
            return new SalesOrderID((int)value);
        }

        public override void SetValue(System.Data.IDbDataParameter parameter, SalesOrderID value)
        {
            parameter.Value = value.Value;
        }
    }

    public class SalesOrderDetailIDTypeHandler : SqlMapper.TypeHandler<SalesOrderDetailID>
    {
        public override SalesOrderDetailID Parse(object value)
        {
            return new SalesOrderDetailID((int)value);
        }

        public override void SetValue(System.Data.IDbDataParameter parameter, SalesOrderDetailID value)
        {
            parameter.Value = value.Value;
        }
    }
}
