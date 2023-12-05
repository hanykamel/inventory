using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Data.Models.Inquiry
{
    public class InquiryStoreItemsRequest
    {
        public int? categoryId { get; set; }
        public int? baseItemId { get; set; }
        public string contractNumber { get; set; }
        public bool? consumed { get; set; }
        public int? StoreItemStatus { get; set; }
        public int? storeId { get; set; }
        public int? budgetId { get; set; }
        public int? StoreItemAvailibilityStatusId { get; set; }
        public int? BookNumberFrom { get; set; }
        public int? BookNumberTo { get; set; }

        public int? BookPageNumberFrom { get; set; }
        public int? BookPageNumberTo { get; set; }
        public int? skip { get; set; }
        public int? take { get; set; }
    }
}
