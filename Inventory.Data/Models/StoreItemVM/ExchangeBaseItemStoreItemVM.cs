
namespace Inventory.Data.Models.StoreItemVM
{
    public class ExchangeBaseItemStoreItemVM
    {
        public long BaseItemId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string BaseItemName { get; set; }
        public string UnitName { get; set; }
        public string Notes { get; set; }
        public string ItemStatus { get; set; }
        public string ReleaseDate { get; set; }
        
    }
}
