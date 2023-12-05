using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class ItemCategory:Entity<int>,IActive
    {
        public ItemCategory()
        {
            BaseItem = new HashSet<BaseItem>();
        }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<BaseItem> BaseItem { get; set; }
    }
}
