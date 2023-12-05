using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Service.Entities.InquiryRequest.Commands
{
   public class InquiryStoreItemsModel
    {
        public int Count { get; set; }
        public List<StoreItemsModel> StoreItemsModel { get; set; }
       
    }



    public class StoreItemsModel
    {
        public Guid Id{ get; set; }
        public string itemCode { get; set; }
        public string itemName { get; set; }
        public string contractNumber { get; set; }
        public int pageNumber { get; set; }
        public long BaseItemId { get; set; }
        public string unit { get; set; }
        public string store { get; set; }
        public string storeItemStatus { get; set; }
        public int storeItemStatusId { get; set; }
        public int CurrentItemStatusId { get; set; }
        public int? BudgetId { get; set; }
        public string storeItemAvalibleStatus { get; set; }
        public bool? UnderDelete { get; set; }

    }
}
