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
    public class ReEnableExchangeOrderStoreItemsHandler : IRequestHandler<ReEnableExchangeOrderStoreItemsCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IExchangeOrderBussiness _exchangeOrderBussiness;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly ILogger<ExchangeOrderReviewHandler> _logger;
        private readonly INotificationBussiness _notificationBussiness;
        private readonly ITenantProvider _tenantProvider;
        readonly private IRepository<ExchangeOrderStoreItem, Guid> _exchangeOrderStoreItemRepository;


        public ReEnableExchangeOrderStoreItemsHandler(
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

        public async Task<bool> Handle(ReEnableExchangeOrderStoreItemsCommand request, CancellationToken cancellationToken)
        {
            //var StoreItemIds = _exchangeOrderBussiness.GetById(request.ExchangeOrderId).ExchangeOrderStoreItem.Select(e=>e.StoreItemId).ToList();
          
         
             var statusResult=  _exchangeOrderBussiness.ChangeStatus(request.ExchangeOrderId,ExchangeOrderStatusEnum.ItemsAvailable);
            if (statusResult)
            {
                var result = await _storeItemBussiness.ReenableStoreItem(request.storeItemsIds);
                return result;
            }
            return statusResult;
        }
    }
}
