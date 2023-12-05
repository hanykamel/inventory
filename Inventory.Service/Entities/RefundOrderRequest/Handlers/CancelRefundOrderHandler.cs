using Inventory.CrossCutting.ExceptionHandling;
using Inventory.Data.Entities;
using Inventory.Repository;
using Inventory.Service.Entities.RefundOrderRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.RefundOrderRequest.Handlers
{
    public class CancelRefundOrderHandler : IRequestHandler<CancelRefundOrderCommand, bool>
    {
        private readonly IRefundOrderBussiness _refundOrderBussiness;
        private readonly ILogger<CancelRefundOrderHandler> _logger;
        private readonly INotificationBussiness _notificationBussiness;
        private readonly IInvoiceStoreItemBussiness _invoiceStoreItemBussiness;
        private readonly IRepository<RefundOrder, Guid> _refundOrderRepository;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public CancelRefundOrderHandler(
            IRefundOrderBussiness refundOrderBussiness,
            ILogger<CancelRefundOrderHandler> logger,
            INotificationBussiness notificationBussiness,
            IInvoiceStoreItemBussiness invoiceStoreItemBussiness,
            IRepository<RefundOrder, Guid> refundOrderRepository,
            IStringLocalizer<SharedResource> Localizer
            )
        {
            _refundOrderBussiness = refundOrderBussiness;
            _logger = logger;
            _notificationBussiness = notificationBussiness;
            _refundOrderRepository = refundOrderRepository;
            _Localizer = Localizer;
            _invoiceStoreItemBussiness = invoiceStoreItemBussiness;
        }

        public async Task<bool> Handle(CancelRefundOrderCommand request, CancellationToken cancellationToken)
        {
            var refundOrder =  _refundOrderBussiness.CancelRefundOrder(request.RefundOrderId);
            _invoiceStoreItemBussiness.CancelInvoiceStoreItemUnderRefund(refundOrder.RefundOrderStoreItem);
            var result = await _refundOrderRepository.SaveChanges();
            if (result <= 0)
            {
               throw new InvalidCanceledExchangeOrder(_Localizer["NotSavedException"]);
            }
            return Task.FromResult(true).Result;
        }
    }
}
