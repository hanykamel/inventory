using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class StockTakingStoreItem:Entity<Guid>,IActive
    {
        public Guid StockTakingId { get; set; }
        public Guid StoreItemId { get; set; }
        public bool? IsActive { get; set; }
        public virtual StockTaking StockTaking { get; set; }
        public virtual StoreItem StoreItem { get; set; }
    }
}
