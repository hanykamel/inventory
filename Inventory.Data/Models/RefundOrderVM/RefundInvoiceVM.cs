using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.RefundOrderVM
{
   public class RefundInvoiceVM
    {
        public Guid id { get; set; }
        public string Code { get; set; }
        public string Date { get; set; }
        public int ExaminationEmployeeId { get; set; }
        public string ExaminationEmployeeName { get; set; }
        public string DirectOrderNotes { get; set; }
        public List<Invoice> AllList { get; set; }

        public Guid RefunditemStoreId { get; set; }
        public string statusRefund { get; set; }

    }

    public class Invoice
    {
     
        public string InvoiceCode { get; set; }
        public Guid InvoiceId { get; set; }
        public string InvoiceDate { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }

        public List<object> Allitem { get; set; }
    }

    public class item
    {
        public string nvoiceStoreId { get; set; }
        public string itemStoreId { get; set; }
        public string ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemStatus { get; set; }
        public int ItemStatusId { get; set; }
        public bool Isrefundexist { get; set; }

    }






}
