using Inventory.Data.Models.ReportVM;
using System.Collections.Generic;

namespace Inventory.Data.Models.PrintTemplateVM
{
    public class DailyReportPrintObjectVM
    {
        public string Currency { get; set; }
        public List<DailyStoreItemsVM> value { get; set; }
    }
}
