using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class ExaminationCommitte:Entity<Guid>,IActive,ITenant,ICode
    {
        public ExaminationCommitte()
        {
            Addition = new HashSet<Addition>();
            CommitteeAttachment = new HashSet<CommitteeAttachment>();
            CommitteeEmployee = new HashSet<CommitteeEmployee>();
            CommitteeItem = new HashSet<CommitteeItem>();
        }
        public string Code { get; set; }

        public DateTime Date { get; set; }
        public int BudgetId { get; set; }
        public int StoreId { get; set; }
        public int? SupplyType { get; set; }
        public string DecisionNumber { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string SupplyOrderNumber { get; set; }
        public DateTime? SupplyOrderDate { get; set; }
        public int? SupplierId { get; set; }
        public string ContractNumber { get; set; }
        public DateTime? ContractDate { get; set; }
        public bool ForConsumedItems { get; set; }
        public int OperationId { get; set; }
        public int? ExternalEntityId { get; set; }
        public int TenantId { get; set; }
        public int ExaminationStatusId { get; set; }
        public int? CurrencyId { get; set; }
        public bool? IsActive { get; set; }
        public Currency Currency { get; set; }
        public virtual Budget Budget { get; set; }
        public virtual ExternalEntity ExternalEntity { get; set; }
        public virtual Operation Operation { get; set; }
        public virtual Store Store { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ExaminationStatus ExaminationStatus { get; set; }
        public virtual ICollection<Addition> Addition { get; set; }
        public virtual ICollection<CommitteeAttachment> CommitteeAttachment { get; set; }
        public virtual ICollection<CommitteeEmployee> CommitteeEmployee { get; set; }
        public virtual ICollection<CommitteeItem> CommitteeItem { get; set; }
        public int Year { get; set; }
        public int Serial { get; set; }
    }
}
