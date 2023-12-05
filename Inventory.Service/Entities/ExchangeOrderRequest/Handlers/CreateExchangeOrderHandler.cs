using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.AdditionVM;
using Inventory.Data.Models.NotificationVM;
using Inventory.Repository;
using Inventory.Service.Entities.AdditionRequest.Commands;
using Inventory.Service.Entities.ExchangeOrderRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.ExchangeOrderRequest.Handlers
{
    public class CreateExchangeOrderHandler : IRequestHandler<CreateExchangeOrderCommand, Guid>
    {
        private readonly IMediator _mediator;
        private readonly IStoreItemBussiness _storeItemBussiness;
        private readonly IExchangeOrderBussiness _exchangeOrderBussiness;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchStoreItemsExchangeOrderHandler> _logger; 
        private readonly INotificationBussiness _notificationBussiness;
        private readonly ITenantProvider _tenantProvider;
        private IStringLocalizer<SharedResource> _localizer;

        public CreateExchangeOrderHandler(
            IMediator mediator,
            IStoreItemBussiness storeItemBussiness,
            IExchangeOrderBussiness exchangeOrderBussiness,
            IUnitOfWork unitOfWork,
            ILogger<SearchStoreItemsExchangeOrderHandler> logger,
            INotificationBussiness notificationBussiness,
            ITenantProvider tenantProvider,
            IStringLocalizer<SharedResource> localizer
            )
        {
            _mediator = mediator;
            _storeItemBussiness = storeItemBussiness;
            _exchangeOrderBussiness = exchangeOrderBussiness;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _notificationBussiness = notificationBussiness;
            _tenantProvider = tenantProvider;
            _localizer = localizer;
        }
        public async Task<Guid> Handle(CreateExchangeOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.items == null || request.items.Count == 0 || request.forEmployeeId <= 0)
                    throw new InvalidModelStateException(_localizer["InvalidModelStateException"]);


                _storeItemBussiness.UpdateStoreItemStatus(request.items.Select(x=>x.Id).ToList(),ItemStatusEnum.Available, ItemStatusEnum.Reserved);

                ExchangeOrder result = await _exchangeOrderBussiness.Create(request.items, 
                    request.budgetId, request.forEmployeeId, request.isDirectOrder, request.notes , request.directNotes);

                 var id =Task.FromResult(result.Id).Result;
                //send notification to (مدير المخازن)
                //var storeId = _tenantProvider.GetTenant();
                NotificationVM notification = new NotificationVM();
                notification.Id = result.Id.ToString();
                notification.code = result.Code;
                notification.notificationTemplateEnum = request.isDirectOrder?NotificationTemplateEnum.NTF_DirectOrder_ExchangeOrder: NotificationTemplateEnum.NTF_Create_ExchangeOrder;
                notification.storeId = result.TenantId.ToString();//storeId.ToString();
                notification.FromStore = _tenantProvider.GetTenantName();
                await _notificationBussiness.SendNotification(notification);
                return id;
            }
            catch (InventoryExceptionBase)
            {
                throw new NotSavedException();
            }
        }

    }
}