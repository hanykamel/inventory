using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.BaseItem;
using Inventory.Data.Models.ExchangeOrderVM;
using Inventory.Data.Models.Inquiry;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Service.Entities.InquiryRequest.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface IStoreItemBussiness
    {
        List<BaseItemStoreItemVM> GetFormStoreItems(List<Guid> storeItems);
        List<FormNo6StoreItemVM> GetFormNo6StoreItems(List<Guid> storeItems);

        bool AddNewStoreItem(StoreItem _storeItem);
        IQueryable<StoreItem> GetAllStoreItems(bool ignoreTentant = false);
        IEnumerable<StoreItem> GetBaseItemsLatestBooksAndPages(List<long> baseItemsIds);
        bool UpdateStoreItem(StoreItem _storeItem);
        StoreItem GetStoreItemsbyId(Guid StoreItemId);
        int GetMax(int budgetId, long baseItemId);
        void GenerateBarcode(List<StoreItem> storeItems, int budgetId, long baseItemId);
        List<StoreItemVM> GenerateBarcodeImages(Guid additionId);
        List<BaseItem> GetBaseItemsByBudgetId(int budgetId);
        void UpdateStoreItemStatus(List<Guid> storeItems, ItemStatusEnum validOldStatus, ItemStatusEnum newItemStatus);
        List<StoreItem> GetByInvoiceId(List<Guid> invoices);
        List<StoreItemExchangesVM> SearchStoreItemExchanges(Guid id);
        List<StoreItemVM> GetAllStoreItems(List<Guid> storeItemIds, bool ignoreTentant = false);
        void DeActivateStoreItem(List<Guid> storeItems);
        IQueryable<StoreItem> GetAvailableStoreItems();
        IQueryable<StoreItem> GetRobbingStoreItems();
        IQueryable<StagnantStoreItemVM> getStagnantStoreItemsByBaseItemId();
        IQueryable<StagnantBaseItemVM> GetStagnantStoreItems(DateTime stagnantDate);

        List<StoreItem> GetInquiryStoreItems(InquiryStoreItemsRequest inquiryRequest, out int count);
        InquiryBaseItems GetInquiryBaseItems(InquiryBaseItemsCommand inquiryRequest);
        Task<bool> EditStoreItemsBooksItems(EditStoreItemsBookCommand inquiryRequest);
        void UpdateStagnantStoreItemAsync(List<Guid> _storeItem);

        IQueryable<BaseItemBudgetVM> GetActiveBaseItemsBudget(int BudgetID, int CurrencyId, int? Status, int? CategoryId,string ContractNum,int? PageNum);
        IQueryable<BaseItemBudgetVM> GetActiveRobbingBaseItemsBudget(int BudgetID, int CurrencyId, int? Status, int? CategoryId, long[] SelectBaseItem ,string ContractNum,int? PageNum);
        Task<bool> ReenableStoreItem(List<Guid> storeItems);
        List<ExchangeOrderStoreItemVM> GetByExchangeOrderId(Guid exchangeOrderId);

        void MakeStoreItemUnderDelete(List<Guid> storeItems);

        void DeActivateTransformationStoreItem(Guid transformationId);
        void DeActivateRobbingStoreItem(Guid robbingId);
        void CancelStoreItemUnderDelete(List<Guid> storeItems);
        IQueryable<BaseItemBudgetVM> GetExecutionOrderBaseItemsBudget(int BudgetID, int CurrancyId, int? CategoryId, long[] SelectBaseItem);
        void MakeStoreItemUnderExecution(List<Guid> storeItems);
        IQueryable<StoreItem> GetTransformationAvailableStoreItems
           (long baseItemId, int budgetId, int statusId, int count, string contractNum, int pageNum);


       
    }
}
