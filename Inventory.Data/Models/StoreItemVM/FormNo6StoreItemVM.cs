using System;


namespace Inventory.Data.Models.StoreItemVM
{
    public class FormNo6StoreItemVM
    {
        public int BookNumber { get; set; }
        public int PageNumber { get; set; }
        public string ContractNumber { get; set; }
        public long BaseItemId { get; set; }
        public string Currency { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string BaseItemName { get; set; }
        public string UnitName { get; set; }
        public string Notes { get; set; }
        public string Description { get; set; }
        public string ItemStatus { get; set; }
        public long BookId { get; set; }
        public int? CurrencyId { get; set; }
        public int? UnitId { get; set; }
        public DateTime Date { get; set; }
    }

}
