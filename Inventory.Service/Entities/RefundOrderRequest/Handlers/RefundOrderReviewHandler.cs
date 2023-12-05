using Inventory.CrossCutting.ExceptionHandling;
using Inventory.CrossCutting.Tenant;
using Inventory.Data.Enums;
using Inventory.Data.Models.NotificationVM;
using Inventory.Service.Entities.ExchangeOrderRequest.Commands;
using Inventory.Service.Entities.RefundOrderRequest.Commands;
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

namespace Inventory.Service.Entities.RefundOrderRequest.Handlers
{
   public class RefundOrderReviewHandler : IRequestHandler<RefundOrderReviewCommand, bool>
    {
        private readonly IMediator _mediator;
        private readonly IRefundOrderBussiness _rerfundOrderBussiness;
        private readonly ILogger<RefundOrderReviewHandler> _logger;
        private readonly INotificationBussiness _notificationBussiness;
        private readonly ITenantProvider _tenantProvider;
        private readonly IStoreBussiness _storeBussiness;
        private IServiceScopeFactory factory;
        private readonly IStringLocalizer<SharedResource> _Localizer;

        public RefundOrderReviewHandler(
            IMediator mediator,
            IRefundOrderBussiness refundOrderBussiness,
            ILogger<RefundOrderReviewHandler> logger,
            INotificationBussiness notificationBussiness,
            ITenantProvider tenantProvider,
             IStoreBussiness storeBussiness,
             IServiceScopeFactory _factory,
             IStringLocalizer<SharedResource> Localizer
            )
        {
            _mediator = mediator;
            _rerfundOrderBussiness = refundOrderBussiness;
            _logger = logger;
            _notificationBussiness = notificationBussiness;
            _tenantProvider = tenantProvider;
            _storeBussiness = storeBussiness;
            factory = _factory;
            _Localizer = Localizer;
        }

        public async Task<bool> Handle(RefundOrderReviewCommand request, CancellationToken cancellationToken)
        {
                var result = await _rerfundOrderBussiness.ChangeStatus(request.RefundOrderId, RefundOrderStatusEnum.Reviewed);
                #region Background_thread_notification
                // Create new  Background thread for notification
                //send notification to (أمين المخزن)
                new Thread(() =>
                {
                    using (var scope = factory.CreateScope())
                    {
                        // create  service For this Scope and  Dispose when scope finish and thread finish
                        var _rerfundOrderSer = scope.ServiceProvider.GetRequiredService<IRefundOrderBussiness>();
                        var _storeSer = scope.ServiceProvider.GetRequiredService<IStoreBussiness>();
                        var _notificationSer = scope.ServiceProvider.GetRequiredService<INotificationBussiness>();

                        // Use service

                        var refundOrder = _rerfundOrderSer.GetById(request.RefundOrderId);
                        var storeId = refundOrder.TenantId;
                        NotificationVM notification = new NotificationVM();
                        notification.Id = refundOrder.Id.ToString();
                        notification.code = refundOrder.Code;
                        notification.notificationTemplateEnum = NotificationTemplateEnum.NTF_Review_RefundOrder;
                        notification.storeId = storeId.ToString();
                        notification.FromStore = _storeSer.GetStoreName(storeId);
                        _notificationSer.SendNotification(notification).Wait();
                        notification.notificationTemplateEnum = NotificationTemplateEnum.NTF_RefundOrder_Review_TechManager;
                        _notificationSer.SendNotification(notification).Wait();
                        // Dispose scope
                    }
                }).Start();
                #endregion
                return Task.FromResult(result).Result;
      
        }

    }
}
