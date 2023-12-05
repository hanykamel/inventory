using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.Inquiry
{
    public class InquiryBaseItems
    {
        public int Count { get; set; }
        public List<InquiryBaseItem> BaseItems { get; set; }
    }

    public class InquiryBaseItem
    {
        public long BaseItemId { get; set; }
        public string BaseItemName { get; set; }
        public int? UnitId { get; set; }
        public string UnitName { get; set; }
        public long? BookId { get; set; }
        public int BookPageNumber { get; set; }
        public string Description { get; set; }
        public String Consumed { get; set; }
        public bool IsConsumed { get; set; }
        public int StoreItemsCount { get; set; }
    }
}
