using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class Shift:Entity<int>,IActive
    {
        public Shift()
        {
            Delegation = new HashSet<Delegation>();

        }
        public string Name { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public virtual ICollection<Delegation> Delegation { get; set; }
        public bool? IsActive { get ; set ; }
    }
}
