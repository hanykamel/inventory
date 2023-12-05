using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class Department:Entity<int>,IActive
    {
        public Department()
        {
            Employees = new HashSet<Employees>();
            Invoice = new HashSet<Invoice>();
        }

        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<Employees> Employees { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }

    }
}
