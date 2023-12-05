using Inventory.Service.Interfaces;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Data.Entities;
using System.Linq;
using Inventory.CrossCutting.ExceptionHandling;
using Microsoft.Extensions.Localization;
using Inventory.Data.Models.PrintTemplateVM;
using Inventory.CrossCutting.Tenant;
using Inventory.Service.Entities.RefundOrderRequest.Commands;
using System.Collections.Generic;

namespace Inventory.Service.Entities.RefundOrderRequest.Handlers
{
    class PrintRefundOrderHandler : IRequestHandler<PrintFromNo9Command, MemoryStream>
    {
        private readonly IRefundOrderBussiness _refundOrderBussiness;
        private readonly IWordBusiness _wordBusiness;
        private IStringLocalizer<SharedResource> _localizer;
        private readonly ITenantProvider _tenantProvider;
        private readonly IExchangeOrderBussiness _exchangeOrderBussiness;

        public PrintRefundOrderHandler(IRefundOrderBussiness refundOrderBussiness,
            IStringLocalizer<SharedResource> localizer,
            ITenantProvider tenantProvider,
        IWordBusiness wordBusiness,
         IExchangeOrderBussiness exchangeOrderBussiness)
        {
            _refundOrderBussiness = refundOrderBussiness;
            _wordBusiness = wordBusiness;
            _localizer = localizer;
            _tenantProvider = tenantProvider;
            _exchangeOrderBussiness = exchangeOrderBussiness;
        }

        

        public Task<MemoryStream> Handle(PrintFromNo9Command request, CancellationToken cancellationToken)
        {

            RefundOrder refundOrder = _refundOrderBussiness.GetById(request.RefundOrderId);
            Form8VM refundOrderVM = new Form8VM
            {
                ExchangeDate = refundOrder.Date.ToString("yyyy / MM / dd "),
                StoreName = _tenantProvider.GetTenantName(),
                StoreKeeper = refundOrder.RefundOrderEmployee.Name
            };
            refundOrderVM.Currencies = new List<string>();
            var refundOrderStoreItems = _refundOrderBussiness.GetTaintedItemsByRefundOrderId(request.RefundOrderId);
            if (refundOrderStoreItems == null|| refundOrderStoreItems.Count()==0)
            {
                throw new InvalidInvoiceException(_localizer["RefundOrderHasNoTaintedItems"]);
            }
            var storeItemsIds = refundOrderStoreItems.Select(r => r.BaseItemId).ToList();
            var ExchangeOrderStoreItems = _exchangeOrderBussiness.GetExchangeOrderStoreItemsByBaseItem(storeItemsIds);
            if (ExchangeOrderStoreItems == null || ExchangeOrderStoreItems.Count() == 0)
            {
                throw new InvalidInvoiceException(_localizer["InvoiceHasNoItems"]);
            }
            foreach (var item in refundOrderStoreItems)
            {
                item.Date = ExchangeOrderStoreItems.Where(a => a.StoreItem.BaseItemId == item.BaseItemId).FirstOrDefault().CreationDate;
            }
            if (refundOrderVM != null)
            { 
                return  Task.FromResult(_wordBusiness.printRefundFormNo9Document
                    (refundOrderVM, refundOrderStoreItems));
            }
            return  Task.FromResult(new MemoryStream());
        }

    }
}
