using System.Collections.Generic;

namespace Inventory.Data.Models.PrintTemplateVM
{
    public class TransactionsVM
    {
        public string BudgetName { get; set; }
        public string StoreName { get; set; }
        public string RequesterName { get; set; }
        public string AdditionDocumentType { get; set; }
        public string Serial { get; set; }
        public string CreationDate { get; set; }
        public string RequestDate { get; set; }
        public bool IsAddition { get; set; }
        public string CustodyOwner { get; set; }
        public string Currency { get; set; }
        public List<string> Currencies { get; set; }

    }
}
