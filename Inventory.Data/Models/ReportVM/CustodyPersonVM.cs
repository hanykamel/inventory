using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.ReportVM
{
    public class CustodyPersonVM
    {
        public long Id { get; set; }
        public int TenantId { get; set; }

        public long baseId { get; set; }
        public string BaseName { get; set; }
        public string ContractNumber { get; set; }
        public int BookNumber { get; set; }
        public int PageNumber { get; set; }
        public string BudgetName { get; set; }
        public int BudgetId { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string ReceivedEmployee { get; set; }
        public string ReceivedEmployeeCard { get; set; }
        public int ReceivedEmployeeId { get; set; }
        public string UnitName { get; set; }
        public string CreationDate { get; set; }
        public int StoreItemCount { get; set; }
    }
}
