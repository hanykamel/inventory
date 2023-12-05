using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class RefundOrderStoreItem : Entity<Guid>,IActive
    {
        public Guid RefundOrderId { get; set; }
        public Guid StoreItemId { get; set; }
        public string Notes { get; set; }
        public int StoreItemStatusId { get; set; }
        public virtual RefundOrder RefundOrder { get; set; }
        public virtual StoreItem StoreItem { get; set; }
        public virtual StoreItemStatus StoreItemStatus { get; set; }
        public bool? IsActive { get; set; }
    }
}
