using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class Attachment:Entity<Guid>,IActive,ITenant,IDelete
    {
        public Attachment()
        {
            Id = Guid.NewGuid();
            AdditionAttachment = new HashSet<AdditionAttachment>();
            CommitteeAttachment = new HashSet<CommitteeAttachment>();
            TransformationAttachment = new HashSet<TransformationAttachment>();
            RefundOrderAttachment = new HashSet<RefundOrderAttachment>();
            StockTakingAttachment = new HashSet<StockTakingAttachment>();
            StagnantAttachment=new HashSet<StagnantAttachment>();
            DeductionAttachment = new HashSet<DeductionAttachment>();
            ExecutionOrderAttachment = new HashSet<ExecutionOrderAttachment>();

        }

        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public long FileSize { get; set; }
        public string FileExtention { get; set; }
        public int AttachmentTypeId { get; set; }
        public bool? IsActive { get; set; }
        public virtual AttachmentType AttachmentType { get; set; }
        public virtual ICollection<AdditionAttachment> AdditionAttachment { get; set; }
        public virtual ICollection<CommitteeAttachment> CommitteeAttachment { get; set; }
        public virtual ICollection<TransformationAttachment> TransformationAttachment { get; set; }
        public virtual ICollection<RefundOrderAttachment> RefundOrderAttachment { get; set; }
        public virtual ICollection<RobbingOrderAttachment> RobbingOrderAttachment { get; set; }
        public virtual ICollection<StockTakingAttachment> StockTakingAttachment { get; set; }
        public virtual ICollection<StagnantAttachment> StagnantAttachment { get; set; }
        public virtual ICollection<DeductionAttachment> DeductionAttachment { get; set; }
        public virtual ICollection<ExecutionOrderAttachment> ExecutionOrderAttachment { get; set; }

        public int TenantId { get; set; }
        public bool? IsDelete { get; set; }
    }
}
