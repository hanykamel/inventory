using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class Remains : Entity<long>, IActive
    {
        public Remains()
        {
            RemainsDetails = new HashSet<RemainsDetails>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool? Consumed { get; set; }
        public virtual ICollection<RemainsDetails> RemainsDetails { get; set; }
        public virtual ICollection<ExecutionOrderResultRemain> ExecutionOrderResultRemain { get; set; }
        public bool? IsActive { get; set; }
    }
}
