
using System;

namespace Inventory.Data.Models.StoreItemVM
{
    public class ExchangeOrderStoreItemVM
    {
        public Guid StoreItemId { get; set; }
        public string StoreItemCode { get; set; }
        public string BaseItemName { get; set; }
        public string Notes { get; set; }
        public int ItemStatus { get; set; }
        
    }
}
