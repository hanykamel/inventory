using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class DelegationStore:Entity<Guid>,IActive
    {
        public int StoreId { get; set; }
        public Guid DelegationId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Store Store  { get; set; }
        public virtual Delegation Delegation { get; set; }
    }
}
