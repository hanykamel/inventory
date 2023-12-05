using System.Collections.Generic;

namespace Inventory.Data.Models.PrintTemplateVM
{
    public class Form8VM
    {
        public string BudgetName { get; set; }
        public string StoreName { get; set; }
        public string StoreKeeper { get; set; }
        public string ExchangeDate { get; set; }
        public List<string> Currencies { get; set; }
    }
}
