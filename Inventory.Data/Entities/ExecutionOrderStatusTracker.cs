using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class ExecutionOrderStatusTracker : Entity<Guid>, ITenant,IActive
    {
       
        public int ExecutionOrderStatusId { get  ; set  ; }
        public Guid ExecutionOrderId { get; set; }
        public int TenantId { get  ; set  ; }

        public virtual ExecutionOrderStatus ExecutionOrderStatus { get; set; }
        public virtual ExecutionOrder ExecutionOrder { get; set; }
        public bool? IsActive { get ; set ; }
    }
}
