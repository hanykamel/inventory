using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class ExchangeOrderStatusTracker : Entity<Guid>, ITenant,IActive
    {
       
        public int ExchangeOrderStatusId { get  ; set  ; }
        public Guid ExchangeOrderId { get; set; }
        public int TenantId { get  ; set  ; }

        public virtual ExchangeOrderStatus ExchangeOrderStatus { get; set; }
        public virtual ExchangeOrder ExchangeOrder { get; set; }
        public bool? IsActive { get ; set ; }
    }
}
