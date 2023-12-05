using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class InvoiceStoreItem:Entity<Guid>,IActive,ITenant
    {
        public Guid InvoiceId { get; set; }
        public Guid StoreItemId { get; set; }
        public int TenantId { get; set; }
        public DateTime? RefundDate { get; set; }
        public bool? IsRefunded { get; set; }
        public bool? UnderRefunded { get; set; }
        public string RefundUserName { get; set; }
        public bool? IsActive { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual StoreItem StoreItem { get; set; }
    }
}
