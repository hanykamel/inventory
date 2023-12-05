using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    
    public class StagnantAttachment : Entity<Guid>, IActive,ITenant
    {
        public Guid AttachmentId { get; set; }
        public Guid StagnantId { get; set; }
        public int TenantId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Attachment Attachment { get; set; }
        public virtual Stagnant Stagnant { get; set; }
    }
}
