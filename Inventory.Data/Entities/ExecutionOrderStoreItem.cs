using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public class ExecutionOrderStoreItem : Entity<Guid>, IActive
    {
        public Guid StoreItemId { get; set; }
        public Guid ExecutionOrderId { get; set; }
        public bool? IsApproved { get; set; }
        //public bool? HasRemains { get; set; }
        //public int? RemainDetailsId { get; set; }
        public string Note { get; set; }
        public bool? IsActive { get; set; }
        public virtual StoreItem StoreItem { get; set; }
        public virtual ExecutionOrder ExecutionOrder { get; set; }
        //public virtual RemainsDetails RemainsDetails { get; set; }


    }
}
