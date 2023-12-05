using Inventory.Data.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.StoreItemVM
{
    public class StockTakingBaseItemVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int BookNumber { get; set; }
        public int BookPageNumber { get; set; }
        public string UnitName { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public long BookId { get; set; }
        public int? UnitId { get; set; }
        public bool Isrobbing { get; set; }
        public string ContractNum { get; set; }


    }
}
