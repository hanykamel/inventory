using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class CommitteeEmployee:Entity<Guid>,IActive,ITenant,IDelete
    {
        public int EmplyeeId { get; set; }
        public int JobTitleId { get; set; }
        public Guid ExaminationCommitteId { get; set; }
        public bool IsHead { get; set; }
        public int TenantId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Employees Emplyee { get; set; }
        public virtual ExaminationCommitte ExaminationCommitte { get; set; }
        public virtual JobTitle JobTitle { get; set; }
        public bool? IsDelete { get; set; }
    }
}
