using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class StockTaking : Entity<Guid>, IActive, ITenant,ICode
    {
        public StockTaking()
        {
            StockTakingStoreItem = new HashSet<StockTakingStoreItem>();
            StockTakingAttachment = new HashSet<StockTakingAttachment>();
            StockTakingRobbedStoreItem = new HashSet<StockTakingRobbedStoreItem>();

        }
        public int OperationId { get; set; }
        public int TenantId { get ; set ; }
        public DateTime Date { get; set; }
        public int Year { get ; set ; }
        public int Serial { get ; set ; }
        public string Code { get ; set ; }
        public bool? IsActive { get; set; }
        public virtual Operation Operation { get; set; }
        public virtual ICollection<StockTakingStoreItem> StockTakingStoreItem { get; set; }
        public virtual ICollection<StockTakingRobbedStoreItem> StockTakingRobbedStoreItem { get; set; }
        public virtual ICollection<StockTakingAttachment> StockTakingAttachment { get; set; }

    }
}
