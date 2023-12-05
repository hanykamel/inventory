using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public class RobbingOrderStatus : Entity<int>,IActive
    {
        public RobbingOrderStatus()
        {
            RobbingOrder = new HashSet<RobbingOrder>();
        }

        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<RobbingOrder> RobbingOrder { get; set; }
    }
}
