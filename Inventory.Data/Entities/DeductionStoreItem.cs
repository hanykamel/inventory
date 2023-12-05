using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class DeductionStoreItem : Entity<Guid>,IActive
    {
        public Guid DeductionId { get; set; }
        public Guid StoreItemId { get; set; }
        public string Note { get; set; }
        public virtual Deduction Deduction { get; set; }
        public virtual StoreItem StoreItem { get; set; }
        public bool? IsActive { get; set; }
    }
}
