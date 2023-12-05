
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.AdditionVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface IExchangeOrderBussiness
    {
        ExchangeOrder GetById(Guid RefundOrderId);
        IQueryable<ExchangeOrder> GetAllExchangeOrder();
        IQueryable<ExchangeOrder> PrintExchangeOrders();
        List<StoreItem> SearchStoreItems(long? baseItemId, int? budgetId, int? itemCategoryId, int? pageNumber, string contractNumber ,List<Guid> SelectStoreItemId, out int count, out int takeitem, out int Expensesitem, out int available);
        Task<ExchangeOrder> Create(List<StoreItemVM> items, int budgetId, int forEmployeeId, bool isDirectOrder, string notes, string directNotes);
        string GetCode(int serial);
        int GetMax();
        bool ChangeStatus(Guid ExchangeOrderId, ExchangeOrderStatusEnum status);
        Task<bool> CancelExchangeOrder_Service();
        Task<bool> CancelExchangeOrder(Guid echangeOrderId);
        Task<bool> ChangeStatusReview(Guid ExchangeOrderId, ExchangeOrderStatusEnum status);
        List<ExchangeOrderStoreItem> GetExchangeOrderStoreItem(List<Guid> storeItemIds);
        List<ExchangeOrderStoreItem> GetExchangeOrderStoreItemsByBaseItem(List<long> baseItemIds);
        IQueryable<ExchangeOrder> GetAllExchangeOrderView();
        //List<StoreItem> SearchtopStoreItems(long baseItemId, int? budgetId, int? itemCategoryId, string contractNumber, int take, int statusitem);
    }
}
