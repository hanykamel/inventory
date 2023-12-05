using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class RefundOrderAttachment : Entity<int>,IActive
    {
        public Guid RefundOrderId { get; set; }
        public Guid AttachmentId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Attachment Attachment { get; set; }
        public virtual RefundOrder RefundOrder { get; set; }
    }
}
