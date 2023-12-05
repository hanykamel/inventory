using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class AdditionDocumentType:Entity<int>,IActive
    {
        public AdditionDocumentType()
        {
            Addition = new HashSet<Addition>();
        }

        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<Addition> Addition { get; set; }
    }
}
