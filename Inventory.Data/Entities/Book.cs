using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class Book:Entity<long>,ITenant,IActive
    {
        public Book()
        {
            StoreItem = new HashSet<StoreItem>();
            RobbedStoreItem = new HashSet<RobbedStoreItem>();

        }

        public int PageCount { get; set; }
        public int BookNumber { get; set; }
        public bool Consumed { get; set; }
        public int StoreId { get; set; }
        public int TenantId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<StoreItem> StoreItem { get; set; }
        public virtual ICollection<RobbedStoreItem> RobbedStoreItem { get; set; }

    }
}
