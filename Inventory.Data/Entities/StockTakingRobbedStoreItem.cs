using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class StockTakingRobbedStoreItem : Entity<Guid>,IActive
    {
        public Guid StockTakingId { get; set; }
        public Guid RobbedStoreItemId { get; set; }
        public bool? IsActive { get; set; }
        public virtual StockTaking StockTaking { get; set; }
        public virtual RobbedStoreItem RobbedStoreItem { get; set; }
    }
}
