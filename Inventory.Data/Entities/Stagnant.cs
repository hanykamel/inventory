using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class Stagnant:Entity<Guid>,ITenant,IActive
    {
        public Stagnant()
        {
            StagnantAttachment = new HashSet<StagnantAttachment>();
            StagnantStoreItem = new HashSet<StagnantStoreItem>();

        }
        public DateTime DateFrom { get; set; }
        public int TenantId { get; set ; }
        public bool? IsActive { get; set; }
        public virtual ICollection<StagnantAttachment> StagnantAttachment { get; set; }
        public virtual ICollection<StagnantStoreItem> StagnantStoreItem { get; set; }
    }
}
