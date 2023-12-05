using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public class RefundOrderStatus : Entity<int>,IActive
    {
        public RefundOrderStatus()
        {
            RefundOrder = new HashSet<RefundOrder>();
            RefundOrderStatusTracker = new HashSet<RefundOrderStatusTracker>();

        }

        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<RefundOrder> RefundOrder { get; set; }
        public virtual ICollection<RefundOrderStatusTracker> RefundOrderStatusTracker { get; set; }
    }
}
