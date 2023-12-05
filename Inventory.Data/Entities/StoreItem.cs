using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class StoreItem:Entity<Guid>,IActive,ITenant,IDelete
    {
        public StoreItem()
        {
            ExchangeOrderStoreItem = new HashSet<ExchangeOrderStoreItem>();
            InvoiceStoreItem = new HashSet<InvoiceStoreItem>();
            RefundOrderStoreItem = new HashSet<RefundOrderStoreItem>();
            StoreItemHistory = new HashSet<StoreItemHistory>();
            RefundOrderStoreItem = new HashSet<RefundOrderStoreItem>();
            StagnantStoreItem=new HashSet<StagnantStoreItem>();
            StoreItemCopy = new HashSet<StoreItemCopy>();

        }

        public Guid AdditionId { get; set; }
       // public int InputId{get; set;}
        public long BaseItemId { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public long BookId { get; set; }
        public int BookPageNumber { get; set; }
        public string Note { get; set; }
        public int StoreId { get; set; }
        //public string RobbingName { get; set; }
        //public decimal? RobbingPrice { get; set; }
       // public int? RobbingStoreItemStatusId { get; set; }
        public int? UnitId { get; set; }
        public int StoreItemStatusId { get; set; }
        public int CurrentItemStatusId { get; set; }
        public bool? IsStagnant { get; set; }
        public bool? UnderDelete { get; set; }
        public bool? UnderExecution { get; set; }

        public int Serial { get; set; }
        public int TenantId { get; set; }
        public int? CurrencyId { get; set; }
        public bool? IsActive { get; set; }
        public Currency Currency { get; set; }
        public virtual Addition Addition { get; set; }
        public virtual BaseItem BaseItem { get; set; }
        public virtual Book Book { get; set; }
        public virtual ItemStatus CurrentItemStatus { get; set; }
        public virtual Store Store { get; set; }
        public virtual Unit Unit { get; set; }
       /// public RobbingStoreItemStatus RobbingStoreItemStatus { get; set; }

        public virtual StoreItemStatus StoreItemStatus { get; set; }
        public virtual ICollection<ExchangeOrderStoreItem> ExchangeOrderStoreItem { get; set; }
        public virtual ICollection<StoreItemCopy> StoreItemCopy { get; set; }

        public virtual ICollection<RefundOrderStoreItem> RefundOrderStoreItem { get; set; }
        public virtual ICollection<TransformationStoreItem> TransformationStoreItem { get; set; }

        public virtual ICollection<InvoiceStoreItem> InvoiceStoreItem { get; set; }
        public virtual ICollection<StagnantStoreItem> StagnantStoreItem { get; set; }
        public virtual ICollection<StoreItemHistory> StoreItemHistory { get; set; }
        public bool? IsDelete { get; set; }
    }
}
