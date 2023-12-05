using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class TransformationAttachment:Entity<Guid>,IActive
    {
        public Guid TransformationId { get; set; }
        public Guid AttachmentId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Attachment Attachment { get; set; }
        public virtual Transformation Transformation { get; set; }
    }
}
