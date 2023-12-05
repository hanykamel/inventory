using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.PrintTemplateVM
{

    public class InvoiceReportVM 
    {
        public String Code { get; set; }
        public DateTime Date { get; set; }
        public int TenantId { get; set; }
    }
}
