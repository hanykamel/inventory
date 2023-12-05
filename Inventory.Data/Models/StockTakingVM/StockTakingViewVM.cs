using Inventory.Data.Models.StoreItemVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.StockTakingVM
{
    public class StockTakingViewVM
    {
        public Guid Id { get; set; }

        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }
        // public List<StockTakingBaseItemVM> BaseItems { get; set; }
        public long baseItemId { get; set; }
        public string Name { get; set; }
        public int BookNumber { get; set; }
        public int BookPageNumber { get; set; }
        public string UnitName { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
