using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class RefundOrderStatusTracker : Entity<Guid>, ITenant,IActive
    {
       
        public int RefundOrderStatusId { get  ; set  ; }
        public Guid RefundOrderId { get; set; }
        public int TenantId { get  ; set  ; }

        public virtual RefundOrderStatus RefundOrderStatus { get; set; }
        public virtual RefundOrder RefundOrder { get; set; }
        public bool? IsActive { get ; set ; }
    }
}
