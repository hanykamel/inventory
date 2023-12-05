using Inventory.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.StoreItemVM
{
    public class BaseItemStoreItemVM
    {
        public Deduction deduction { get; set; }
        public long BaseItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal FullPrice { get; set; }
        public string Currency { get; set; }
        public int Quantity { get; set; }
        public string BaseItemDesc { get; set; }
        public string BaseItemName { get; set; }
        public string UnitName { get; set; }
        public string ContractNumber { get; set; }
        public int PageNumber { get; set; }

        public string Notes { get; set; }
        public string ItemStatus { get; set; }
        public string ExaminationReport { get; set; }
        public DateTime Date { get; set; }
    }
}
