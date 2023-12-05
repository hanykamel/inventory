using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class ExchangeOrderStoreItem:Entity<Guid>,IActive
    {
        public Guid ExchangeOrderId { get; set; }
        public Guid StoreItemId { get; set; }
        public string Notes { get; set; }
        public bool? IsActive { get; set; }
        public virtual ExchangeOrder ExchangeOrder { get; set; }
        public virtual StoreItem StoreItem { get; set; }
    }
}
