using Inventory.Data.Entities;
using Inventory.Data.Models.StockTakingVM;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Service.Entities.StockTakingRequest.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface IStockTakingBussiness
    {
        IQueryable<StockTaking> GetAllStockTaking();
        StockTaking GetById(Guid Id);
        string GetCode();
        string GetLastCode();
        IQueryable<StockTaking> GetStockTakingview();
        SearchStockTakingVM SearchStoreItems(SearchStockTakingCommand request, out int count);
        StockTaking Create(CreateStockTakingCommand request, List<StockTakingAttachment> attachments);
        List<StockTakingRobbedStoreItem> GetStockTakingRobbedStoreItem(Guid Id);
         List<StockTakingStoreItem> GetStockTakingStoreItems(Guid Id);
         Task<bool> SaveChange();
    }
}
