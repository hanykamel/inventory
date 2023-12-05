using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class OperationAttachmentType:Entity<int>,IActive
    {
        public int OperationId { get; set; }
        public int? BudgetId { get; set; }
        public int AttachmentTypeId { get; set; }
        public bool? IsActive { get; set; }
        public int? AdditionDocumentTypeId { get; set; }
        public virtual AttachmentType AttachmentType { get; set; }
        public virtual Operation Operation { get; set; }
        public virtual Budget Budget { get; set; }
        public virtual AdditionDocumentType AdditionDocumentType { get; set; }
    }
}
