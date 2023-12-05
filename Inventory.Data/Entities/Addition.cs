
using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class Addition:Entity<Guid>, IActive,ITenant,ICode,IDelete
    {
      
        public Addition()
        {
            AdditionAttachment = new HashSet<AdditionAttachment>();
            StoreItem = new HashSet<StoreItem>();
            RobbedStoreItem = new HashSet<RobbedStoreItem>();

            //Date = DateTime.Now;
        }

        public Guid? ExaminationCommitteId { get; set; }
        public Guid? TransformationId { get; set; }
        public Guid? RobbingOrderId { get; set; }
        public Guid? ExecutionOrderId { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public int BudgetId { get; set; }
        public int OperationId { get; set; }
        public int? AdditionDocumentTypeId { get; set; }
        public string AdditionDocumentNumber { get; set; }
        public string Note { get; set; }
        public string RequesterName { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? AdditionDocumentDate { get; set; }

        public int? AdditionNumber { get; set; }
        public virtual AdditionDocumentType AdditionDocumentType { get; set; }
        public virtual Budget Budget { get; set; }
        public virtual ExaminationCommitte ExaminationCommitte { get; set; }
        public virtual Operation Operation { get; set; }
        public virtual Transformation Transformation { get; set; }
        public virtual ExecutionOrder ExecutionOrder { get; set; }

        public virtual RobbingOrder RobbingOrder { get; set; }
        public virtual ICollection<AdditionAttachment> AdditionAttachment { get; set; }
        public virtual ICollection<StoreItem> StoreItem { get; set; }
        public virtual ICollection<RobbedStoreItem> RobbedStoreItem { get; set; }

        public virtual ICollection<RemainsDetails> RemainsDetails { get; set; }
        public int TenantId { get; set; }
        public int Year { get; set; }
        public int Serial { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
    }
}
