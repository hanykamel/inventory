using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Data.Models.ReportVM
{
   public class StoreBookVM
    {
        public int BookNumber { get; set; }
        public int PageNumber { get; set; }
        public string ContractNumber { get; set; }
        public int TenantId { get; set; }
        public int BudgetId { get; set; }
        public long Id { get; set; }
        public string ItemName { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
