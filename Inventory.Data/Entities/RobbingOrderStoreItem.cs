using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class RobbingOrderStoreItem : Entity<Guid>,IActive
    {
        public Guid RobbingOrderId { get; set; }
        public Guid StoreItemId { get; set; }
        public string Notes { get; set; }
        public decimal Price { get; set; }
        public string ExaminationReport { get; set; }
        public int? StoreItemStatusId { get; set; }
        public bool? IsActive { get; set; }
        public virtual RobbingOrder RobbingOrder { get; set; }
        public virtual StoreItem StoreItem { get; set; }
        public virtual StoreItemStatus StoreItemStatus { get; set; }

    }
}
