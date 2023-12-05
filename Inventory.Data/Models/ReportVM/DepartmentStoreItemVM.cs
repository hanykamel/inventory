using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.ReportVM
{
    public class DepartmentStoreItemVM
    {
        public long Id { get; set; }
        public string BaseItemName { get; set; }
        public int BookNumber { get; set; }
        public int PageNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ContractNumber { get; set; }
        public int TenantId { get; set; }
        public int AllQuantity { get; set; }
        public int paidQuantity { get; set; }
        public int BudgetId { get; set; }
        public int storeQuantity { get; set; }
        public List<DepartmentDetailsVM> DepartmentDetails = new List<DepartmentDetailsVM>();


    }
    public class DepartmentDetailsVM
    {
        public long Id { get; set; }
        public int BookNumber { get; set; }
        public int PageNumber { get; set; }
        public string BaseItemName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string ReceiverName { get; set; }
        public string Location { get; set; }
        public int Amount { get; set; }
        public string Notes { get; set; }
        public int BudgetId { get; set; }
    }


    public class DepartmentStoreItemPrintVM
    {
        public long Id { get; set; }
        public string BaseItemName { get; set; }
        public int BookNumber { get; set; }
        public int PageNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ContractNumber { get; set; }
        public int TenantId { get; set; }
        public int AllQuantity { get; set; }
        public int paidQuantity { get; set; }
        public int storeQuantity { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string ReceiverName { get; set; }
        public string Location { get; set; }
        public int Amount { get; set; }
        public int BudgetId { get; set; }
        public string Notes { get; set; }
    }
}
