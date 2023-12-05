using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class Transformation:Entity<Guid>,IActive,ICode,ITenant
    {
        public Transformation()
        {
            Addition = new HashSet<Addition>();
            TransformationAttachment = new HashSet<TransformationAttachment>();
            TransformationStoreItem= new HashSet<TransformationStoreItem>();
        }

        public int FromStoreId { get; set; }
        public int ToStoreId { get; set; }
        public int? ToExternalEntityId { get; set; }
        public int OperationId { get; set; }
        public string Code { get; set; }
        public int? BudgetId { get; set; }
        public int? TransformationStatusId { get; set; }
        //public DateTime Date { get; set; }
        public string Notes { get; set; }
        public bool? IsActive { get; set; }
        //public string RequesterName { get; set; }
        //public DateTime RequestDate { get; set; }
        //public int AdditionNumber { get; set; }
        public virtual Budget Budget { get; set; }
        public virtual Store FromStore { get; set; }
        public virtual Store ToStore { get; set; }
        public virtual Operation Operation { get; set; }
        public virtual ExternalEntity ToExternalEntity { get; set; }
        public virtual TransformationStatus TransformationStatus { get; set; }
        public virtual ICollection<Subtraction> Subtraction { get; set; }
        public virtual ICollection<Addition> Addition { get; set; }
        public virtual ICollection<TransformationAttachment> TransformationAttachment { get; set; }
        public virtual ICollection<TransformationStoreItem> TransformationStoreItem { get; set; }
        public int Year { get; set; }
        public int Serial { get; set; }
        public int TenantId { get ; set; }
    }
}
