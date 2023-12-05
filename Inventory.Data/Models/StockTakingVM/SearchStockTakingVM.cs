using Inventory.Data.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.StoreItemVM
{
    public class SearchStockTakingVM
    {
        public List<StockTakingBaseItemVM> BaseItems { get; set; }
        public decimal TotalPrice{ get; set; }
        public long TotalUnitsCount{ get; set; }
        public long TotalConsumedCount { get; set; }
        public long TotalNotConsumedCount { get; set; }

    }
}
