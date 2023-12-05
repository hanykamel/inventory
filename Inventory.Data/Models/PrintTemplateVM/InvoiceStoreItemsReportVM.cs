using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.PrintTemplateVM
{

    public class InvoiceStoreItemsReportVM 
    {
        public String Code { get; set; }
        public String StoreItem { get; set; }
        public int Quantity { get; set; }
        public String Unit { get; set; }
        public Decimal Price { get; set; }
        public DateTime CreationDate { get; set; }

        public int TenantId { get; set; }

        public InvoiceContainerVM Invoice { get; set; }
    }

    public class InvoiceContainerVM
    {
        public String Code { get; set; }
        public String ReceivedEmployeeName { get; set; }
        public String EmployeeCardCode { get; set; }
        public String LocationName { get; set; }
    }
    public class GroupedInvoiceStoreItemsVM
    {
        public String Code { get; set; }
        public String ReceivedEmployeeName { get; set; }
        public String EmployeeCardCode { get; set; }
        public String LocationName { get; set; }
        public List<InvoiceStoreItemsReportVM> StoreItems { get; set; }
    }
}
