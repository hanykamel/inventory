using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public class ExecutionOrderAttachment : Entity<Guid>, IActive
    {
        public Guid ExecutionOrderId { get; set; }
        public Guid AttachmentId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Attachment Attachment { get; set; }
        public virtual ExecutionOrder ExecutionOrder { get; set; }
    }
}
