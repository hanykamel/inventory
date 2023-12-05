using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.StoreItemVM
{
    public class DedcuctionStoreItemVM
    {
        public Guid Id { get; set; }
        public long BaseItemId { get; set; }
        public string ContractNumber { get; set; }
        public int PageNumber { get; set; }

        public string BaseItemName { get; set; }
        public string UnitName { get; set; }
        public int Quantity { get; set; }
        public string Currency { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string ItemStatus { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}
