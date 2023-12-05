using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class ExchangeOrderStatus:Entity<int>,IActive
    {
        public ExchangeOrderStatus()
        {
            ExchangeOrder = new HashSet<ExchangeOrder>();
        }

        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<ExchangeOrder> ExchangeOrder { get; set; }
        public virtual ICollection<ExchangeOrderStatusTracker> ExchangeOrderStatusTracker { get; set; }

    }
}
