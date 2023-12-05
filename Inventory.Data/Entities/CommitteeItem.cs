using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class CommitteeItem:Entity<Guid>,IActive,ITenant,IDelete
    {
        public CommitteeItem()
        {
            CommitteeItemHistory = new HashSet<CommitteeItemHistory>();
        }

        public long BaseItemId { get; set; }
        public Guid ExaminationCommitteId { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public int ExaminationPercentage { get; set; }
        public int Accepted { get; set; }
        public int? Rejected { get; set; }
        public string Reasons { get; set; }
        public string AdditionNotes { get; set; }
        public int TenantId { get; set; }
        public bool? IsActive { get; set; }
        public virtual BaseItem BaseItem { get; set; }
        public virtual ExaminationCommitte ExaminationCommitte { get; set; }
        public virtual ICollection<CommitteeItemHistory> CommitteeItemHistory { get; set; }
        public virtual Unit Unit { get; set; }
        public bool? IsDelete { get; set; }
    }
}
