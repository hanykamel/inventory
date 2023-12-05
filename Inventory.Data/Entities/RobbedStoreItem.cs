using Inventory.Data.Interfaces;
using Inventory.Data.Migrations;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class RobbedStoreItem:Entity<Guid>,IActive,ITenant
    {
        public RobbedStoreItem()
        {
          
        }

        public Guid AdditionId { get; set; }
        public long StoreBaseItemId { get; set; }

        public long BaseItemId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public long BookId { get; set; }
        public int BookPageNumber { get; set; }
        public string Note { get; set; }
        public int UnitId { get; set; }
        public int TenantId { get; set; }
        public int CurrencyId { get; set; }
        public bool? IsActive { get; set; }
        public int StoreItemStatusId { get; set; }
        public Currency Currency { get; set; }
        public virtual Addition Addition { get; set; }
        public virtual BaseItem BaseItem { get; set; }
        public virtual BaseItem StoreBaseItem { get; set; }
        public StoreItemStatus StoreItemStatus { get; set; }
        public virtual Book Book { get; set; }
        public virtual Unit Unit { get; set; }
     
       
    }
}
