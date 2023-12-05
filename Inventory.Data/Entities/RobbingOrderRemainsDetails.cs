using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class RobbingOrderRemainsDetails : Entity<Guid>,IActive
    {
        public Guid RobbingOrderId { get; set; }
        public Guid ExecutionOrderResultRemainId { get; set; }
        public string Notes { get; set; }
        public decimal Price { get; set; }
        public string ExaminationReport { get; set; }
        public bool? IsActive { get; set; }
        public virtual RobbingOrder RobbingOrder { get; set; }
        public virtual ExecutionOrderResultRemain ExecutionOrderResultRemain { get; set; }

    }
}
