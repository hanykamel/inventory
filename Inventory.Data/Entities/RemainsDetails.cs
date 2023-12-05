using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public class RemainsDetails : Entity<Guid>, IActive, ITenant
    {
        public RemainsDetails()
        {
        }

        public long RemainsId { get; set; }
        public int? UnitId { get; set; }
        public decimal Quantity { get; set; }
        public int? CurrencyId { get; set; }
        public decimal Price { get; set; } 
        public Guid? AdditionId { get; set; }
        //public Guid? ExecutionOrderId { get; set; }
        public long BookId { get; set; }
        public int BookPageNumber { get; set; }
        public string RobbingName { get; set; }
        public string Notes { get; set; }
        public int TenantId { get; set; }
        
        public bool? IsActive { get; set; }

        public Remains Remains { get; set; }
        public Unit Unit { get; set; }
        public Addition Addition { get; set; }
        public Book Book { get; set; }
        public Currency Currency { get; set; }


    }
}
