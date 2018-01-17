using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;

namespace DapperStudy.Extensions
{
    public class PurchaseOrderDetailMapper : AutoClassMapper<PurchaseOrderDetail>
    {
        public PurchaseOrderDetailMapper()
        {
            Schema("Purchasing");
        }
    }
}
