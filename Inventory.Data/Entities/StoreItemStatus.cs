using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class StoreItemStatus
    {
        public StoreItemStatus()
        {
            StoreItem = new HashSet<StoreItem>();
            StoreItemCopy = new HashSet<StoreItemCopy>();
            RefundOrderStoreItem = new HashSet<RefundOrderStoreItem>();
            RobbingOrderStoreItem=new HashSet<RobbingOrderStoreItem>();

    }

    public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<StoreItem> StoreItem { get; set; }
        public virtual ICollection<StoreItemCopy> StoreItemCopy { get; set; }
        public virtual ICollection<RobbingOrderStoreItem> RobbingOrderStoreItem { get; set; }

        
        public virtual ICollection<RefundOrderStoreItem> RefundOrderStoreItem { get; set; }

    }
}
