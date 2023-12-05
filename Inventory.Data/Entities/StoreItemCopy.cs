using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory.Data.Entities
{
    public class StoreItemCopy : Entity<Guid>,IActive,IHistory<Guid>
    {
        public StoreItemCopy()
        {
            Id = Guid.NewGuid();
        }
        public Guid AdditionId { get; set; }
        public long BaseItemId { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public long BookId { get; set; }
        public int BookPageNumber { get; set; }
        public string Note { get; set; }
        public int StoreId { get; set; }
        public string RobbingName { get; set; }
        public decimal? RobbingPrice { get; set; }
        public int? UnitId { get; set; }
        public int StoreItemStatusId { get; set; }
        public int CurrentItemStatusId { get; set; }
        public bool? IsStagnant { get; set; }
        public bool? UnderDelete { get; set; }
        public int Serial { get; set; }
        public int TenantId { get; set; }
        public int? CurrencyId { get; set; }
        public bool? IsActive { get; set; }
        public string AuditAction { get; set; }
        public Guid HistoryId { get; set; }
        public string AuditUser { get; set; }
        public DateTime? AuditDate { get; set; }

        public virtual StoreItemStatus StoreItemStatus { get; set; }
        public virtual StoreItem History { get; set; }
    }
}
