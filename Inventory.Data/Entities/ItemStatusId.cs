using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class ItemStatus
    {
        public ItemStatus()
        {
            StoreItem = new HashSet<StoreItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<StoreItem> StoreItem { get; set; }
    }
}
