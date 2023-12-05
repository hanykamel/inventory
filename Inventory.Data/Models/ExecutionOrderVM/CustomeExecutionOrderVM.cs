using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Data.Models.ExecutionOrderVM
{
    public class CustomeExecutionOrderVM
    {
        public int TenantId { get; set; }
        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public bool ItemsNotAddedCount { get; set; }
        public bool RemainItemAdded { get; set; }
        public int ExecutionOrderStatusId { get; set; }
        public DateTime CreationDate { get; set; }
        public int? SubtractionNum { get; set; }
        public IQueryable<SubtractionVM> SubtractionModel { get; set; }
        public ExecutionOrderStatusVM ExecutionOrderStatus { get; set; }
    }
    public class ExecutionOrderStatusVM
    {
        public int Id { get; set; }
        public String Name { get; set; }
    }

    public class SubtractionVM
    {
        public Guid Id { get; set; }
        public DateTime date { get; set; }
        public int? SubtractionNum { get; set; }
    }



}
