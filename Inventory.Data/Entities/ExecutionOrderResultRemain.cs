using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public class ExecutionOrderResultRemain : Entity<Guid>, IActive
    {
        public long RemainsId { get; set; }
        public Guid ExecutionOrderId { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public string Note { get; set; }
        public int? CurrencyId { get; set; }

        //public bool? IsRobbed { get; set; }
        public bool? IsActive { get; set; }
        public virtual Remains Remains { get; set; }
        public virtual ExecutionOrder ExecutionOrder { get; set; }
        public virtual Unit Unit { get; set; }
        public Currency Currency { get; set; }

        //public virtual RemainsDetails RemainsDetails { get; set; }


    }
}
