using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class DelegationType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Delegation> Delegation { get; set; }

    }
}
