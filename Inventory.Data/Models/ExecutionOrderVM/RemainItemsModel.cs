using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.ExecutionOrderVM
{

    public class RemainModel
    {
        public int BudgetId { get; set; }
        public string BudgetName { get; set; }

        public IEnumerable<RemainItemsModel> RemainItemsModels { get; set; }
    }
    public  class RemainItemsModel
    {
        public Guid Id { get; set; }
        public long RemainsId { get; set; }
        public string Remainsname{ get; set; }
        public Guid ExecutionOrderId { get; set; }
        public string ExecutionOrderCode { get; set; }
        public int Quantity { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public string Note { get; set; }
    }
}
