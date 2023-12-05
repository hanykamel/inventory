using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.ReportVM
{
    public class ExistingStoreItemVM
    {
        public long Id { get; set; }
        public long BaseItemId { get; set; }
        public int availableQuantity { get; set; }
        public int TenantId { get; set; }
        public int AllQuantity { get; set; }
        public DateTime CreationDate { get; set; }
        public string ContractNumber { get; set; }
        public string BaseItemName { get; set; }
        public string UnitName { get; set; }
        public int differenceQuantity { get; set; }
        public int BudgetId { get; set; }
        public int BookNumber { set; get; }

        public int PageNumber { set; get; }
    }
}
