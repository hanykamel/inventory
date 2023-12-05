using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.Data.Models.StoreItemVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Service.Interfaces
{
    public interface IRefundOrderBussiness
    {
        IQueryable<RefundOrder> GetAllRefundOrder();
        IQueryable<RefundOrderVM> PrintRefundOrdersList();
        Task<RefundOrder> Create(RefundOrder items);
        RefundOrder GetById(Guid RefundOrderId);
        Task<bool> ChangeStatus(Guid RefundOrderId, RefundOrderStatusEnum status);
        int GetMax();

        string GetCode(int serial);

        Task<bool> CompeleteRefundOrder(Guid RefundOrderId);

        List<DedcuctionStoreItemVM> GetTaintedItemsByRefundOrderId(Guid RefundOrderId);
        IQueryable<RefundOrder> GetAllRefundOrderView();
        RefundOrder CancelRefundOrder(Guid RefundOrderId);
        //IEnumerable<RefundInvoiceVM> GetRefundOrderForEditInvoice(Guid refundId);
        List<Guid> GetRefundOrderStoreItems(Guid RefundOrderId, List<Guid> notIncludeStoreItems);
    }  
}
