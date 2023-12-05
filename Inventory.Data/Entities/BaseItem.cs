using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class BaseItem:Entity<long>,IActive
    {
        public BaseItem()
        {
            CommitteeItem = new HashSet<CommitteeItem>();
            StoreItem = new HashSet<StoreItem>();
            RobbedStoreItem = new HashSet<RobbedStoreItem>();

        }

        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public bool Consumed { get; set; }
        public int DefaultUnitId { get; set; }
        public int? WarningLevel { get; set; }
        public int ItemCategoryId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Unit DefaultUnit { get; set; }
        public virtual ItemCategory ItemCategory { get; set; }
        public virtual ICollection<CommitteeItem> CommitteeItem { get; set; }
        public virtual ICollection<StoreItem> StoreItem { get; set; }
        public virtual ICollection<RobbedStoreItem> RobbedStoreItem { get; set; }

        public virtual ICollection<ExecutionOrderResultItem> ExecutionOrderResultItem { get; set; }



    }
}
