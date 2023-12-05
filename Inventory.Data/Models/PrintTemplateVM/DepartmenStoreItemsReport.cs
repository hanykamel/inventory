using Inventory.Data.Models.ReportVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.PrintTemplateVM
{
    public class GroupedDeartmentStoreItemReportVM
    {
        public int? DepartmentId { get; set; }
        public String DepartmentName { get; set; }
        //public int? Amount { get; set; }
        public List<DepartmentDetailsVM> Items { get; set; }
    }
   
}
