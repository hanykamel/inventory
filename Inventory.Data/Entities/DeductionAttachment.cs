using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public class DeductionAttachment : Entity<Guid>, IActive
    {
        public Guid DeductionId { get; set; }
        public Guid AttachmentId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Attachment Attachment { get; set; }
        public virtual Deduction Deduction { get; set; }
    }
}
