using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class JobTitle:Entity<int>,IActive
    {
        public JobTitle()
        {
            CommitteeEmployee = new HashSet<CommitteeEmployee>();
        }

       
        public string Name { get; set; }

        public virtual ICollection<CommitteeEmployee> CommitteeEmployee { get; set; }
        public bool? IsActive { get ; set; }
    }
}
