using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class Unit:Entity<int>,IActive
    {
        public Unit()
        {
            BaseItem = new HashSet<BaseItem>();
            StoreItem = new HashSet<StoreItem>();
            RobbedStoreItem = new HashSet<RobbedStoreItem>();

        }
        public bool? IsActive { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BaseItem> BaseItem { get; set; }
        public virtual ICollection<StoreItem> StoreItem { get; set; }
        public virtual ICollection<RobbedStoreItem> RobbedStoreItem { get; set; }

    }
}
