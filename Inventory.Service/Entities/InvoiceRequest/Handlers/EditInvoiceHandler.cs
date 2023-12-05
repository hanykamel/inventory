using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.NotificationVM;
using Inventory.Service.Entities.InvoiceRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.InvoiceRequest.Handlers
{
    public class EditInvoiceHandler : IRequestHandler<EditInvoiceCommand, bool>
    {
        private readonly IStringLocalizer<SharedResource> _Localizer;
        private readonly IInvoiceBussiness _invoiceBussiness;

        private readonly IRefundOrderBussiness _refundOrderBussiness;
        private readonly ITenantProvider _tenantProvider;
        private readonly INotificationBussiness _notificationBussiness;
        public EditInvoiceHandler(IInvoiceBussiness invoiceBussiness, IRefundOrderBussiness refundOrderBussiness,
            ITenantProvider tenantProvider,
            INotificationBussiness notificationBussiness,
            IStringLocalizer<SharedResource> Localizer)
        {
            _invoiceBussiness = invoiceBussiness;
            _refundOrderBussiness = refundOrderBussiness;
            _tenantProvider = tenantProvider;
            _notificationBussiness = notificationBussiness;
            _Localizer = Localizer;
        }

        public async Task<bool> Handle(EditInvoiceCommand request, CancellationToken cancellationToken)
        {
          
      
            List<InvoiceStoreItem> allInvoiceStoreItem = new List<InvoiceStoreItem>();
            foreach (var item in request.storeItem)
            {
                InvoiceStoreItem invoiceStoreItem = new InvoiceStoreItem();
                invoiceStoreItem.Id = item.nvoiceStoreId;
                invoiceStoreItem.StoreItemId = item.storeitemId;
                invoiceStoreItem.StoreItem = new StoreItem();
                invoiceStoreItem.StoreItem.StoreItemStatusId = item.storeStatus;
                allInvoiceStoreItem.Add(invoiceStoreItem);
            }

            //update store items in the invoice that has been refunded
            var result = await _invoiceBussiness.EditeInvoice(allInvoiceStoreItem);
            if (result )
            {
                //get the rest of refund order storeitems
                var storeItems=_refundOrderBussiness.GetRefundOrderStoreItems(request.RefundOrderId,
                    allInvoiceStoreItem.Select(o=>o.StoreItemId).ToList());
                //check that all storeitems has been refunded so we can update refund order status to has been refunded
                if(_invoiceBussiness.checkInvoiceStoreItemRefunded(storeItems))
                     await _refundOrderBussiness.CompeleteRefundOrder(request.RefundOrderId);

            }
            result= await _invoiceBussiness.Savechange();

            //Send Notification
            NotificationVM notification = new NotificationVM();
            notification.Id = request.Invoice.Id.ToString();
            notification.code = request.Invoice.Code;
            notification.notificationTemplateEnum = NotificationTemplateEnum.NTF_Invoice_Edit;
            //notification.storeId = request.StoreId.ToString();
            notification.storeId = _tenantProvider.GetTenant().ToString();
            notification.FromStore = _tenantProvider.GetTenantName();
            await _notificationBussiness.SendNotification(notification);
            return Task.FromResult(result).Result;
        }
    }
}
