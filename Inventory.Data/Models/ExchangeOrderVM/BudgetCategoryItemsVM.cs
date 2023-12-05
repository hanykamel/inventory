using Inventory.Data.Models.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.ExchangeOrderVM
{
    public class BudgetCategoryItemsVM
    {
        public object BaseItems { get; set; }
        public List<LookupVM<int>> Categories { get; set; }
    }

}

