using Inventory.Data.Entities;
using Inventory.Data.Models.ExecutionOrderVM;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.Data.Models.StoreItemVM;
using Inventory.Service.Entities.AdditionRequest.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface IExecutionOrderBussiness
    {
        IQueryable<ExecutionOrder> GetAllExecutionOrders();

        string GetCode();

        Task<Guid> AddNewExecutionOrder(ExecutionOrder ExecutionOrder);
        ExecutionOrder GetById(Guid Id);
        Task<bool> Save();
        Task<bool> CancelExecutionOrderAsync(Guid Id);
        bool ReviewExecutionOrderAsync(Guid Id,string reviewNote, List<Guid> storeitemReviewId, List<Guid> storeitemUnReviewId, List<Attachment> allAttachment);
        IQueryable<ExecutionOrder> GetAllExecutionOrdersView();
        IQueryable<CustomeExecutionOrderVM> GetCustomeExecutionOrdersList();
        IQueryable<ExecutionListVM> GetExecutionOrdersList();
        IEnumerable<ExecutionOrderBaseItemModel> GetExecutionOrderStoreItemReview(Guid Id);
        Task<bool> AddNewExecutionOrderReminsAndItem(Guid executionOrdeid, List<ExecutionOrderResultItem> AllExecutionOrderResultItem, List<ExecutionOrderResultRemain> AllExecutionOrderResultRemain, List<Attachment> allAttachment);

        ExecutionOrder GetExecutionOrderById(Guid Id);
        IQueryable<RemainItemsModel> GetRobbingRemainItems(long? RemainId, Guid[] SelectExecutionOrderRemainItemId, int BudgetId);
        void UpdateExecutionOrderAfterAddition(Guid? executionOrderId, List<Guid> committeeItemids, List<string> notes);
        IQueryable<RemainModel> getRemainItemRobbing(Guid ExecutionOrderId);
    }
}

