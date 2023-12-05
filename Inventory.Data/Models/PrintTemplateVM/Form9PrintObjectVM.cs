using Inventory.Data.Models.StoreItemVM;
using System.Collections.Generic;

namespace Inventory.Data.Models.PrintTemplateVM
{
    public class Form9PrintObjectVM
    {
        public string Currency { get; set; }
        public List<DedcuctionStoreItemVM> value { get; set; }
    }
}
