using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperStudy.Dommel
{
    /// <summary>
    /// A class which represents the PurchaseOrderDetail table.
    /// </summary>
    [Table("PurchaseOrderDetail", Schema = "Purchasing")]
    public partial class PurchaseOrderDetail
    {
        [Key]
        public virtual int PurchaseOrderID { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int PurchaseOrderDetailID { get; set; }
        public virtual DateTime DueDate { get; set; }
        public virtual short OrderQty { get; set; }
        public virtual int ProductID { get; set; }
        public virtual decimal UnitPrice { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual decimal LineTotal { get; set; }
        public virtual decimal ReceivedQty { get; set; }
        public virtual decimal RejectedQty { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual decimal StockedQty { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
    }
}
