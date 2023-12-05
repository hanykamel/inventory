using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class RobbingOrder : Entity<Guid>,IActive,ITenant,ICode
    {
        public RobbingOrder()
        {
            RobbingOrderStoreItem = new HashSet<RobbingOrderStoreItem>();
            RobbingOrderAttachment = new HashSet<RobbingOrderAttachment>();
            RobbingOrderRemainsDetails = new HashSet<RobbingOrderRemainsDetails>();

        }
        public string Code { get; set; }
        //public DateTime Date { get; set; }
        public int BudgetId { get; set; }
        public int OperationId { get; set; }
        public int? RobbingOrderStatusId { get; set; }
        public string Notes { get; set; }
        public int FromStoreId { get; set; }
        public int ToStoreId { get; set; }
        //public int AdditionNumber { get; set; }
        //public string RequesterName { get; set; }
        public bool? IsActive { get; set; }
        //public DateTime RequestDate { get; set; }

        public virtual Budget Budget { get; set; }
        public virtual RobbingOrderStatus RobbingOrderStatus { get; set; }
        
        public virtual Store FromStore { get; set; }
        public virtual Store ToStore { get; set; }

        public virtual Operation Operation { get; set; }
        public virtual ICollection<RobbingOrderStoreItem> RobbingOrderStoreItem { get; set; }
        public virtual ICollection<RobbingOrderAttachment> RobbingOrderAttachment { get; set; }
        public virtual ICollection<RobbingOrderRemainsDetails> RobbingOrderRemainsDetails { get; set; }
        public virtual ICollection<Subtraction> Subtraction { get; set; }

        public virtual ICollection<Invoice > Invoice { get; set; }
        public int TenantId { get ; set; }
        public int Year { get; set; }
        public int Serial { get ; set ; }
    }
}
