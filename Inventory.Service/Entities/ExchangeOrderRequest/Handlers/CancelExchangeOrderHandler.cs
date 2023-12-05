using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Repository;
using Inventory.Service.Entities.ExchangeOrderRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.ExchangeOrderRequest.Handlers
{
    public class CancelExchangeOrderHandler : IRequestHandler<CancelExchangeOrderCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IExchangeOrderBussiness _exchangeOrderBussiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly ILogger<ExchangeOrderReviewHandler> _logger;
        private readonly INotificationBussiness _notificationBussiness;
        private readonly ITenantProvider _tenantProvider;
        readonly private IRepository<ExchangeOrderStoreItem, Guid> _exchangeOrderStoreItemRepository;


        public CancelExchangeOrderHandler(
            IMediator mediator,
            IExchangeOrderBussiness exchangeOrderBussiness,
            ILogger<ExchangeOrderReviewHandler> logger,
            INotificationBussiness notificationBussiness,
            ITenantProvider tenantProvider,
            IStoreItemBussiness storeItemBussiness,
            IRepository<ExchangeOrderStoreItem, Guid> exchangeOrderStoreItemRepository
            )
        {
            _mediator = mediator;
            _exchangeOrderBussiness = exchangeOrderBussiness;
            _logger = logger;
            _notificationBussiness = notificationBussiness;
            _tenantProvider = tenantProvider;
            _exchangeOrderStoreItemRepository = exchangeOrderStoreItemRepository;
            _storeItemBussiness = storeItemBussiness;
        }

        public async Task<bool> Handle(CancelExchangeOrderCommand request, CancellationToken cancellationToken)
        {
            var result = await _exchangeOrderBussiness.CancelExchangeOrder(request.ExchangeOrderId);
           
            return Task.FromResult(result).Result;
        }
    }
}
