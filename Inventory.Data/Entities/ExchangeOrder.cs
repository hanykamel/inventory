using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;

namespace Inventory.Data.Entities
{
    public  class ExchangeOrder:Entity<Guid>,IActive,ITenant,ICode
    {
        public ExchangeOrder()
        {
            ExchangeOrderStoreItem = new HashSet<ExchangeOrderStoreItem>();
        }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public int BudgetId { get; set; }
        public int OperationId { get; set; }
        public int ForEmployeeId { get; set; }
        public string DirectOrderNotes { get; set; }
        public int? ExchangeOrderStatusId { get; set; }
        public bool IsDirectOrder { get; set; }
        public string Notes { get; set; }
        public virtual Budget Budget { get; set; }
        public virtual ExchangeOrderStatus ExchangeOrderStatus { get; set; }
        public virtual Employees ForEmployee { get; set; }
        public virtual Operation Operation { get; set; }
        public virtual ICollection<ExchangeOrderStoreItem> ExchangeOrderStoreItem { get; set; }
        public virtual ICollection<ExchangeOrderStatusTracker> ExchangeOrderStatusTracker { get; set; }

        public virtual ICollection<Invoice > Invoice { get; set; }
        public int TenantId { get ; set; }
        public int Year { get; set; }
        public int Serial { get ; set ; }
        public bool? IsActive { get; set; }

        //public object Select()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
