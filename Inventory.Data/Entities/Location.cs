using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class Location:Entity<int>,IActive
    {
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }

    }
}
