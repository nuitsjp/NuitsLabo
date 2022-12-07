using System;
using System.Collections;
using System.Collections.Generic;
using MagicOnion;
using MessagePack;

namespace MagicOnionStudy
{
    public interface IMyFirstService : IService<IMyFirstService>
    {
        // The return type must be `UnaryResult<T>`.
        UnaryResult<PurchaseOrderHeader> SumAsync(int x, int y);
    }

    [MessagePackObject]
    public class PurchaseOrderHeader
    {
        [Key(0)]
        public int Result { get; set; }

        [Key(1)]
        public IList<PurchaseOrderDetail> PurchaseOrderDetails { get; set; } = Array.Empty<PurchaseOrderDetail>();
    }

    [MessagePackObject]
    public class PurchaseOrderDetail
    {

    }
}