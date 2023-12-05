using Inventory.Data.Entities;
using Inventory.Data.Models.Inquiry;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Service.Entities.StockTakingRequest.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface IRobbedStoreItemBussiness
    {
        SearchStockTakingVM GetRobbedStoreItem(SearchStockTakingCommand request, out int count);
        StockTaking CreateStockTakingRobbing(CreateStockTakingCommand request, StockTaking NewStockTaking);
        List<FormNo6StoreItemVM> GetStocktackingRobbedStoreItems(List<Guid> RobbedstoreItems);
        List<RobbedStoreItem> GetRobbedStoreItemInquiry(InquiryStoreItemsRequest inquiryRequest, out int count);
    }
}
