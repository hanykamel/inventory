using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.BaseItem
{
  public  class BaseItemBudgetVM
    {
        public Guid index { get; set; }
        public long  BaseItemID { get; set; }
        public string BaseItemName { get; set; }
        public string BaseItemdisc { get; set; }
        public string statusItem { get; set; }
        public string UnitName { get; set; }
        public int statusItemId { get; set; }
        public decimal price { get; set; }
        public int CountStoreItem { get; set; }
        public int PageNum { get; set; }

        public string ContractNum { get; set; }
        public IEnumerable<StoreItemBudgetVM> StoreItemsBudget { get; set; }
    }

    public class StoreItemBudgetVM
    {
        public string code { get; set; }
        public Guid StoreItemId { get; set; }
        public string StoreItemdisc { get; set; }
        public string statusItem { get; set; }
        public int statusItemId { get; set; }

    }

}
