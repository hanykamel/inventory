using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class TechnicalDepartment:Entity<int>,IActive
    {
        public TechnicalDepartment()
        {
            Store = new HashSet<Store>();
        }

        public string Name { get; set; }
        public string TechnicianId { get; set; }
        public string AssistantTechnician { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<Store> Store { get; set; }
    }
}
