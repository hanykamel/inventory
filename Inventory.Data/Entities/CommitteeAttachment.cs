using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class CommitteeAttachment:Entity<Guid>,IActive,ITenant,IDelete
    {
        public Guid AttachmentId { get; set; }
        public Guid CommitteeId { get; set; }
        public int TenantId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Attachment Attachment { get; set; }
        public virtual ExaminationCommitte Committee { get; set; }
        public bool? IsDelete { get ; set; }
    }
}
