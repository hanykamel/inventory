using Inventory.CrossCutting.Tenant;
using Inventory.Data.Enums;
using Inventory.Data.Models.NotificationVM;
using Inventory.Service.Entities.InvoiceRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.InvoiceRequest.Handlers
{
    class EditInvoiceDataHandler : IRequestHandler<EditInvoiceDataCommand, bool>
    {

        private readonly IInvoiceBussiness _invoiceBussiness;
        private readonly ITenantProvider _tenantProvider;
        private readonly INotificationBussiness _notificationBussiness;

        public EditInvoiceDataHandler(IInvoiceBussiness invoiceBussiness, ITenantProvider tenantProvider,
            INotificationBussiness notificationBussiness)
        {
            _invoiceBussiness = invoiceBussiness;
            _tenantProvider = tenantProvider;
            _notificationBussiness = notificationBussiness;
        }

        public async Task<bool> Handle(EditInvoiceDataCommand request, CancellationToken cancellationToken)
        {
           var invoice= _invoiceBussiness.GetInvoiceById(request.Id);
            invoice.LocationId = request.LocationId;
            invoice.DepartmentId = request.DepartmentId;
            invoice.ReceivedEmployeeId = request.ReceivedEmployeeId;
            invoice.Date = Convert.ToDateTime(request.Date);
            var result = await _invoiceBussiness.Savechange();

            //Send Notification
            NotificationVM notification = new NotificationVM();
            notification.Id = invoice.Id.ToString();
            notification.code = invoice.Code;
            notification.notificationTemplateEnum = NotificationTemplateEnum.NTF_Invoice_Edit;
            //notification.storeId = request.StoreId.ToString();
            notification.storeId = invoice.TenantId.ToString();
            notification.FromStore = _tenantProvider.GetTenantName();
            await _notificationBussiness.SendNotification(notification);


            return await Task.FromResult(result);
        }
    }
}
