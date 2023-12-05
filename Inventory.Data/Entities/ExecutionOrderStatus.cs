using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public class ExecutionOrderStatus : Entity<int>, IActive
    {
        public ExecutionOrderStatus()
        {
            ExecutionOrder = new HashSet<ExecutionOrder>();
            ExecutionOrderStatusTracker = new HashSet<ExecutionOrderStatusTracker>();

        }

        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<ExecutionOrder> ExecutionOrder { get; set; }
        public virtual ICollection<ExecutionOrderStatusTracker> ExecutionOrderStatusTracker { get; set; }
    }
}
