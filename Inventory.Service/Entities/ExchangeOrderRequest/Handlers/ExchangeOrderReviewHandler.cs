using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Entities;
using Inventory.Data.Enums;
using Inventory.Data.Models.NotificationVM;
using Inventory.Service.Entities.ExchangeOrderRequest.Commands;
using Inventory.Service.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Service.Entities.ExchangeOrderRequest.Handlers
{
   public class ExchangeOrderReviewHandler : IRequestHandler<ExchangeOrderReviewCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IExchangeOrderBussiness _exchangeOrderBussiness;
        private readonly ILogger<ExchangeOrderReviewHandler> _logger;
        private readonly INotificationBussiness _notificationBussiness;
        private readonly ITenantProvider _tenantProvider;
        private readonly IStoreBussiness _storeBussiness;
        private readonly IStringLocalizer<SharedResource> _Localizer;
        private IServiceScopeFactory factory;


        public ExchangeOrderReviewHandler(
            IMediator mediator,
            IExchangeOrderBussiness exchangeOrderBussiness,
            ILogger<ExchangeOrderReviewHandler> logger,
            INotificationBussiness notificationBussiness,
            ITenantProvider tenantProvider,
             IStoreBussiness storeBussiness,
              IServiceScopeFactory _factory,
             IStringLocalizer<SharedResource> Localizer
            )
        {
            _storeBussiness = storeBussiness;
            _mediator = mediator;
            _exchangeOrderBussiness = exchangeOrderBussiness;
            _logger = logger;
            _notificationBussiness = notificationBussiness;
            _tenantProvider = tenantProvider;
            _Localizer = Localizer;
            factory = _factory;
        }

        public async Task<bool> Handle(ExchangeOrderReviewCommand request, CancellationToken cancellationToken)
        {
            var exchange = _exchangeOrderBussiness.GetById(request.ExchangeOrderId);
            if (exchange.ExchangeOrderStatusId==(int)ExchangeOrderStatusEnum.Pending)
            {
                var result = await _exchangeOrderBussiness.ChangeStatusReview(request.ExchangeOrderId, ExchangeOrderStatusEnum.Reviewed);
                //send notification to (أمين المخزن)

              
                //var storeId = _tenantProvider.GetTenant();
             
              

                #region Background_thread_notification
                // Create new  Background thread for notification
                //send notification to (أمين المخزن)
                new Thread(() =>
                {
                    using (var scope = factory.CreateScope())
                    {
                        // create  service For this Scope and  Dispose when scope finish and thread finish
                        var _exchangeOrderSer = scope.ServiceProvider.GetRequiredService<IExchangeOrderBussiness>();
                        var _notificationSer = scope.ServiceProvider.GetRequiredService<INotificationBussiness>();
                        var _storeSer = scope.ServiceProvider.GetRequiredService<IStoreBussiness>();
                        // Use service

                        var exchangeOrder = _exchangeOrderSer.GetById(request.ExchangeOrderId);
                        var storeId = exchangeOrder.TenantId;
                        NotificationVM notification = new NotificationVM();
                        notification.Id = exchangeOrder.Id.ToString();
                        notification.code = exchangeOrder.Code;
                        notification.notificationTemplateEnum = NotificationTemplateEnum.NTF_Review_ExchangeOrder;
                        notification.storeId = storeId.ToString();
                        notification.FromStore = _storeSer.GetStoreName(storeId);
                        _notificationSer.SendNotification(notification).Wait();
                        notification.notificationTemplateEnum = NotificationTemplateEnum.NTF_ExchangeOrder_Review_TechManager;
                        _notificationSer.SendNotification(notification).Wait();
                        // Dispose scope
                    }
                }).Start();
                #endregion
                return Task.FromResult(result).Result;
            }
            else
            {
                throw new ReviewException(_Localizer["ChangeReviewExchangeorder"]);
            }
    
        }
    }
}
