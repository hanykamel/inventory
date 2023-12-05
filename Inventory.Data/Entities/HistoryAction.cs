using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class HistoryAction:Entity<int>,IActive
    {
        public HistoryAction()
        {
            CommitteeItemHistory = new HashSet<CommitteeItemHistory>();
        }

        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<CommitteeItemHistory> CommitteeItemHistory { get; set; }
    }
}
