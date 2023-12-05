using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.StoreItemVM
{
    public class InvoiceStoreItemVM
    {
        public Guid InvoiceId { get; set; }
        public string StoreItemCode { get; set; }
        public string BaseItemDesc { get; set; }
        public string BaseItemName { get; set; }
        public string UnitName { get; set; }
        public int Count { get; set; }
        public int PageNumber { get; set; }
        public string ExchangeOrderNotes { get; set; }
        public string ContractNumber { get; set; }
    }
}
