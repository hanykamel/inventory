using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.NotificationVM;
using Inventory.Service.Entities.InvoiceRequest.Commands;
using Inventory.Service.Implementation;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.InvoiceRequest.Handlers
{
    public class AddInvoiceHandler : IRequestHandler<AddInvoiceCommand, List<Guid>>
    {

        private readonly IInvoiceBussiness _invoiceBussiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly IExchangeOrderBussiness _iexchangeOrderBussiness;
        private readonly INotificationBussiness _notificationBussiness;
        private readonly ITenantProvider _tenantProvider;
        private IServiceScopeFactory factory;


        public AddInvoiceHandler(IInvoiceBussiness invoiceBussiness,
            IStoreItemBussiness storeItemBussiness, IExchangeOrderBussiness iexchangeOrderBussiness,
            INotificationBussiness notificationBussiness,
            ITenantProvider tenantProvider,
            IServiceScopeFactory _factory
            )
        {
            _invoiceBussiness = invoiceBussiness;
            _storeItemBussiness = storeItemBussiness;
            _iexchangeOrderBussiness = iexchangeOrderBussiness;
            _notificationBussiness = notificationBussiness;
            _tenantProvider = tenantProvider;
            factory = _factory;
        }
        public async Task<List<Guid>> Handle(AddInvoiceCommand request, CancellationToken cancellationToken)
        {

            List<Invoice> allInvoice = new List<Invoice>();
            //DateTime d2 = DateTime.Parse(request.Date, null, System.Globalization.DateTimeStyles.RoundtripKind);
            if (request.checkInvoice == false)
            {
                Invoice invoice = new Invoice();
                invoice.Id = Guid.NewGuid();
                invoice.Code = request.Code;
                invoice.DepartmentId = request.DepartmentId;
                invoice.ExchangeOrderId = request.ExchangeOrderId;
                invoice.ReceivedEmployeeId = request.ReceivedEmployeeId;
                invoice.Date = Convert.ToDateTime(request.Date);
                invoice.LocationId = request.LocationId;
                foreach (var item in request.InvoiceStoreItem)
                {
                    InvoiceStoreItem invoiceStoreItem = new InvoiceStoreItem();
                    // invoiceStoreItem.InvoiceId = invoice.Id;
                    invoiceStoreItem.StoreItemId = item;
                    invoiceStoreItem.Id = Guid.NewGuid();
                    invoice.InvoiceStoreItem.Add(invoiceStoreItem);

                }
                allInvoice.Add(invoice);
                //update storeItemStatus
                //_storeItemBussiness.UpdateStoreItemStatus(request.InvoiceStoreItem, ItemStatusEnum.Expenses);
                //return await _invoiceBussiness.AddNewInvoice(allInvoice);
            }
            else
            {

                foreach (var item in request.InvoiceStoreItem)
                {
                    Invoice invoice = new Invoice();
                    invoice.Id = Guid.NewGuid();
                    invoice.Code = request.Code;
                    invoice.DepartmentId = request.DepartmentId;
                    invoice.ExchangeOrderId = request.ExchangeOrderId;
                    invoice.ReceivedEmployeeId = request.ReceivedEmployeeId;
                    invoice.Date = Convert.ToDateTime(request.Date);
                    invoice.LocationId = request.LocationId;
                    InvoiceStoreItem invoiceStoreItem = new InvoiceStoreItem();
                    //  invoiceStoreItem.InvoiceId = invoice.Id;
                    invoiceStoreItem.StoreItemId = item;
                    invoiceStoreItem.Id = Guid.NewGuid();
                    invoice.InvoiceStoreItem.Add(invoiceStoreItem);
                    allInvoice.Add(invoice);
                }

                //update storeItemStatus

            }
            if (request.statusExchangeOrder == true)
            {
                _iexchangeOrderBussiness.ChangeStatus(request.ExchangeOrderId, ExchangeOrderStatusEnum.PaidOff);
            }
            _storeItemBussiness.UpdateStoreItemStatus(request.InvoiceStoreItem, ItemStatusEnum.Reserved, ItemStatusEnum.Expenses);

            _invoiceBussiness.AddAllInvoice(allInvoice);
            var ids = await _invoiceBussiness.SaveAllInvoice(allInvoice);
            #region Background_thread_notification
            // Create new  Background thread for notification
            new Thread(() =>
            {
                using (var scope = this.factory.CreateScope())
                {
                    // create  service For this Scope and  Dispose when scope finish and thread finish
                    var _tenantProviderSer = scope.ServiceProvider.GetRequiredService<ITenantProvider>();
                    var _notificationSer = scope.ServiceProvider.GetRequiredService<INotificationBussiness>();

                    // Use service

                    var storeId = _tenantProviderSer.GetTenant();
                    NotificationVM notification = new NotificationVM();
                    notification.notificationTemplateEnum = NotificationTemplateEnum.NTF_Invoice;
                    notification.storeId = storeId.ToString();
                    notification.FromStore = _tenantProviderSer.GetTenantName();
                    foreach (var invoice in allInvoice)
                    {
                        //send notification to (مدير الإدارة الفنية و مدير المخازن)
                        notification.Id = invoice.Id.ToString();
                        notification.code = invoice.Code;
                        _notificationSer.SendNotification(notification).Wait();
                    }

                    // Dispose scope
                }
            }).Start();
            #endregion
            return ids;


        }
    }
}
