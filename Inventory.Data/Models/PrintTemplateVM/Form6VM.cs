using System.Collections.Generic;

namespace Inventory.Data.Models.PrintTemplateVM
{
    public class Form6VM
    {
        public string BudgetName { get; set; }
        public string StoreName { get; set; }
        public string StoreKeeper { get; set; }
        public string StockTakingDate { get; set; }
        public string TotalPrice { get; set; }
        public string Currency { get; set; }
        public List<string> CurrencyList { get; set; }
        public List<string> TotalPriceList { get; set; }
    }
}
