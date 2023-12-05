using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.RefundOrderVM
{
    public class RefundStoreItemVM
    {
        public Guid InvoiceId { get; set; }
        public string StoreItemCode { get; set; }
        public string BaseItemName { get; set; }
        public Guid StoreItemId { get; set; }
        public string Notes { get; set; }
        public Guid InvoicestoreItemId { get; set; }
        public StoreItemStatus storeItemStatus { get; set; }
    }
}
