using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class TransformationStoreItem:Entity<Guid>,IActive
    {
        public Guid StoreItemId { get; set; }
        public Guid TransformationId { get; set; }
        public string Note { get; set; }
        public string NoteCreatorId { get; set; }
        public bool? IsActive { get; set; }
        public virtual StoreItem StoreItem { get; set; }
        public virtual Transformation Transformation { get; set; }
    }
}
