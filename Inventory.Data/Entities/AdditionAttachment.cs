
using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class AdditionAttachment: Entity<Guid>,IActive,ITenant
    {
        
        public Guid AdditionId { get; set; }
        public Guid AttachmentId { get; set; }

        public virtual Addition Addition { get; set; }
        public virtual Attachment Attachment { get; set; }
        public int TenantId { get; set; }
        public bool? IsActive { get; set; }
    }
}
