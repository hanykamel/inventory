using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.ReportVM
{
 public   class DailyStoreItemVM
    {
        public int Id { get; set; }
        public Guid AdditionId { get; set; }
        public DateTime CreationDate { get; set; }

        public DateTime Date { get; set; }
        public string PermissionCode { get; set; }
        public string FromOrTo { get; set; }

        public int AddedItemsValue { get; set; }

        public int ExpendedItemsValue { get; set; }

        public string Notes { get; set; }

        public int TotalPermissions { get; set; }

        public int shiftedCredit { get; set; }

        public int total { get; set; }
    }
}
