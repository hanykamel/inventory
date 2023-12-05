using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class Invoice:Entity<Guid>,IActive,ITenant,ICode
    {
        public Invoice()
        {
            InvoiceStoreItem = new HashSet<InvoiceStoreItem>();
        }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public int ReceivedEmployeeId { get; set; }
        public int? DepartmentId { get; set; }
        public int TenantId { get; set; }
        public Guid ExchangeOrderId { get; set; }
        public int Year { get; set; }
        public int Serial { get; set; }
        public int LocationId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Employees ReceivedEmployee { get; set; }
        public virtual Department Department { get; set; }

        public virtual Location  Location { get; set; }
        public virtual ICollection<InvoiceStoreItem> InvoiceStoreItem { get; set; }
        public virtual ExchangeOrder ExchangeOrder { get; set; }
       
    }
}
