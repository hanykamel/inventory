using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class AttachmentType:Entity<int>,IActive
    {
        public AttachmentType()
        {
            Attachment = new HashSet<Attachment>();
        }

        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<Attachment> Attachment { get; set; }
        public virtual ICollection<OperationAttachmentType> OperationAttachmentType { get; set; }
        
    }
}
