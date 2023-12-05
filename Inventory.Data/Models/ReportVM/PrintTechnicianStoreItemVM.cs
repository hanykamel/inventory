using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.ReportVM
{
    public class PrintTechnicianStoreItemVM
    {
        public long Id { get; set; }
        public int TotalQuantity { get; set; }
        public string ContractNumber { get; set; }
        public int PageNumber { get; set; }
        public int storeQuantity { get; set; }
        public int expensedQuantity { get; set; }
        public int reservedQuantity { get; set; }
        public int robbingQuantity { get; set; }
        public DateTime CreationDate { get; set; }
        public int TenantId { get; set; }
        public string BaseItemName { get; set; }
        public long? BaseItemID { get; set; }
        public int? CategoryID { get; set; }
        public string itemCategory { get; set; }
        public int BudgetId { get; set; }
        public string receiverName { get; set; }
        public string department { get; set; }
        public int? departmentId { get; set; }
        public DateTime invoiceDate { get; set; }
        public string invoiceCode { get; set; }

    }

    public class GroupedTechnicianStoreItemsVM
    {
        public long? BaseItemID { get; set; }
        public string BaseItemName { get; set; }
        public int? TotalQuantity { get; set; }
        public int? storeQuantity { get; set; }
        public int? expensedQuantity { get; set; }
        public int? reservedQuantity { get; set; }
        public int? robbingQuantity { get; set; }

        public List<PrintTechnicianStoreItemVM> Details { get; set; }
    }
}
