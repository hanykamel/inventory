using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.ExecutionOrderVM
{
   public class ExecutionOrderBaseItemModel
    {
        public long BaseItemId { get; set; }
        public string BaseItemName { get; set; }
        public string Discription { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        
        public IEnumerable<ExecutionOrderStoreItemModel> ExecutionOrderStoreItems { get; set; }
    }


    public class ExecutionOrderStoreItemModel
    {
        public Guid StoreItemId { get; set; }
        public string StoreItemCode { get; set; }
        public string status { get; set; }
        public int? CurrancyId { get; set; }

        public bool isApproved { get; set; }
    }
}
