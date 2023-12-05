using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class Deduction : Entity<Guid>,IActive,ITenant,ICode
    {
        public Deduction()
        {
            DeductionStoreItem = new HashSet<DeductionStoreItem>();
            DeductionAttachment = new HashSet<DeductionAttachment>();
        }
        public string Code { get; set; }
        //public DateTime Date { get; set; }
        public int BudgetId { get; set; }
        public int OperationId { get; set; }
        public string Notes { get; set; }

        //public string RequesterName { get; set; }
        public bool? IsActive { get; set; }
        //public DateTime RequestDate { get; set; }

        public virtual Budget Budget { get; set; }
        
        public virtual Operation Operation { get; set; }
        public virtual ICollection<DeductionStoreItem> DeductionStoreItem { get; set; }
        public virtual ICollection<DeductionAttachment> DeductionAttachment { get; set; }
        public virtual ICollection<Subtraction> Subtraction { get; set; }

        public int TenantId { get ; set; }
        public int Year { get; set; }
        public int Serial { get ; set ; }
    }
}
