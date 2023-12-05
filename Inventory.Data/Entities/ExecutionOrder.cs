using Inventory.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Entities
{
    public class ExecutionOrder: Entity<Guid>, IActive, ITenant, ICode
    {
        public ExecutionOrder()
        {
        }
        public string Code { get; set; }
        public string Notes { get; set; }
        public string ReviewNotes { get; set; }
        public DateTime Date { get; set; }
        public int BudgetId { get; set; }
        public int OperationId { get; set; }
        public int TenantId { get; set; }
        public int StoreId { get; set; }
        public int ExecutionOrderStatusId { get; set; }
        public bool? IsActive { get; set; }
        public virtual Budget Budget { get; set; }
        public virtual Operation Operation { get; set; }
        public virtual Store Store { get; set; }
        public virtual ExecutionOrderStatus ExecutionOrderStatus { get; set; }
        public virtual ICollection<ExecutionOrderAttachment> ExecutionOrderAttachment { get; set; }
        public virtual ICollection<ExecutionOrderStoreItem> ExecutionOrderStoreItem { get; set; }
        public virtual ICollection<ExecutionOrderStatusTracker> ExecutionOrderStatusTracker { get; set; }
        public virtual ICollection<ExecutionOrderResultItem> ExecutionOrderResultItem { get; set; }
        public virtual ICollection<ExecutionOrderResultRemain> ExecutionOrderResultRemain { get; set; }
        public virtual ICollection<Subtraction> Subtraction { get; set; }

        public int Year { get; set; }
        public int Serial { get; set; }
    }
}
