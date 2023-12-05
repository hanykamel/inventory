using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public class ExecutionOrderResultItem : Entity<Guid>, IActive
    {
        public long BaseItemId { get; set; }
        public Guid ExecutionOrderId { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public string Note { get; set; }
        public bool? IsAdded { get; set; }
        public bool? IsActive { get; set; }
        public int? CurrencyId { get; set; }
        public virtual BaseItem BaseItem { get; set; }
        public virtual ExecutionOrder ExecutionOrder { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual Currency Currency { get; set; }


        //public virtual RemainsDetails RemainsDetails { get; set; }


    }
}
