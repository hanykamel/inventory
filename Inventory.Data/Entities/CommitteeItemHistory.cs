using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class CommitteeItemHistory:Entity<Guid>,IActive,ITenant
    {
        public long BaseItemId { get; set; }
        public Guid ExaminationCommitteId { get; set; }
        public int Quantity { get; set; }
        public int ExaminationPercentage { get; set; }
        public int Accepted { get; set; }
        public int? Rejected { get; set; }
        public string Reasons { get; set; }
        public string Notes { get; set; }
        public Guid CommitteeItemId { get; set; }
        public int HistoryActionId { get; set; }
        public int TenantId { get; set; }
        public bool? IsActive { get; set; }
        public virtual CommitteeItem CommitteeItem { get; set; }
        public virtual HistoryAction HistoryAction { get; set; }
    }
}
