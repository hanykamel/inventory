using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class Delegation:Entity<Guid>,IActive,ITenant
    {
     public Delegation()
        {
            DelegationStore = new HashSet<DelegationStore>();
        }
        public string UserName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public int ShiftId { get; set; }
        public int DelegationTypeId { get; set; }
        public bool? IsSuspended { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<DelegationStore> DelegationStore { get; set; }
        public virtual Shift Shift { get; set; }
        public virtual DelegationType DelegationType { get; set; }
        public int TenantId { get; set; }
    }
}
