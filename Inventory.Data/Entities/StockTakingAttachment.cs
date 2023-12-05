using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
   
    public class StockTakingAttachment : Entity<Guid>, IActive, ITenant
    {
        public Guid AttachmentId { get; set; }
        public Guid StockTakingId { get; set; }
        public int TenantId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Attachment Attachment { get; set; }
        public virtual StockTaking StockTaking { get; set; }
    }
}
