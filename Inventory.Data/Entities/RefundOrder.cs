using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class RefundOrder : Entity<Guid>,IActive,ITenant,ICode
    {
        public RefundOrder()
        {
            RefundOrderStoreItem = new HashSet<RefundOrderStoreItem>();
        }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public int RefundOrderEmployeeId { get; set; }
        public int OperationId { get; set; }
        public int ExaminationEmployeeId { get; set; }
        public int? RefundOrderStatusId { get; set; }
        public string Notes { get; set; }
        public string DirectOrderNotes { get; set; }
        public bool IsDirectOrder { get; set; }
        public bool? IsActive { get; set; }
        public virtual RefundOrderStatus RefundOrderStatus { get; set; }
        public virtual Employees ExaminationEmployee { get; set; }
        public virtual Employees RefundOrderEmployee { get; set; }

        public virtual Operation Operation { get; set; }
        public virtual ICollection<RefundOrderStoreItem> RefundOrderStoreItem { get; set; }
        public virtual ICollection<RefundOrderStatusTracker> RefundOrderStatusTracker { get; set; }
        public virtual ICollection<RefundOrderAttachment> RefundOrderAttachment { get; set; }
        public int TenantId { get ; set; }
        public int Year { get; set; }
        public int Serial { get ; set ; }
    }
}
