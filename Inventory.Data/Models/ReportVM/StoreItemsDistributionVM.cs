using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.ReportVM
{
   public  class StoreItemsDistributionVM
    {

        public long Id { get; set; }
        public string BaseItemName { get; set; }
        public string ContractNumber { get; set; }
        public int BookNumber { get; set; }
        public int BudgetId { get; set; }
        public int PageNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public int TenantId { get; set; }
        public int CategoryId { get; set; }
        public int AllQuantity { get; set; }
        public int paidQuantity { get; set; }
      
        public int storeQuantity { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public List<DistributionDetailsVM> DistributionDetails = new List<DistributionDetailsVM>();

    }

    public class DistributionDetailsVM
    {
        public long Id { get; set; }
        public string BaseItemName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Amount { get; set; }
        public string Notes { get; set; }
        public int BookNumber { get; set; }
        public int BudgetId { get; set; }
        public int PageNumber { get; set; }
    }

    public class StoreItemsDistributionPrintVM
    {


        public long Id { get; set; }
        public string BaseItemName { get; set; }
        public string ContractNumber { get; set; }
        public int BookNumber { get; set; }
        public int PageNumber { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreationDate { get; set; }
        public int TenantId { get; set; }
        public int BudgetId { get; set; }
        public int AllQuantity { get; set; }
        public int paidQuantity { get; set; }
        public int storeQuantity { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public int Amount { get; set; }
        public string Notes { get; set; }
    }
}
