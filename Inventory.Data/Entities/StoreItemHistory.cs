using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class StoreItemHistory:Entity<Guid>,IActive,ITenant
    {
        public Guid StoreItemId { get; set; }
        public int OperationId { get; set; }
        public string OperationCode { get; set; }
        public decimal Price { get; set; }
        public int BookNumber { get; set; }
        public int BookPageNumber { get; set; }
        public string Note { get; set; }
        public string NoteCreatorName { get; set; }
        public int TenantId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Operation Operation { get; set; }
        public virtual StoreItem StoreItem { get; set; }
    }
}
