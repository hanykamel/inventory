using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class StagnantStoreItem:Entity<Guid>,IActive,ITenant
    {
        public Guid StagnantId { get; set; }
        public Guid StoreItemId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Stagnant Stagnant { get; set; }
        public virtual StoreItem StoreItem { get; set; }
        public int TenantId { get ; set; }
    }
}
